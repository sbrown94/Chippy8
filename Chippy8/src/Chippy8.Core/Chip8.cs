using Chippy8.GUI;
using System.Collections;
using System.Timers;

namespace Chippy8.Core
{
    public class Chip8
    {
        private IMemory _memory;
        private IScreen _screen;
        private IStack _stack;
        private ICounter _counter;
        private IRegisters _registers;
        private IInput _input;
        private ITimers _timers;
        private IWindow _window;

        private int _gameState;
        private bool _terminating;

        public Chip8(IMemory memory, IScreen screen, IStack stack, ICounter counter, IRegisters registers, IInput input, ITimers timers, IWindow window)
        {
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _screen = screen ?? throw new ArgumentNullException(nameof(screen));
            _stack = stack ?? throw new ArgumentNullException(nameof(stack));
            _counter = counter ?? throw new ArgumentNullException(nameof(counter));
            _registers = registers ?? throw new ArgumentNullException(nameof(registers));
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _timers = timers ?? throw new ArgumentNullException(nameof(timers));
            _window = window ?? throw new ArgumentNullException(nameof(window));

            _gameState = 0; // 0 = running, 1 = paused, 2 = wait for input
            _terminating = false; // shutdown command issued

        }

        public void Boot(string programPath)
        {

            _memory.LoadProgram(programPath);
            _window.Init();

            while (!_terminating)
            {
            _window.Render(_screen.GetScreen());
                switch (_gameState)
                {
                    case 0:

                        _input.Capture();

                        if (_counter.Get() >= 2048)
                        {
                            _counter.SetTo(2047);
                            for (int y = 0; y < 32; y++)
                            {
                                for (int x = 0; x < 64; x++)
                                {
                                    if (_screen.GetScreen()[x, y])
                                    {
                                        Console.Write("X");
                                    }
                                    else
                                    {
                                        Console.Write(".");
                                    }
                                }
                                Console.WriteLine();
                            }
                        }

                        ExecuteInstruction(_memory.Read(_counter.Get(), 1)[0]);
                        _counter.Increment();
                        break;

                    case 1:
                        //pause
                        break;

                    case 2:
                        if (WaitForInput())
                            _gameState = 0;
                        break;
                }
            }
        }

        // Refer to http://devernay.free.fr/hacks/chip8/C8TECH10.HTM#3.1 for instruction documentation
        public void ExecuteInstruction(ushort instruction)
        {
            var DEBUG = instruction.ToString("X4");

            switch (instruction & 0xF000)
            {
                case 0x0000:

                    switch (instruction)
                    {
                        case 0x00E0:    // CLS
                            _screen.ClearScreen();
                            break;

                        case 0x00EE:    // RET
                            _counter.SetTo(_stack.Pop());
                            break;
                        default:
                            // ignore 0xnnn
                            break;
                    }
                    break;

                case 0x1000:    // JP addr
                    _counter.SetTo(ValAtIdx(instruction, 2, 3));
                    break;

                case 0x2000:    // CALL addr
                    _stack.Push((byte)_counter.Get());
                    _counter.SetTo(ValAtIdx(instruction, 2, 3));
                    break;

                case 0x3000:    // SE Vx, byte
                    if (_registers.GetVReg(ValAtIdx(instruction, 2, 1)) == (ValAtIdx(instruction, 3, 2)))
                        _counter.Increment();
                    break;

                case 0x4000:    // SNE Vx, byte
                    if (_registers.GetVReg(ValAtIdx(instruction, 2, 1)) != (ValAtIdx(instruction, 3, 2)))
                        _counter.Increment();
                    break;

                case 0x5000:    // SE Vx, Vy
                    if (_registers.GetVReg(ValAtIdx(instruction, 2, 1)) == _registers.GetVReg(ValAtIdx(instruction, 3, 1)))
                        _counter.Increment();
                    break;

                case 0x6000:    // LD Vx, byte
                    _registers.SetVReg(ValAtIdx(instruction, 2, 1), ValAtIdx(instruction, 3, 2));
                    break;

                case 0x7000:    // ADD Vx, byte
                    _registers.AddAtVReg(ValAtIdx(instruction, 2, 1), ValAtIdx(instruction, 3, 2));
                    break;

                case 0x8000:
                    switch (instruction & 0x000F)
                    {
                        case 0x0001:    // LD Vx, Vy
                            _registers.SetVReg(ValAtIdx(instruction, 2, 1), ValAtIdx(instruction, 3, 1));
                            break;

                        case 0x0002:    // OR Vx, Vy
                            _registers.BitwiseOrToVx(ValAtIdx(instruction, 2, 1), ValAtIdx(instruction, 3, 1));
                            break;

                        case 0x0003:    // XOR Vx, Vy
                            _registers.BitwiseXorToVx(ValAtIdx(instruction, 2, 1), ValAtIdx(instruction, 3, 1));
                            break;

                        case 0x0004:    // ADD Vx, Vy
                            _registers.AddToVxAndCarryToVf(ValAtIdx(instruction, 2, 1), ValAtIdx(instruction, 3, 1));
                            break;

                        case 0x0005:    // SUB Vx, Vy
                            _registers.SubFromVxAndCarryToVf(ValAtIdx(instruction, 2, 1), ValAtIdx(instruction, 3, 1));
                            break;

                        case 0x0006:    // SHR Vx {, Vy}
                            _registers.ShrVx(ValAtIdx(instruction, 2, 1), ValAtIdx(instruction, 3, 1));
                            break;

                        case 0x0007:    // SUBN Vx, Vy
                            _registers.SubnVx(ValAtIdx(instruction, 2, 1), ValAtIdx(instruction, 3, 1));
                            break;

                        case 0x0008:    // SHL Vx {, Vy}
                            _registers.ShlVx(ValAtIdx(instruction, 2, 1), ValAtIdx(instruction, 3, 1));
                            break;
                    }
                    break;

                case 0x9000:    // SNE Vx, Vy
                    if (!_registers.AreEqual(ValAtIdx(instruction, 2, 1), ValAtIdx(instruction, 3, 1)))
                        _counter.Increment();
                    break;

                case 0xA000:    // LD I, addr
                    _registers.SetIReg(ValAtIdx(instruction, 2, 3));
                    break;

                case 0xB000:    // JP V0, addr
                    _counter.SetTo(_registers.GetVReg(0));
                    break;

                case 0xC000:    // RND Vx, byte
                    _registers.BitwiseAndWithInputToVx(ValAtIdx(instruction, 2, 1), new Random().Next(255), ValAtIdx(instruction, 3, 2));
                    break;

                case 0xD000:    // DRW Vx, Vy, nibble
                    var xCrdBase = _registers.GetVReg(ValAtIdx(instruction, 2, 1)) % 64;
                    var yCrdBase = _registers.GetVReg(ValAtIdx(instruction, 3, 1)) % 32;

                    _registers.SetVReg(15, 0);

                    for (var i = 0; i < ((int)ValAtIdx(instruction, 4, 1)); i++)
                    {
                        var memDat = _memory.ReadByte((ushort)(_registers.GetIReg() + i));
                        var sprBitArr = new BitArray(8);

                        for (int bit = 0; bit < 8; bit++)
                        {
                            bool isBitSet = (memDat & (1 << (7 - bit))) != 0;
                            sprBitArr[bit] = isBitSet;
                        }

                        for (var j = 0; j < sprBitArr.Length; j++)
                        {
                            if (sprBitArr[j])
                            {
                                var xPixel = (xCrdBase + j) % 64;
                                var yPixel = (yCrdBase + i) % 32;
                                if (_screen.InvertPixelAndReturnShouldSetVF(xPixel, yPixel))
                                    _registers.SetVReg(15, 1);
                            }
                        }
                    }
                    break;

                case 0xE000:
                    switch (instruction & 0x00FF)
                    {
                        case 0x009E:    // SKP Vx
                            if (_input.IsPressed(ValAtIdx(instruction, 2, 1)))
                                _counter.Increment();
                            break;

                        case 0x00A1:    // SKNP Vx
                            if (!_input.IsPressed(ValAtIdx(instruction, 2, 1)))
                                _counter.Increment();
                            break;
                    }
                    break;

                case 0xF000:
                    switch (instruction & 0x00FF)
                    {
                        case 0x0007:    // LD Vx, DT
                            _registers.SetVReg(ValAtIdx(instruction, 2, 1), _registers.GetDelayReg());
                            break;

                        case 0x000A:    // LD Vx, K
                            _gameState = 2;
                            break;

                        case 0x0015:    // LD DT, Vx
                            _registers.SetDelayReg((byte)(ValAtIdx(instruction, 2, 1)));
                            break;

                        case 0x0018:    // LD ST, Vx
                            _registers.SetSoundReg((byte)(ValAtIdx(instruction, 2, 1)));
                            break;

                        case 0x001E:    // ADD I, Vx
                            _registers.SetIReg((ushort)(_registers.GetVReg(ValAtIdx(instruction, 2, 1)) + _registers.GetIReg()));
                            break;

                        case 0x0029:    // LD F, Vx
                            //_registers.SetIReg()
                            break;
                    }
                    break;

            }
        }

        public ushort ValAtIdx(ushort num, int index, int length = 1) =>
            (ushort)Convert.ToInt32(num.ToString("X4").Substring(index-1, length), 16);


        public bool WaitForInput()
        {
            //var input = _input
            return false;
            //var input = _input.WaitForInput
        }
    }
}
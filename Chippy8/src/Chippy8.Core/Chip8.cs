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

        private int _gameState;
        private bool _terminating;

        public Chip8(IMemory memory, IScreen screen, IStack stack, ICounter counter, IRegisters registers, IInput input, ITimers timers)
        {
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _screen = screen ?? throw new ArgumentNullException(nameof(screen));
            _stack = stack ?? throw new ArgumentNullException(nameof(stack));
            _counter = counter ?? throw new ArgumentNullException(nameof(counter));
            _registers = registers ?? throw new ArgumentNullException(nameof(registers));
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _timers = timers ?? throw new ArgumentNullException(nameof(timers));

            _gameState = 0; // 0 = running, 1 = paused, 2 = wait for input
            _terminating = false; // shutdown command issued

        }

        public void Boot()
        {
            var timer = new System.Timers.Timer(700);

            while (!_terminating)
            {
                timer.Elapsed += (sender, e) =>
                {
                    switch (_gameState)
                    {
                        case 0:

                            _input.Capture();
                            //ExecuteInstruction();
                            //TODO: export to GUI library
                            break;

                        case 1:
                            //pause
                            break;

                        case 2:
                            if (WaitForInput())
                                _gameState = 0;
                            break;
                    }
                };
            }

            
        }

        // Refer to http://devernay.free.fr/hacks/chip8/C8TECH10.HTM#3.1 for instruction documentation
        public void ExecuteInstruction(byte instruction)
        {
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
                            throw new InvalidOperationException("not implemented");
                    }
                    break;

                case 0x1000:    // JP addr
                    _counter.SetTo(instruction & 0x0FFF);
                    break;

                case 0x2000:    // CALL addr
                    _stack.Push((byte)_counter.Get());
                    _counter.SetTo(instruction & 0x0FFF);
                    break;

                case 0x3000:    // SE Vx, byte
                    if (_registers.GetVReg(instruction * 0x0F00) == (instruction & 0x00FF))
                        _counter.Increment();
                    break;

                case 0x4000:    // SNE Vx, byte
                    if (_registers.GetVReg(instruction * 0x0F00) != (instruction & 0x00FF))
                        _counter.Increment();
                    break;

                case 0x5000:    // SE Vx, Vy
                    if (_registers.GetVReg(instruction * 0x0F00) == _registers.GetVReg(instruction * 0x00F0))
                        _counter.Increment();
                    break;

                case 0x6000:    // LD Vx, byte
                    _registers.SetVReg(instruction & 0x0F00, instruction & 0x00FF);
                    break;

                case 0x7000:    // ADD Vx, byte
                    _registers.AddAtVReg(instruction & 0x0F00, instruction & 0x00FF);
                    break;

                case 0x8000:
                    switch (instruction & 0x000F)
                    {
                        case 0x0001:    // LD Vx, Vy
                            _registers.SetVReg(instruction & 0x0F00, instruction & 0x00F0);
                            break;

                        case 0x0002:    // OR Vx, Vy
                            _registers.BitwiseOrToVx(instruction & 0x0F00, instruction & 0x00F0);
                            break;

                        case 0x0003:    // XOR Vx, Vy
                            _registers.BitwiseXorToVx(instruction & 0x0F00, instruction & 0x00F0);
                            break;

                        case 0x0004:    // ADD Vx, Vy
                            _registers.AddToVxAndCarryToVf(instruction & 0x0F00, instruction & 0x00F0);
                            break;

                        case 0x0005:    // SUB Vx, Vy
                            _registers.SubFromVxAndCarryToVf(instruction & 0x0F00, instruction & 0x00F0);
                            break;

                        case 0x0006:    // SHR Vx {, Vy}
                            _registers.ShrVx(instruction & 0x0F00, instruction & 0x00F0);
                            break;

                        case 0x0007:    // SUBN Vx, Vy
                            _registers.SubnVx(instruction & 0x0F00, instruction & 0x00F0);
                            break;

                        case 0x0008:    // SHL Vx {, Vy}
                            _registers.ShlVx(instruction & 0x0F00, instruction & 0x00F0);
                            break;
                    }
                    break;

                case 0x9000:    // SNE Vx, Vy
                    if (!_registers.AreEqual(instruction & 0x0F00, instruction & 0x00F0))
                        _counter.Increment();
                    break;

                case 0xA000:    // LD I, addr
                    _registers.SetIReg((short)(instruction & 0x0FFF));
                    break;

                case 0xB000:    // JP V0, addr
                    _counter.SetTo(_registers.GetVReg(0));
                    break;

                case 0xC000:    // RND Vx, byte
                    _registers.BitwiseAndWithInputToVx(instruction & 0x0F00, new Random().Next(255), instruction & 0x00FF);
                    break;

                case 0xD000:    // DRW Vx, Vy, nibble
                    var xCrd = _registers.GetVReg((instruction & 0x0F00) & 63);
                    var yCrd = _registers.GetVReg((instruction & 0x00F0) & 31);

                    _registers.SetVReg(15, 0);

                    for (var i = 0; i < ((int)instruction & 0x000F); i++)
                    {
                        var sprBitArr = new BitArray(_memory.Read(_registers.GetIReg(), 1));

                        for(var j = 0; j < sprBitArr.Length; j++)
                        {
                            if (sprBitArr[j] == true)
                                if (_screen.InvertPixelAndReturnShouldSetVF(xCrd+j, yCrd))
                                    _registers.SetVReg(15, 1);

                            xCrd++;
                        }

                        yCrd++;
                    }

                    break;

                case 0xE000:
                    switch (instruction & 0x00FF)
                    {
                        case 0x009E:    // SKP Vx
                            if (_input.IsPressed(instruction & 0x0F00))
                                _counter.Increment();
                            break;

                        case 0x00A1:    // SKNP Vx
                            if (!_input.IsPressed(instruction & 0x0F00))
                                _counter.Increment();
                            break;
                    }
                    break;

                case 0xF000:
                    switch (instruction & 0x00FF)
                    {
                        case 0x0007:    // LD Vx, DT
                            _registers.SetVReg(instruction & 0x0F00, _registers.GetDelayReg());
                            break;

                        case 0x000A:    // LD Vx, K
                            _gameState = 2;
                            break;

                        case 0x0015:    // LD DT, Vx
                            _registers.SetDelayReg((byte)(instruction & 0x0F00));
                            break;

                        case 0x0018:    // LD ST, Vx
                            _registers.SetSoundReg((byte)(instruction & 0x0F00));
                            break;

                        case 0x001E:    // ADD I, Vx
                            _registers.SetIReg((short)(_registers.GetVReg(instruction & 0x0F00) + _registers.GetIReg()));
                            break;

                        case 0x0029:    // LD F, Vx
                            //_registers.SetIReg()
                            break;
                    }
                    break;

            }
        }

        public bool WaitForInput()
        {
            return false;
            //var input = _input.WaitForInput
        }
    }
}
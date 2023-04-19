namespace Chippy8.Core
{
    public class Chip8
    {
        private IMemory _memory;
        private IScreen _screen;
        private IStack _stack;
        private ICounter _counter;
        private IRegisters _registers;

        private bool delayTimer; // delay timer. Active when delayReg is not 0
        private bool soundTimer; // sound timer. Active when soundReg is not 0

        public Chip8(IMemory memory, IScreen screen, IStack stack, ICounter counter, IRegisters registers)
        {
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _screen = screen ?? throw new ArgumentNullException(nameof(screen));
            _stack = stack ?? throw new ArgumentNullException(nameof(stack));
            _counter = counter ?? throw new ArgumentNullException(nameof(counter));
            _registers = registers ?? throw new ArgumentNullException(nameof(registers));

            delayTimer = false;
            soundTimer = false;
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
                    _stack.Push(_counter.Get());
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
                            _registers.SubFromVxAndCarryToVf(instruction, & 0x0F00, instruction & 0x00F0);
                            break;

                        case 0x0006:
                    }
                    break;


            }
        }
    }
}
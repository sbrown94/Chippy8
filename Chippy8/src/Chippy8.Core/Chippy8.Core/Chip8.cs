namespace Chippy8.Core
{
    public class Chip8
    {
        private IMemory _memory;
        private IScreen _screen;
        private IStack _stack;
        private ICounter _counter;

        private byte[] genReg;   // 16 general purpose 8 bit registers
        private byte[] iReg;     // 1 16 bit register
        private byte delayReg;   // 8 bit delay register
        private byte soundReg;   // 8 bit sound register
        private byte[] pCount;   // 16 bit program counter

        private bool delayTimer; // delay timer. Active when delayReg is not 0
        private bool soundTimer; // sound timer. Active when soundReg is not 0

        public Chip8(IMemory memory, IScreen screen, IStack stack, ICounter counter)
        {
            _memory = memory ?? throw new ArgumentNullException(nameof(memory));
            _screen = screen ?? throw new ArgumentNullException(nameof(screen));
            _stack = stack ?? throw new ArgumentNullException(nameof(stack));
            _counter = counter ?? throw new ArgumentNullException(nameof(counter));

            genReg = new byte[16];
            iReg = new byte[2];
            delayReg = new byte();
            soundReg = new byte();
            pCount = new byte[2];

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

            }
        }
    }
}
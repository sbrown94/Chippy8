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

        public void ExecuteInstruction(byte instruction)
        {
            switch (instruction)
            {
                case 0x00E0:    // CLS
                    _screen.ClearScreen();
                    break; 

                case 0x00EE:    // RET
                    _stack.Pop();
                    _counter.Increment();
                    break;
            }
        }
    }
}
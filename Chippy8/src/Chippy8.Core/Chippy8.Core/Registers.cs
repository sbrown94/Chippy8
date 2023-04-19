namespace Chippy8.Core
{
    public class Registers : IRegisters
    {
        private byte[] vReg;     // 16 general purpose 8 bit registers
        private byte[] iReg;     // 1 16 bit register
        private byte delayReg;   // 8 bit delay register
        private byte soundReg;   // 8 bit sound register

        public Registers()
        {
            vReg = new byte[16];
            iReg = new byte[2];
            delayReg = new byte();
            soundReg = new byte();
        }

        public void setVReg(int register, byte value)
        {
            vReg[register] = value;
        }

        public byte getVReg(int register) => vReg[register];
    }
}

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

        public void SetVReg(int register, int value)
        {
            vReg[register] = (byte)value;
        }

        public byte GetVReg(int register) => vReg[register];

        public void AddAtVReg(int register, int value) => vReg[register] = (byte)(vReg[register] + value);

        public void BitwiseOrToVx(int regX, int regY) => vReg[regX] = (byte)(regX | regY);

        public void BitwiseAndToVx(int regX, int regY) => vReg[regX] = (byte)(regX & regY);

        public void BitwiseXorToVx(int regX, int regY) => vReg[regX] = (byte)(regX ^ regY);
    }
}

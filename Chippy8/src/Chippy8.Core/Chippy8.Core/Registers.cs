namespace Chippy8.Core
{
    public class Registers : IRegisters
    {
        private byte[] vReg;     // 16 general purpose 8 bit registers
        private short iReg;     // 1 16 bit register
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

        public void BitwiseOrToVx(int regX, int regY) => vReg[regX] = (byte)(vReg[regX] | vReg[regY]);

        public void BitwiseAndToVx(int regX, int regY) => vReg[regX] = (byte)(vReg[regX] & vReg[regY]);

        public void BitwiseAndWithInputToVx(int regX, int rand, int input) => vReg[regX] = (byte)(rand & input);

        public void BitwiseXorToVx(int regX, int regY) => vReg[regX] = (byte)(vReg[regX] ^ vReg[regY]);

        public void AddToVxAndCarryToVf(int regX, int regY)
        {
            var result = vReg[regX] + vReg[regY];
            vReg[15] = (byte)((result > 255) ? 1 : 0);
            vReg[regX] = (byte)(result & 0xFF);
        }

        public void SubFromVxAndCarryToVf(int regX, int regY)
        {
            var result = vReg[regX] - vReg[regY];
            vReg[15] = (byte)((regX > regY) ? 1 : 0);
            vReg[regX] = (byte)result;
        }

        public void ShrVx(int regX, int regY)
        {
            vReg[15] = (byte)(((vReg[regX] & 0x000F) == 1) ? 1 : 0);
            vReg[regX] = (byte)(vReg[regX] / 2);
        }

        public void SubnVx(int regX, int regY)
        {
            vReg[15] = (byte)((vReg[regX] > vReg[regY]) ? 1 : 0);
            vReg[regX] = (byte)(vReg[regY] - vReg[regX]);
        }

        public void ShlVx(int regX, int regY)
        {
            vReg[15] = (byte)(((vReg[regX] & 0xF000) == 1) ? 1 : 0);
            vReg[regX] = (byte)(vReg[regX] * 2);
        }

        public bool AreEqual(int regX, int regY)
        {
            return regX == regY;
        }

        public void SetIReg(short value) => iReg = value;

        public short GetIReg() => iReg;
    }
}

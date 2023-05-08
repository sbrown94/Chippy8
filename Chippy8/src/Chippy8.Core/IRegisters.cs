namespace Chippy8.Core
{
    public interface IRegisters
    {
        public void SetVReg(int register, int value);
        public byte GetVReg(int register);
        public void AddAtVReg(int register, int value);
        public void BitwiseOrToVx(int regX, int regY);
        public void BitwiseAndToVx(int regX, int regY);
        public void BitwiseAndWithInputToVx(int regX, int rand, int input);
        public void BitwiseXorToVx(int regX, int regY);
        public void AddToVxAndCarryToVf(int regX, int regY);
        public void SubFromVxAndCarryToVf(int regX, int regY);
        public void ShrVx(int regX, int regY);
        public void SubnVx(int regX, int regY);
        public void ShlVx(int regX, int regY);
        public bool AreEqual(int regX, int regY);
        public void SetIReg(short value);
        public short GetIReg();
        public byte SetDelayReg(byte value);
        public byte GetDelayReg();
        public byte SetSoundReg(byte value);
        public byte GetSoundReg();
    }
}

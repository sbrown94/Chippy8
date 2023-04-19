namespace Chippy8.Core
{
    public interface IRegisters
    {
        public void SetVReg(int register, int value);
        public byte GetVReg(int register);
        public void AddAtVReg(int register, int value);
        public void BitwiseOrToVx(int regX, int regY);
        public void BitwiseAndToVx(int regX, int regY);
        public void BitwiseXorToVx(int regX, int regY);
        public void AddToVxAndCarryToVf(int regX, int regY);
        public void SubFromVxAndCarryToVf(int regX, int regY);

    }
}

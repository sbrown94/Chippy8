namespace Chippy8.Core
{
    public interface IRegisters
    {
        public void setVReg(int register, byte value);
        public byte getVReg(int register);
    }
}

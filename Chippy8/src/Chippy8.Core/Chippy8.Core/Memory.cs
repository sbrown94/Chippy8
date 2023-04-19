namespace Chippy8.Core
{
    public class Memory : IMemory
    {
        private byte[] Data;

        public Memory()
        {
            Data = new byte[0x1000]; // 4096 bytes
        }

        public bool Write(byte data, byte location)
        {
            Data[location] = data;
            return true;
        }
    }
}

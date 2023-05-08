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

        public byte[] Read(short start, byte length)
        {
            return (byte[])Data.Skip(start).Take(length);
        }
    }
}

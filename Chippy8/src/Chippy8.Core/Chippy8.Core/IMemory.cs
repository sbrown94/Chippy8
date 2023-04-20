namespace Chippy8.Core
{
    public interface IMemory
    {
        public bool Write(byte data, byte location);
        public byte[] Read(short start, byte length);
    }
}

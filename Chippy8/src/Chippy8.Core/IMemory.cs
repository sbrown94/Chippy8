namespace Chippy8.Core
{
    public interface IMemory
    {
        public bool Write(byte data, byte location);
        public short[] Read(short start, byte length);

        public void LoadProgram(string path);
    }
}

namespace Chippy8.Core
{
    public interface IMemory
    {
        public bool Write(byte data, byte location);
        public ushort[] Read(ushort start, ushort length);

        public void LoadProgram(string path);
    }
}

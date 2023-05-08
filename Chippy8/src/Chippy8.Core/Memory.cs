namespace Chippy8.Core
{
    public class Memory : IMemory
    {
        private short[] Data;

        public Memory()
        {
            Data = new short[0x500]; // 4096 bytes
        }

        public bool Write(byte data, byte location)
        {
            Data[location] = data;
            return true;
        }

        public short[] Read(short start, short length)
        {
            return Data.Skip(start).Take(length).ToArray();
        }

        public void LoadProgram(string path)
        {
            var bytes = File.ReadAllBytes(path);
            Array.Copy(bytes, 0, Data, 0x200, bytes.Length);
        }
    }
}

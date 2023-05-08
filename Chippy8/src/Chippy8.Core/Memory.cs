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

        public short[] Read(short start, byte length)
        {
            var data = Data.Skip(start).Take(length).ToArray();
            short[] sdata = new short[(int)Math.Ceiling((double)data.Length / 2)];
            Buffer.BlockCopy(data, 0, sdata, 0, data.Length);

            return sdata;
        }

        public void LoadProgram(string path)
        {
            var bytes = File.ReadAllBytes(path);
            Array.Copy(bytes, 0, Data, 0x200, bytes.Length);
        }
    }
}

namespace Chippy8.Core
{
    public class Memory : IMemory
    {
        private ushort[] Data;

        public Memory()
        {
            Data = new ushort[0x800]; // 4096 bytes
        }

        public bool Write(byte data, byte location)
        {
            Data[location] = data;
            return true;
        }

        public ushort[] Read(ushort start, ushort length)
        {
            return Data.Skip(start).Take(length).ToArray();
        }

        public void LoadProgram(string path)
        {
            var bytes = File.ReadAllBytes(path);


            ushort[] ushorts = new ushort[bytes.Length / 2];
            for (int i = 0; i < ushorts.Length; i++)
            {
                ushorts[i] = (ushort)((bytes[2 * i] << 8) | bytes[2 * i + 1]);
            }

            Array.Copy(ushorts, 0, Data, 0x100, ushorts.Length);
        }
    }
}

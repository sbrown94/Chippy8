namespace Chippy8.Core
{
    public class Counter : ICounter
    {
        private ushort pCounter;

        public Counter()
        {
            pCounter = 0x100;
        }

        public ushort Get() => pCounter;

        public void Increment() => pCounter+=1;

        public void Decrement() => pCounter-=1;

        public void SetTo(ushort value) => pCounter = value;
    }
}

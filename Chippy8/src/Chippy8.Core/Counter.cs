namespace Chippy8.Core
{
    public class Counter : ICounter
    {
        private short pCounter;

        public Counter()
        {
            pCounter = 0x200;
        }

        public short Get() => pCounter;

        public void Increment() => pCounter+=2;

        public void Decrement() => pCounter-=2;

        public void SetTo(short value) => pCounter = value;
    }
}

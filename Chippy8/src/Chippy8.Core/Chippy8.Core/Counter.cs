namespace Chippy8.Core
{
    public class Counter : ICounter
    {
        private int pCounter;

        public Counter()
        {
            pCounter = 0;
        }

        public int Get() => pCounter;

        public void Increment() => pCounter++;

        public void Decrement() => pCounter--;

        public void SetTo(int value) => pCounter = value;
    }
}

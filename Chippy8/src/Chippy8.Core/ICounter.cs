namespace Chippy8.Core
{
    public interface ICounter
    {
        public int Get();
        public void Increment();
        public void Decrement();
        public void SetTo(int value);
    }
}

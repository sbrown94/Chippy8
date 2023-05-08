namespace Chippy8.Core
{
    public interface ICounter
    {
        public short Get();
        public void Increment();
        public void Decrement();
        public void SetTo(short value);
    }
}

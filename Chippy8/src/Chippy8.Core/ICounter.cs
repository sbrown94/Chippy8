namespace Chippy8.Core
{
    public interface ICounter
    {
        public ushort Get();
        public void Increment();
        public void Decrement();
        public void SetTo(ushort value);
    }
}

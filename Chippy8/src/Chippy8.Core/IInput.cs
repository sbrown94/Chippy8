namespace Chippy8.Core
{
    public interface IInput
    {
        // run on tick to capture input
        public void Capture();

        public bool IsPressed(int key);
    }
}

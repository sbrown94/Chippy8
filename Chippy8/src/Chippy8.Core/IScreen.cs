namespace Chippy8.Core
{
    public interface IScreen
    {
        public void ClearScreen();

        public bool InvertPixelAndReturnShouldSetVF(int x, int y);
    }
}

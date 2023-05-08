namespace Chippy8.Core
{
    public class Input : IInput
    {
        public bool[] _keyboard;

        public Input()
        {
            _keyboard = new bool[16];
        }

        public void Capture()
        {
            // idk come back to this
            return;
            throw new NotImplementedException();
        }

        public bool IsPressed(int key) => _keyboard[key];
    }
}

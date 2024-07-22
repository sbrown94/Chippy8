using SFML.Window;

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
            //keyboard input capture
            

            return;
            throw new NotImplementedException();
        }

        public bool IsPressed(int key) => _keyboard[key];
    }
}

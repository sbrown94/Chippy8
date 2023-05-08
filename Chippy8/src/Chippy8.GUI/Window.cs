using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Chippy8.GUI
{
    public class Window : IWindow
    {
        public RenderWindow _window;
        public RenderTexture _texture;

        public int _gridWidth = 32;
        public int _gridHeight = 64;

        public void Init()
        {
            // Create a new window with a resolution of 800x600 pixels and a title of "SFML Example"
            _window = new RenderWindow(new VideoMode((uint)_gridWidth, (uint)_gridHeight), "SFML Example");
            _texture = new RenderTexture(64, 32);
        }

        public void Render(bool[,] display)
        {
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    if (display[i, j])
                    {
                        var pixel = new RectangleShape(new Vector2f(1, 1));
                        pixel.Position = new Vector2f(i, j);
                        pixel.FillColor = Color.White;
                        _texture.Draw(pixel);
                    }
                }
            }

            _texture.Display();
            var sprite = new Sprite(_texture.Texture);

            _window.DispatchEvents();

            // Draw the circle shape
            _window.Draw(sprite);

            // Display the window contents on screen
            _window.Display();
        }
    }
}

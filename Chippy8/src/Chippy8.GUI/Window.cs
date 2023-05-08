using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Chippy8.GUI
{
    public class Window : IWindow
    {
        public RenderWindow _window;

        public void Init()
        {
            // Create a new window with a resolution of 800x600 pixels and a title of "SFML Example"
            _window = new RenderWindow(new VideoMode(800, 600), "SFML Example");
        }

        public void Render(bool[,] display)
        {
            // Create a circle shape with a radius of 50 pixels
            var circle = new CircleShape(50)
            {
                FillColor = Color.Red,
                Position = new Vector2f(400, 300),
                Origin = new Vector2f(50, 50)
            };

            //// Create a texture from the pixel array and a sprite from the texture
            //var texture = new Texture((uint)GridWidth, (uint)GridHeight);
            //texture.Update(pixelData);
            //var sprite = new Sprite(texture)
            //{
            //    Scale = new Vector2f(PixelSize, PixelSize)
            //};

            // Handle events
            _window.DispatchEvents();

            // Draw the circle shape
            _window.Draw(circle);

            // Display the window contents on screen
            _window.Display();
        }
    }
}

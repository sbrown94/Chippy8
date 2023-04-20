using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chippy8.Core
{
    public class Screen : IScreen
    {
        public bool[,] Matrix { get; set; }

        public Screen()
        {
            Matrix = new bool[64, 32];
        }

        public void ClearScreen()
        {
            for (var col = 0; col < Matrix.GetLength(0); col++)
            {
                for (var row = 0; row < Matrix.GetLength(1), row++)
                {
                    Matrix[col, row] = false;
                }
            }
        }

        public bool DrawToScreen(int x, int y)
        {
            if (Matrix[x, y] == true)

        }
    }
}

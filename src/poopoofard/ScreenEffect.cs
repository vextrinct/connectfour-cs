using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poopoofard
{
    internal class ScreenEffect
    {
        public void SpiralIn()
        {
            for (int i = 0; i < 80; i++)
            {
                int offsetX = 0;
                int offsetY = 0;
                int posY = 0;
                bool inverse = false;
                Console.SetCursorPosition(0, 0);
                for (int j = 0; j < i; j++)
                {

                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.White;

                    if (inverse) { Console.SetCursorPosition(offsetX, posY); }
                    Console.Write(String.Concat(Enumerable.Repeat(" ", Console.WindowWidth - offsetX)));
                    //if (inverse) { Console.SetCursorPosition(offsetX, posY); }
                    offsetX += 1;

                    for (int k = 0; k < Console.WindowHeight - offsetY - 1; k++)
                    {
                        Console.SetCursorPosition(inverse ? offsetX / 2 : Console.WindowWidth - offsetX, posY);
                        Console.Write(" ");
                        posY += inverse ? -1 : 1;
                        if (posY >= Console.WindowHeight)
                        {
                            posY = Console.WindowHeight - 1;
                        }
                    }
                    offsetY += 1;
                    inverse = !inverse;
                }
                Thread.Sleep(1000);
            }
        }
    }
}

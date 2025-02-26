using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poopoofard
{
    internal class Utils
    {
        // Avoids flicker by setting the cursor to 0,0
        // Does not redraw, its not an artist.
        public static void Clear()
        {
            Console.SetCursorPosition(0, 0);
        }
        // Dash a writes
        public static void WriteDash(int length)
        {
            for (int i = 0; i < length + 4; i++)
            {
                Console.Write("-");
            }
        }
        // Title title.
        // Writes a title in the title
        public static void Title(string title)
        {
            Clear();
            //Console.Write("\x1b[3J");
            WriteDash(title.Length);
            Console.WriteLine("\n  " + title);
            WriteDash(title.Length);
            Console.WriteLine("");
        }
        // Actual clear. 
        // we have to send a message to the aliens if we want to fully clear, since c#
        public static void FullClear()
        {
            Console.Clear();
            Console.Write("\x1b[3J");
        }
        // Like clamp(), but it wraps the num around.
        // Kind of like %, but it also works backwards
        public static int Wrap(int num, int min, int max)
        {
            return num>max?min:num<min?max:num;
        }
    }
}

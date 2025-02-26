using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poopoofard
{
    internal class Utils
    {
        public static void Clear()
        {
            Console.SetCursorPosition(0, 0);
        }
        public static void WriteDash(int length)
        {
            for (int i = 0; i < length + 4; i++)
            {
                Console.Write("-");
            }
        }
        public static void Title(string title)
        {
            Clear();
            //Console.Write("\x1b[3J");
            WriteDash(title.Length);
            Console.WriteLine("\n  " + title);
            WriteDash(title.Length);
            Console.WriteLine("");
        }
        public static void FullClear()
        {
            Console.Clear();
            Console.Write("\x1b[3J");
        }
    }
}

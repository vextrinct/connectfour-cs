using System;
using System.Collections.Generic;
namespace poopoofard
{
    internal class Program
    {
        // 8 bits per cell
        // TOO MUCH!!!!!
        // literal bloat
        static byte[] A = {0,0,0,1,2,1};
        static byte[] B = {0,0,0,0,0,0};
        static byte[] C = {0,0,0,0,0,0};
        static byte[] D = {0,0,0,0,0,0};
        static byte[] E = {0,0,0,0,0,0};
        static byte[] F = {0,0,0,0,0,0};
        static byte[] G = {0,0,0,0,0,0};

        static List<byte[]> board = new List<byte[]>() {A,B,C,D,E,F,G};
        const byte RED = 1;
        const byte YELLOW = 2;

        static bool P1turn = true;
        static int cursor = 0;

        static void WriteDash(int length)
        {
            for (int i = 0; i < length + 4; i++)
            {
                Console.Write("-");
            }
        }
        public static void Title(string title)
        {
            Console.Clear();
            Console.Write("\x1b[3J");

            WriteDash(title.Length);
            Console.WriteLine("\n  " + title);
            WriteDash(title.Length);
            Console.WriteLine("");
        }

        public static void ShowGrid()
        {
            for(int i = 0; i < 6; i++)
            {
                for(int j = 0; j < board.Count; j++)
                {
                    if(board[j][i] == RED)
                    {
                        Console.Write("X");
                    } 
                    else if(board[j][i] == YELLOW)
                    {
                        Console.Write("O");
                    }
                    else
                    {
                        Console.Write("_");
                    }    
                    Console.Write("|");
                }
                Console.Write("\n");
            }
        }
        static void ShowHeader()
        {
            Title("Connect four");
            Console.WriteLine("\nproudly enshittified by: Maxim K\n");
            WriteDash(60);
            Console.WriteLine("\nA B C D E F G\n");
        }
        static string StrMul(string text, int count)
        {
            string result = "";
            for (int i = count; i > 0; i--)
            {
                result+=text;
            }
            return result;
        }
        static void Main(string[] args)
        {
            ConsoleKey input =  ConsoleKey.Help;
            while(input != ConsoleKey.Spacebar)
            {
                ShowHeader();
                switch (input)
                {
                    case ConsoleKey.LeftArrow:
                        cursor = Math.Clamp(cursor-1,0,6);
                        break;
                    case ConsoleKey.RightArrow:
                        cursor = Math.Clamp(cursor+1,0,6);
                        break;
                }
                Console.WriteLine(StrMul(" ",cursor*2)+(P1turn ? "X" : "O"));
                Console.WriteLine(StrMul(" ",cursor*2)+"V");
                ShowGrid();

                input = Console.ReadKey().Key;
            }
            P1turn = !P1turn;
        }
    }
}

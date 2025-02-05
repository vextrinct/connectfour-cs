using System;
using System.Collections.Generic;
namespace poopoofard
{
    internal class Program
    {
        // 8 bits per cell
        // TOO MUCH!!!!!
        // literal bloat
        // who needs objects when you have bits
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


        static void Clear()
        {
            Console.SetCursorPosition(0, 0);
        }
        static void WriteDash(int length)
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
        static void Refresh()
        {
            ShowHeader();
            Console.WriteLine(StrMul(" ", cursor * 2) + (P1turn ? "X" : "O") + "            ");
            Console.WriteLine(StrMul(" ", cursor * 2) + "V              ");
            ShowGrid();
        }
        static void Round()
        {
            ConsoleKey input = ConsoleKey.Help;
            while (input != ConsoleKey.Spacebar)
            {
                ShowHeader();
                switch (input)
                {
                    case ConsoleKey.LeftArrow:
                        cursor = Math.Clamp(cursor - 1, 0, 6);
                        break;
                    case ConsoleKey.RightArrow:
                        cursor = Math.Clamp(cursor + 1, 0, 6);
                        break;
                }
                Console.WriteLine(StrMul(" ", cursor * 2) + (P1turn ? "X" : "O") + "            ");
                Console.WriteLine(StrMul(" ", cursor * 2) + "V              ");
                ShowGrid();

                input = Console.ReadKey().Key;
            }
            for (int i = 0; i < 6; i++)
            {
                byte coin = board[cursor][i];
                if (coin == 0)
                {
                    board[cursor][i] = P1turn ? RED : YELLOW;
                    Refresh();
                    Thread.Sleep(50);
                    board[cursor][i] = 0;
                }
                if (i==5 || coin != 0)
                {
                    // When I was writing this code, only I and God knew how it works.
                    // Now only God knows it!!!!
                    // Please do not attempt to change the following line, it will just be a waste of time
                    // Increment the counter below as a warning for others ;)
                    //
                    // total_hours_wasted_here = 234;
                    //
                    board[cursor][i>0?coin==0?i:i-1:0]=i!=0?P1turn?RED:YELLOW:coin==(P1turn?RED:YELLOW)?P1turn?RED:YELLOW:P1turn?YELLOW:RED; // terniary operator my beloved
                    if(i==0)
                    {
                        P1turn = !P1turn;
                    }

                    break;
                }
            }
            P1turn = !P1turn;
        }

        static bool IsWinner()
        {
            int coin = P1turn ? 1 : 2;
            for (int i = 0; i < board.Count; i++)
            {
                int same = 0;
                for(int j = 0; j < 6; j++)
                {
                    if(board[i][j] == coin)
                    {
                        same++;
                        if(same>=4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        same = 0;
                    }
                }
            }

            return false;
        }

        static void Main(string[] args)
        {
            while (!IsWinner())
            {
                Round();
            }               
        }
    }
}

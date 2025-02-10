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
        static byte[] A = {0,0,0,0,0,0};
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

        static string COIN = "▇▇▇";
        static int WIDTH = 3;

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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(COIN);
                        Console.ForegroundColor = ConsoleColor.White;
                    } 
                    else if(board[j][i] == YELLOW)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(COIN);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write("___");
                    }
                    
                    Console.Write("|");
                }
                Console.Write("\n");
            }
            WriteDash(24);
        }
        static void ShowHeader()
        {
            Title("Connect four");
            Console.WriteLine("\nproudly enshittified by: Maxim K");
            Console.WriteLine("Use arrow keys(<- ->) to move the cursor. Press space to drop.\n");
            WriteDash(60);
            Console.WriteLine("\n A   B   C   D   E   F   G\n");
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
        static void DrawCursor()
        {
            if (P1turn)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            Console.WriteLine(StrMul(" ", cursor * (WIDTH+1)) +$"{COIN}         ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(StrMul(" ", cursor * (WIDTH+1)) + " V              ");
        }
        static void Refresh()
        {
            ShowHeader();
            DrawCursor();
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
                DrawCursor();
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
            // Vertical
            int coin = P1turn ? RED : YELLOW;
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
            // Horizontal
            for (int i = 0; i < 6; i++)
            {
                int same = 0;
                for (int j = 0; j < board.Count; j++)
                {
                    if (board[j][i] == coin)
                    {
                        same++;
                        if (same >= 4)
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
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool vertical = false;
            bool inverse = false;
            int offsetX = -2;
            int offsetY = 0;
            int posX = 0;
            int posY = 0;

            for (int i = 0; i < 80; i++) 
            {
                if (i % 2 == 0 && i != 0)
                {
                    inverse = !inverse;
                }

                for (int j = 0; j < (vertical ? Console.WindowHeight - offsetY : Console.WindowWidth - offsetX); j++) 
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.White;

                    
                    Console.SetCursorPosition(posX, posY);
                    
                    Console.Write(" ");
                    if (vertical)
                    {
                        Console.SetCursorPosition(posX - (inverse ? -1 : 1), posY);
                        Console.Write(" ");
                        Console.SetCursorPosition(posX - (inverse ? -2 : 2), posY);
                        Console.Write(" ");
                        posY += inverse ? -1 : 1; 
                        if(posY > Console.WindowHeight - 1) { posY = Console.WindowHeight-1; }
                    }
                    else 
                    { 
                        posX += inverse ? -1 : 1; 
                        if (posX>Console.WindowWidth-1) { posX = Console.WindowWidth-1; }
                    }    
                }

                if(vertical)
                {
                    offsetX+=3;
                }
                else
                {
                    offsetY++;
                }
                vertical = !vertical;
                
                Thread.Sleep(1);
            }
            offsetY = 30;
            for (int i = 0; i < 81; i++)
            {
                if (i % 2 == 0 && i != 0)
                {
                    inverse = !inverse;
                }

                for (int j = 0; j < (vertical ? Console.WindowHeight - offsetY : Console.WindowWidth - offsetX); j++)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;


                    Console.SetCursorPosition(posX, posY);

                    Console.Write(" ");
                    if (vertical)
                    {
                        Console.SetCursorPosition(posX - (inverse ? -1 : 1), posY);
                        Console.Write(" ");
                        Console.SetCursorPosition(posX - (inverse ? -2 : 2), posY);
                        Console.Write(" ");
                        posY += inverse ? -1 : 1;
                        if (posY > Console.WindowHeight - 1) { posY = Console.WindowHeight - 1; }
                        if (posY <= 0) { posY = 0; }
                    }
                    else
                    {
                        posX += inverse ? -1 : 1;
                        if (posX <=0) { posX = 0; }
                    }
                }

                if (vertical)
                {
                    offsetX -= 3;
                }
                else
                {
                    offsetY--;
                }
                vertical = !vertical;

                Thread.Sleep(1);
            }

            Console.Read();
            while (!IsWinner())
            {
                Round();
            }
            int x = Console.WindowWidth;
            int y = Console.WindowHeight;
            
            for (int i = 0; i < 4; i++)
            {

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.Write("\x1b[3J");
                Console.SetCursorPosition(x / 2 - 20, y / 2);
                Console.Write("!!!!!!!!!!!!!!YOU WON!!!!!!!!!!!!!!");
                Thread.Sleep(100);

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                Console.Write("\x1b[3J");
                Console.SetCursorPosition(x / 2 - 20, y / 2);
                Console.Write("!!!!!!!!!!!!!!YOU WON!!!!!!!!!!!!!!");
                Thread.Sleep(100);
            }

        }
    }
}

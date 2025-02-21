using System;
using System.Threading;
using System.Collections.Generic;
namespace poopoofard
{
    internal class Program
    {
        // this is the way
        
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
        const byte WINNER = 3;

        static bool P1turn = true;
        static int cursor = 0;

        static string COIN = "▇▇▇";
        static int WIDTH = 3;

        static bool playing = true;

        static void Init()
        {
            for (int i = 0; i < board.Count; i++)
            {
                Array.Clear(board[i],0,board.Count-1);
            }
            Console.Clear();
            Console.Write("\x1b[3J");
        }
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

        public static void ShowGrid(List<byte[]> board)
        {
            for(int i = 0; i < 6; i++)
            {
                for (int j = 0; j < board.Count; j++)
                {
                    if (board[j][i] == RED)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(COIN);
                        Console.ForegroundColor = ConsoleColor.White;
                    } 
                    else if (board[j][i] == YELLOW)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(COIN);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (board[j][i] == WINNER)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
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
        static void Refresh(List<byte[]> board)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            ShowHeader();
            DrawCursor();
            ShowGrid(board);
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
                ShowGrid(board);

                input = Console.ReadKey().Key;
            }
            for (int i = 0; i < 6; i++)
            {
                byte coin = board[cursor][i];
                if (coin == 0)
                {
                    board[cursor][i] = P1turn ? RED : YELLOW;
                    Refresh(board);
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
        // i should be executed for this terrible function
        static List<byte[]> IsWinner()
        {
            // Vertical
            int coin = P1turn ? YELLOW : RED;
            
            List<byte[]> tempBoard;
           
            for (int i = 0; i < board.Count; i++)
            {
                int same = 0;
                // good lord i did not know c# was this terrible.
                // DONT DO THIS FOR THE LOVE OF GOD
                tempBoard = new List<byte[]>(board.Count);

                foreach (var byteArray in board)
                {
                    tempBoard.Add((byte[])byteArray.Clone()); 
                }
                for (int j = 0; j < 6; j++)
                {
                    if(board[i][j] == coin)
                    {
                        same++;
                        tempBoard[i][j] = WINNER;
                        if(same>=4)
                        {
                            return tempBoard;
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
                tempBoard = new List<byte[]>(board.Count);

                foreach (var byteArray in board)
                {
                    tempBoard.Add((byte[])byteArray.Clone());
                }
                for (int j = 0; j < board.Count; j++)
                {
                    if (board[j][i] == coin)
                    {
                        same++;
                        tempBoard[j][i] = WINNER;
                        if (same >= 4)
                        {
                            return tempBoard;
                        }
                    }
                    else
                    {
                        same = 0;
                    }
                }
            }

            // Diagonal
            // Works, but barely. Currently cant check diagonals lower than the top 2 left corners.
            // Sometimes gets schizophrenic and thinks that 3 coins are 4.
            for (int i = 0; i < board.Count; i++)
            {
                int same = 0;
                // OVERFLOW idea: when i is less then 0 or above board.count, overflow is how much outside the limit i is.
                // Then we can use it to offset the height :>
                // terrible way but my code is alredy shit enough. One shit stain wont get rid of the others.
                //int overflow = Math.Abs();
                tempBoard = new List<byte[]>(board.Count);

                foreach (var byteArray in board)
                {
                    tempBoard.Add((byte[])byteArray.Clone());
                }
                for (int j = 0; j < Math.Clamp(board.Count-i,0,6); j++)
                {
                    if (board[j+i][j] == coin)
                    {
                        same++;
                        tempBoard[j+i][j] = WINNER;
                        if (same >= 4)
                        {
                            return tempBoard;
                        }
                    }
                    else
                    {
                        same = 0;
                    }

                }
                // this fella's schizophrenic.
                // Don't mind him, i'll call him bob.
                for (int j = 0; j < Math.Clamp(i,0,6); j++)
                {
                    if (board[i-j][j] == coin)
                    {
                        same++;
                        tempBoard[i-j][j] = WINNER;
                        if (same >= 4)
                        {
                            return tempBoard;
                        }
                    }
                    else
                    {
                        same = 0;
                    }

                }

            }

            return new List<byte[]>();
        }

        static void ShowWinner(List<byte[]> tempBoard)
        {
            bool flipflop = false;
            while(!playing)
            {
                Refresh(flipflop? board : tempBoard);
                Console.WriteLine("\n\nPress any key to play again, Q to quit...");
                Thread.Sleep(200);
                flipflop = !flipflop;
            }
        }
        static void Main(string[] args)
        {
            while (true)
            {
                Init();
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                playing = true;
                //ScreenEffect fx = new ScreenEffect();
                //fx.SpiralIn();
                List<byte[]> tempBoard;
                while ((tempBoard = IsWinner()).Count == 0)
                {
                    Round();
                }
                int x = Console.WindowWidth;
                int y = Console.WindowHeight;
                playing = false;
                for (int i = 0; i < 4; i++)
                {

                    Console.BackgroundColor = P1turn ? ConsoleColor.Yellow : ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Clear();
                    Console.Write("\x1b[3J");
                    Console.SetCursorPosition(x / 2 - 20, y / 2);
                    Console.Write($"!!!!!!!!!!!!!!{(P1turn ? "PLAYER 2" : "PLAYER 1")} WON!!!!!!!!!!!!!!");
                    Thread.Sleep(100);

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    Console.Write("\x1b[3J");
                    Console.SetCursorPosition(x / 2 - 20, y / 2);
                    Console.Write($"!!!!!!!!!!!!!!{(P1turn ? "PLAYER 2" : "PLAYER 1")} WON!!!!!!!!!!!!!!");
                    Thread.Sleep(100);
                }
                
                Thread.Sleep(1000);

                while (Console.KeyAvailable)
                    Console.ReadKey(false);
                Console.Clear();
                Console.Write("\x1b[3J");

                Thread t = new Thread(() => ShowWinner(tempBoard));
                t.Start();

                if(Console.ReadKey().Key == ConsoleKey.Q)
                {
                    t.Abort();
                    break;
                }
                
            }
        }
    }
}

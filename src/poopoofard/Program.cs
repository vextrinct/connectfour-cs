using System;
using System.Threading;
using System.Collections.Generic;
namespace poopoofard
{
    internal class Program
    {
        // this is the way
        static byte[,] board = new byte[7,6];

        static int sizeX = board.GetLength(0);
        static int sizeY = board.GetLength(1);
        static int numCells = board.Length;

        const byte RED = 1;
        const byte YELLOW = 2;
        const byte WINNER = 3;

        static bool P1turn = true;
        static int cursor = 0;

        static string COIN = "▇▇▇";
        static int WIDTH = 3;

        static bool playing = true;
        static int counter = 0;

        static void Init()
        {
            counter = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                Array.Clear(board,0,board.Length);
            }
            Utils.FullClear(); 
        }
        public static void ShowGrid(byte[,] board)
        {
            for(int i = 0; i < sizeY; i++)
            {
                for (int j = 0; j < sizeX; j++)
                {
                    if (board[j,i] == RED)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(COIN);
                        Console.ForegroundColor = ConsoleColor.White;
                    } 
                    else if (board[j,i] == YELLOW)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(COIN);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (board[j,i] == WINNER)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(COIN);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("___");
                    }
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("|");
                }
                Console.Write("\n");
            }
            Utils.WriteDash(24);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void ShowHeader()
        {
            Utils.Title("Connect four");
            Console.WriteLine("\nproudly enshittified by: Maxim K");
            Console.WriteLine("Use arrow keys(<- ->) to move the cursor. Press space to drop.\n");
            Utils.WriteDash(60);
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
        static void Refresh(byte[,] board)
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
            for (int i = 0; i < sizeY; i++)
            {
                byte coin = board[cursor,i];
                if (coin == 0)
                {
                    board[cursor,i] = P1turn ? RED : YELLOW;
                    Refresh(board);
                    Thread.Sleep(50);
                    board[cursor,i] = 0;
                }
                if (i==sizeY-1 || coin != 0)
                {
                    // When I was writing this code, only I and God knew how it works.
                    // Now only God knows it!!!!
                    // Please do not attempt to change the following line, it will just be a waste of time
                    // Increment the counter below as a warning for others ;)
                    //
                    // total_hours_wasted_here = 234;
                    //
                    board[cursor,i>0?coin==0?i:i-1:0]=i!=0?P1turn?RED:YELLOW:coin==(P1turn?RED:YELLOW)?P1turn?RED:YELLOW:P1turn?YELLOW:RED; // terniary operator my beloved
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
        static byte[,] IsWinner()
        {
            // Vertical
            int coin = P1turn ? YELLOW : RED;
            byte[,] tempBoard;
           
            for (int i = 0; i < sizeX; i++)
            {
                int same = 0;
                tempBoard = board.Clone() as byte[,];
                
                for (int j = 0; j < sizeY; j++)
                {
                    if(board[i,j] == coin)
                    {
                        same++;
                        tempBoard[i,j] = WINNER;
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
            for (int i = 0; i < sizeY; i++)
            {
                int same = 0;
                tempBoard = board.Clone() as byte[,];

                for (int j = 0; j < sizeX; j++)
                {
                    if (board[j,i] == coin)
                    {
                        same++;
                        tempBoard[j,i] = WINNER;
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
            for (int i = -2; i < sizeX + 2; i++)
            {
                int same = 0;
                // OVERFLOW idea: when i is less then 0 or above board.count, overflow is how much outside the limit i is.
                // Then we can use it to offset the height :>
                // terrible way but my code is alredy shit enough. One shit stain wont get rid of the others.
                int overflow = i < 0 ? Math.Abs(i) : i > sizeX - 1 ? i+1 - sizeX : 0;
                int column = Math.Clamp(i, 0, sizeX - 1);
                tempBoard = board.Clone() as byte[,];

                for (int j = 0; j < Math.Clamp(sizeX - column, 0, sizeY-overflow); j++)
                {
                    if (board[j+column,j+overflow] == coin)
                    {
                        same++;
                        tempBoard[j+ column,j + overflow] = WINNER;
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
                // Don't mind him, he's just also a tiny bit autistic, please be patient
                same = 0;
                for (int j = 0; j < column-overflow; j++)
                {
                    if (board[column - j,j + overflow] == coin)
                    {
                        same++;
                        tempBoard[column - j,j + overflow] = WINNER;
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

            return new byte[0,0];
        }

        static void ShowWinner(byte[,] tempBoard)
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
                byte[,] tempBoard;
                while ((tempBoard = IsWinner()).GetLength(0) == 0)
                {
                    Round();
                    counter++;
                    if(counter >= numCells)
                    {
                        Console.WriteLine("\n\nGrid is full! Press any key to try again, Q to quit...");
                        if (Console.ReadKey().Key == ConsoleKey.Q)
                        {
                            System.Environment.Exit(0);
                        }
                        Init();
                    }
                }
                int x = Console.WindowWidth;
                int y = Console.WindowHeight;
                playing = false;
                for (int i = 0; i < 4; i++)
                {

                    Console.BackgroundColor = P1turn ? ConsoleColor.Yellow : ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Utils.FullClear();
                    Console.SetCursorPosition(x / 2 - 20, y / 2);
                    Console.Write($"!!!!!!!!!!!!!!{(P1turn ? "PLAYER 2" : "PLAYER 1")} WON!!!!!!!!!!!!!!");
                    Thread.Sleep(100);

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Utils.FullClear();
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

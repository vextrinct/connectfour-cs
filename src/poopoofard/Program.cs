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
        // Resets the board.
        static void Init()
        {
            counter = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                Array.Clear(board,0,board.Length);
            }
            Utils.FullClear(); 
        }
        // Shows the board
        public static void ShowGrid(byte[,] board)
        {
            // For each row...
            for(int i = 0; i < sizeY; i++)
            {
                // In each column...
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
                    // Blank cell
                    else
                    {
                        Console.Write("___");
                    }
                    Console.Write("|");
                }
                Console.Write("\n");
            }
            Utils.WriteDash(24);
            Console.ForegroundColor = ConsoleColor.White;
        }
        // Title
        static void ShowHeader()
        {
            Utils.Title("Connect four");
            Console.WriteLine("\nproudly enshittified by: Maxim K");
            Console.WriteLine("Use arrow keys(<- ->) to move the cursor. Press space to drop.\n");
            Utils.WriteDash(60);
            Console.WriteLine("\n A   B   C   D   E   F   G\n");
        }
        // Multiples a string
        // To: Microsoft
        // Was is that hard to implement string*string??? This language is already high level ya doofuses!
        static string StrMul(string text, int count)
        {
            string result = "";
            for (int i = count; i > 0; i--)
            {
                result+=text;
            }
            return result;
        }
        // Draws the cursor. Can you even read smh my head?
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
            Console.WriteLine(StrMul(" ", cursor * (WIDTH+1)) +$"{COIN}                           ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(StrMul(" ", cursor * (WIDTH+1)) + " V                               ");
        }
        // Redraws the entire screen
        // Very efficient.
        static void Refresh(byte[,] board)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            ShowHeader();
            DrawCursor();
            ShowGrid(board);
        }
        // Game round
        // Waits for input, drops the coin, switches turns.
        static void Round()
        {
            // We move the cursor
            ConsoleKey input = ConsoleKey.Help;
            while (input != ConsoleKey.Spacebar)
            {
                ShowHeader();
                switch (input)
                {
                    case ConsoleKey.LeftArrow:
                        cursor = Utils.Wrap(cursor-1,0,sizeX-1);
                        break;
                    case ConsoleKey.RightArrow:
                        cursor = (cursor+1) % sizeX;
                        break;
                }
                DrawCursor();
                ShowGrid(board);

                input = Console.ReadKey().Key;
            }
            // Dropping animation
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
                    // Equivalent to:
                    // We get the correct color,
                    // If i!=0 (column is not full)
                    //    Set to current player's coin color
                    // Else if coin == active color
                    //    keep it
                    // Else
                    //    flip it
                    
                    // If the column is full, we keep the turn.
                    // Result of spaghetti code, yum
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
            // for each column,
            for (int i = 0; i < sizeX; i++)
            {
                int same = 0;
                tempBoard = board.Clone() as byte[,];
                // in each row,
                for (int j = 0; j < sizeY; j++)
                {
                    // If we get the color we need
                    if(board[i,j] == coin)
                    {
                        same++;
                        tempBoard[i,j] = WINNER;
                        if(same>=4)
                        {
                            return tempBoard;
                        }
                    }
                    // Different color, we abort
                    else
                    {
                        same = 0;
                    }
                }
            }
            // Horizontal
            // for each row,
            for (int i = 0; i < sizeY; i++)
            {
                int same = 0;
                tempBoard = board.Clone() as byte[,];
                // in each column
                for (int j = 0; j < sizeX; j++)
                {
                    // If we get the color we need
                    if (board[j,i] == coin)
                    {
                        same++;
                        tempBoard[j,i] = WINNER;
                        if (same >= 4)
                        {
                            return tempBoard;
                        }
                    }
                    // Different color, we abort
                    else
                    {
                        same = 0;
                    }
                }
            }

            // Diagonal
            // Works, but barely. Currently cant check diagonals lower than the top 2 left corners. sizeX MUST be below or equal to 7
            // Sometimes gets schizophrenic and thinks that 3 coins are 4.
            // For each column,
            for (int i = -2; i < sizeX + 2; i++)
            {
                int same = 0;
                // OVERFLOW idea: when i is less then 0 or above board.count, overflow is how much outside the limit i is.
                // Then we can use it to offset the height :>
                // terrible way but my code is alredy shit enough. Cleaning one shit stain wont get rid of the others
                int overflow = i < 0 ? Math.Abs(i) : i > sizeX - 1 ? i+1 - sizeX : 0;
                int column = Math.Clamp(i, 0, sizeX - 1);
                tempBoard = board.Clone() as byte[,];
                // In each row,
                for (int j = 0; j < Math.Clamp(sizeX - column, 0, sizeY-overflow); j++)
                {
                    // We move down right
                    if (board[j+column,j+overflow] == coin)
                    {
                        same++;
                        tempBoard[j+ column,j + overflow] = WINNER;
                        if (same >= 4)
                        {
                            return tempBoard;
                        }
                    }
                    // Different color, we abort
                    else
                    {
                        same = 0;
                    }

                }
                // this fella's schizophrenic.
                // Don't mind him, he's just also a tiny bit autistic, please be patient
                same = 0;
                // In each row,
                for (int j = 0; j < column-overflow; j++)
                {
                    // We move down left.
                    if (board[column - j,j + overflow] == coin)
                    {
                        same++;
                        tempBoard[column - j,j + overflow] = WINNER;
                        if (same >= 4)
                        {
                            return tempBoard;
                        }
                    }
                    // Different color, we abort
                    else
                    {
                        same = 0;
                    }

                }

            }

            return new byte[0,0];
        }
        // Shows the winner. 
        // "Thank you, Captain obvious!"
        // MUST RUN IN A SEPARATE THREAD!!!
        // or ur stuck haha
        static void ShowWinner(byte[,] tempBoard)
        {
            bool flipflop = false;
            // Flip flops the flippy flappy boards between floppy flippy colors.
            while(!playing)
            {
                Refresh(flipflop? board : tempBoard);
                Console.WriteLine("\n\nPress any key to play again, Q to quit...");
                Thread.Sleep(200);
                flipflop = !flipflop;
            }
        }
        // Main. It's the main function. Do I even need to explain this?
        static void Main(string[] args)
        {
            // Trap the player into eternal doom.
            while (true)
            {
                Init();
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                playing = true;
                byte[,] tempBoard;
                // Main game loop: we loop the loop until we cant loop it anymore because the loop said that the player won, while in the loop
                while ((tempBoard = IsWinner()).GetLength(0) == 0)
                {
                    Round();
                    // Hacky solution to the full board problem
                    // Works, so idgaf
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
                // flashbang the player
                // increase the speed if u wanna breakdance on the floor involuntarily
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

                // where syntax go?
                // (we clear the input buffer)
                while (Console.KeyAvailable)
                    Console.ReadKey(false);

                Utils.FullClear();

                // We flash the winner.
                // Not in that way!
                Thread t = new Thread(() => ShowWinner(tempBoard));
                t.Start();

                if(Console.ReadKey().Key == ConsoleKey.Q)
                {
                    // WONTFIX:
                    // Not an issue in Linux.
                    // Windows issue, skill issue + ratio + not open source + no stability + no money + bozo
                    t.Abort();
                    break;
                }
                
            }
        }
    }
}

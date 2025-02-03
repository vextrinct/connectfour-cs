namespace poopoofard
{
    internal class Program
    {

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
            Console.WriteLine("\x1b[3J");

            WriteDash(title.Length);
            Console.WriteLine("\n  " + title);
            WriteDash(title.Length);
            Console.WriteLine("");
        }

        static void Main(string[] args)
        {
            Title("Connect four");
            Console.WriteLine("\nproudly enshittified by: Maxim K\n");
            WriteDash(30);
        }
    }
}

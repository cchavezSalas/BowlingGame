using Logic;
using System;

namespace BowlingApp
{
    internal class Program
    {
        static void Main(string[] args)
        {


            if (args.Length == 1)
            {
                string filepath = args[0];
                Console.WriteLine(filepath);
                new Application(filepath).Start();
                //test machine
            }
            else {
                Console.WriteLine("Please provide a filepath to process");
            }
            Console.WriteLine("Please press Enter to continue");
            Console.ReadLine();
        }
    }
}

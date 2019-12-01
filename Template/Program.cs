using System;
using System.IO;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            foreach(var line in lines){
                var intval = Int.Parse(line);
            }
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

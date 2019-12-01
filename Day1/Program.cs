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
            double total = 0;
            double runningTotal;
            foreach (var line in lines)
            {
                runningTotal = Double.Parse(line);
                do
                {
                    var temp = Math.Floor(runningTotal / 3) - 2;
                    if (temp > 0)
                        total += temp;
                    runningTotal = temp;
                } while (runningTotal > 0);
            }
            Console.WriteLine(total);
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

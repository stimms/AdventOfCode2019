using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            foreach(var line in lines){
                var intval = Int32.Parse(line);
            }
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

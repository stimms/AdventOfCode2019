using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Advent
{
    class Reaction
    {
        IList<(int qty, string chemical)> inputs { get; set; }
        (int qty, string chemical) output { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            foreach (var line in lines)
            {
                var tempInputs = line.Split("=>").First().Split(",");
                var inputs = new List<(int, string)>();
                foreach (var temp in tempInputs)
                {
                    var qty = Int32.Parse(temp.Trim().Split(" ").First());
                    var chemical = temp.Trim().Split(" ").Last();
                    inputs.Add((qty, chemical));
                }
                var tempChemical = line.Split("=>").Last().Trim().Split(" ");
                inputs.Add((Int32.Parse(tempChemical.First()), tempChemical.Last()));
            }
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

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
            var input = lines[0];
            var phases = 100;
            var inputSet = input.ToCharArray().Select(x => Int32.Parse(x.ToString())).ToList();
            var workingSet = new List<int>();
            for (int i = 0; i < 10_000; i++)
                workingSet.AddRange(inputSet);
            var output = new List<int>();
            for (int i = 0; i < phases; i++)
            {
                Console.WriteLine(i);
                int digitCounter = 1;
                output = new List<int>();
                foreach (var c in workingSet)
                {
                    var multipliers = new List<int>();
                    while (multipliers.Count < workingSet.Count + 2)
                    {
                        for (int j = 0; j < digitCounter; j++)
                            multipliers.Add(0);
                        for (int j = 0; j < digitCounter; j++)
                            multipliers.Add(1);
                        for (int j = 0; j < digitCounter; j++)
                            multipliers.Add(0);
                        for (int j = 0; j < digitCounter; j++)
                            multipliers.Add(-1);
                    }
                    var total = 0;
                    var multiplier = digitCounter;
                    for (int nonZero = digitCounter - 1; nonZero < workingSet.Count; nonZero += 0)
                    {

                        total += multipliers[multiplier] * workingSet[nonZero];
                        multiplier++;
                        if (multipliers[multiplier] == 0)
                        {
                            nonZero += digitCounter + 1;
                            multiplier+=digitCounter;
                        }
                        else
                            nonZero++;
                    }
                    output.Add(Math.Abs(total) % 10);
                    digitCounter++;
                }
                workingSet = output;
            }
            var offset = Int32.Parse(String.Join("", output.Take(7)));
            Console.WriteLine(offset);
            Console.WriteLine(String.Join("", output.Skip(offset).Take(8)));
            //Console.WriteLine(String.Join("", output));

            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

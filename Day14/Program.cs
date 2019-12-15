using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Advent
{
    class Reaction
    {
        public IList<(long qty, string chemical)> inputs { get; set; }
        public (long qty, string chemical) output { get; set; }
    }

    class Program
    {
        static List<Reaction> reactions;
        static Dictionary<string, long> surpluss = new Dictionary<string, long>();
        static long generate(string chemical, long qty)
        {
            var result = 0L;
            var reaction = reactions.Where(x => x.output.chemical == chemical).Single();
            var multiplier = (long)Math.Ceiling((double)qty / (double)reaction.output.qty);
            foreach (var input in reaction.inputs.Select(x => (qty: x.qty, x.chemical)))
            {
                if (!surpluss.ContainsKey(chemical))
                    surpluss[chemical] = 0;
                if (!surpluss.ContainsKey(input.chemical))
                    surpluss[input.chemical] = 0;

                if (input.chemical == "ORE")
                    result += input.qty * multiplier;
                else
                {
                    if (surpluss[input.chemical] < multiplier * input.qty)
                    {
                        result += generate(input.chemical, input.qty * multiplier- surpluss[input.chemical]);
                    }
                    surpluss[input.chemical] -= multiplier * input.qty;
                }
            }
            surpluss[chemical] += multiplier * reaction.output.qty;
            return result;
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            foreach (var file in new string[] {
                //"input.test1.txt",
                // "input.test2.txt",
                // "input.test3.txt",
                // "input.test4.txt",
                // "input.test5.txt",
                "input.txt" })
            {
                var lines = File.ReadAllLines(file);
                surpluss = new Dictionary<string, long>();
                reactions = new List<Reaction>();
                foreach (var line in lines)
                {
                    var tempInputs = line.Split("=>").First().Split(",");
                    var inputs = new List<(long, string)>();
                    foreach (var temp in tempInputs)
                    {
                        var qty = long.Parse(temp.Trim().Split(" ").First());
                        var chemical = temp.Trim().Split(" ").Last();
                        inputs.Add((qty, chemical));
                    }
                    var tempChemical = line.Split("=>").Last().Trim().Split(" ");
                    reactions.Add(new Reaction { inputs = inputs, output = (long.Parse(tempChemical.First()), tempChemical.Last()) });
                }
                Console.WriteLine(generate("FUEL", 1));
                var upper = 1_000_000_000L;
                var lower = 0L;
                while(upper>lower)
                {
                    var midpoint = lower + (upper-lower)/2;
                    var generated = generate("FUEL", midpoint);
                    Console.WriteLine(midpoint + " " + generated);
                    if(generated>1_000_000_000_000L)
                        upper = midpoint;
                    else
                        lower = midpoint;
                }
                Console.WriteLine(upper);
                Console.WriteLine(lower);


            }
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

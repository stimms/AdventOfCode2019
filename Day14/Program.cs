using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Advent
{
    class Reaction
    {
        public IList<(int qty, string chemical)> inputs { get; set; }
        public (int qty, string chemical) output { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            foreach (var file in new string[] {
                "input.test1.txt",
                "input.test2.txt",
                "input.test3.txt",
                "input.test4.txt",
                "input.test5.txt",
                "input.txt" })
            {
                var lines = File.ReadAllLines(file);
                var reactions = new List<Reaction>();
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
                    reactions.Add(new Reaction { inputs = inputs, output = (Int32.Parse(tempChemical.First()), tempChemical.Last()) });
                }
                var needed = reactions.Where(x => x.output.chemical == "FUEL").Single().inputs;
                var surpluss = new Dictionary<string, int>();

                while (needed.Any(x => x.chemical != "ORE"))
                {
                    var newNeeded = needed.Where(x => x.chemical == "ORE").ToList();

                    foreach (var need in needed.Where(x => x.chemical != "ORE"))
                    {
                        var reaction = reactions.Where(x => x.output.chemical == need.chemical).Single();
                        var workingNeed = (need.qty, need.chemical);
                        if (surpluss.ContainsKey(workingNeed.chemical))
                        {
                            if (workingNeed.qty > surpluss[workingNeed.chemical])
                            {
                                workingNeed.qty -= surpluss[workingNeed.chemical];
                                surpluss[workingNeed.chemical] = 0;
                            }
                            else
                            {
                                surpluss[workingNeed.chemical] = surpluss[workingNeed.chemical] - workingNeed.qty;
                                workingNeed.qty = 0;
                            }
                        }

                        var multiplier = (int)Math.Ceiling((double)workingNeed.qty / reaction.output.qty);
                        newNeeded.AddRange(reaction.inputs.Select(x => (x.qty * multiplier, x.chemical)));
                        surpluss[reaction.output.chemical] = (reaction.output.qty * multiplier) - workingNeed.qty;
                    }
                    //compact
                    needed = newNeeded.GroupBy(x => x.chemical).Select(x => (x.Sum(y => y.qty), x.Key)).ToList();
                }
                Console.WriteLine(needed.Sum(x => x.qty));
            }
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

using System;
using System.IO;
using System.Linq;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");

            var memory = lines[0].Split(",").Select(x => Int32.Parse(x)).ToArray();
            Console.WriteLine(memory[0]);

            for (int noun = 0; noun < 100; noun++)
                for (int verb = 0; verb < 100; verb++)
                {
                    var counter = 0;
                    memory = lines[0].Split(",").Select(x => Int32.Parse(x)).ToArray();
                    memory[1] = noun;
                    memory[2] = verb;
                    while (counter < memory.Count())
                    {
                        if (memory[counter] == 1)
                        {
                            memory[memory[counter + 3]] = memory[memory[counter + 1]] + memory[memory[counter + 2]];
                        }
                        else if (memory[counter] == 2)
                            memory[memory[counter + 3]] = memory[memory[counter + 1]] * memory[memory[counter + 2]];
                        else if (memory[counter] == 99)
                            counter = 50000;
                        counter += 4;
                    }
                    if (memory[0] == 19690720)
                    {
                        Console.WriteLine(noun);
                        Console.WriteLine(verb);
                        System.Environment.Exit(0);
                    }
                }
            Console.WriteLine(String.Join(",", memory));
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

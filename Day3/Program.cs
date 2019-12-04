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
            var populated = new Dictionary<(int, int), int[]>();
            int index = 0;
            foreach (var line in lines)
            {
                int currentXLocation = 0;
                int currentYLocation = 0;
                int distanceTraveled = 1;
                foreach (var instruction in line.Split(','))
                {
                    var moves = Int32.Parse(instruction.Substring(1));
                    if (instruction.StartsWith("R"))
                    {
                        for (int i = currentXLocation + 1; i <= currentXLocation + moves; i++)
                        {
                            if (!populated.ContainsKey((i, currentYLocation)))
                            {
                                populated[(i, currentYLocation)] = new int[] {0, 0};
                            }

                            populated[(i, currentYLocation)][index] = distanceTraveled++;

                        }
                        currentXLocation = currentXLocation + moves;
                    }
                    if (instruction.StartsWith("L"))
                    {
                        for (int i = currentXLocation - 1; i >= currentXLocation - moves; i--)
                        {
                            if (!populated.ContainsKey((i, currentYLocation)))
                            {
                                populated[(i, currentYLocation)] = new int[] {0, 0};
                            }
                            populated[(i, currentYLocation)][index] = distanceTraveled++;

                        }
                        currentXLocation = currentXLocation - moves;
                    }

                    if (instruction.StartsWith("U"))
                    {
                        for (int i = currentYLocation + 1; i <= currentYLocation + moves; i++)
                        {
                            if (!populated.ContainsKey((currentXLocation, i)))
                            {
                                populated[(currentXLocation, i)] = new int[] {0, 0};
                            }
                            populated[(currentXLocation, i)][index] = distanceTraveled++;

                        }
                        currentYLocation = currentYLocation + moves;
                    }
                    if (instruction.StartsWith("D"))
                    {
                        for (int i = currentYLocation - 1; i >= currentYLocation - moves; i--)
                        {
                            if (!populated.ContainsKey((currentXLocation, i)))
                            {
                                populated[(currentXLocation, i)] = new int[] {0, 0};
                            }
                            populated[(currentXLocation, i)][index] = distanceTraveled++;
                        }
                        currentYLocation = currentYLocation - moves;
                    }
                }


                index++;
            }
            //Console.WriteLine(String.Join(",", populated.Where(x => x.Value[0] > 0 && x.Value[1] > 0).Select(x => x.Key.Item1 + "," + x.Key.Item2 + "\n")));
            Console.WriteLine(populated.Where(x => x.Value[0] > 0 && x.Value[1] > 0).OrderBy(x => Math.Abs(x.Key.Item1) + Math.Abs(x.Key.Item2)).First());

            //Console.WriteLine(populated.Where(x => x.Value[0] > 0 && x.Value[1] > 0).OrderBy(x => x.Value[0]+x.Value[1]).First());
            Console.WriteLine(String.Join(",",populated.Where(x => x.Value[0] > 0 && x.Value[1] > 0).OrderBy(x => x.Value[0]+x.Value[1]).First().Value));
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

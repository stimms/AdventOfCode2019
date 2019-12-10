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
            var asteroids = new bool[lines[0].Length, lines.Length];

            int lineCounter = 0;
            foreach (var line in lines)
            {
                int counter = 0;
                foreach (var c in line.ToCharArray())
                {
                    asteroids[lineCounter, counter] = c == '#';
                    counter++;
                }
                lineCounter++;
            }

            var visibleCount = new int[lines[0].Length, lines.Length];
            for (int y = 0; y < lines[0].Length; y++)
                for (int x = 0; x < lines[0].Length; x++)
                {
                    if (asteroids[y, x])
                    {//is an asteroid here

                        //rays to top row
                        var workingAsteroids = asteroids.Clone() as bool[,];
                        for (int x1 = 0; x1 < lines[0].Length; x1++)
                        {
                            //x run
                            var run = x1 - x;
                            //y rise
                            var rise = y;

                            //reduce
                            if (rise != 0 && run % rise == 0)
                                run = run / rise;
                            if (rise == 0)
                                run = 1;
                            if (run != 0 && rise % run == 0)
                                rise = rise / run;
                            if (run == 0)
                                rise = 1;
                            bool appears = false;
                            int steps = 1;
                            while (x + (run * steps) >= 0 && x + (run * steps) < lines.Length && y + (rise * steps) >= 0 && y + (rise * steps) < lines.Length)
                            {
                                if (workingAsteroids[y + (rise * steps), x + (run * steps)])
                                {
                                    appears = true;
                                    workingAsteroids[y + (rise * steps), x + (run * steps)] = false;
                                }
                                steps++;
                            }

                            if (appears)
                            {
                                visibleCount[y, x]++;
                            }
                            appears = false;
                            steps = -1;
                            while (x + (run * steps) >= 0 && x + (run * steps) < lines.Length && y + (rise * steps) >= 0 && y + (rise * steps) < lines.Length)
                            {
                                if (workingAsteroids[y + (rise * steps), x + (run * steps)])
                                {
                                    appears = true;
                                    workingAsteroids[y + (rise * steps), x + (run * steps)] = false;
                                }
                                steps--;
                            }
                            if (appears)
                            {
                                visibleCount[y, x]++;
                            }
                        }


                        //rays to left col
                        //rays to righ col
                        //rays to bottom row
                        for (int x1 = 0; x1 < lines[0].Length; x1++)
                        {
                            //x run
                            var run = x1 - x;
                            //y rise
                            var rise = lines.Length - y;

                            //reduce
                            if (rise != 0 && run % rise == 0)
                                run = run / rise;
                            if (rise == 0)
                                run = 1;
                            if (run != 0 && rise % run == 0)
                                rise = rise / run;
                            if (run == 0)
                                rise = 1;
                            bool appears = false;
                            int steps = 1;
                            while (x + (run * steps) >= 0 && x + (run * steps) < lines.Length && y + (rise * steps) >= 0 && y + (rise * steps) < lines.Length)
                            {
                                if (workingAsteroids[y + (rise * steps), x + (run * steps)])
                                {
                                    appears = true;
                                    workingAsteroids[y + (rise * steps), x + (run * steps)] = false;
                                }
                                steps++;
                            }

                            if (appears)
                            {
                                visibleCount[y, x]++;
                            }
                            appears = false;
                            steps = -1;
                            while (x + (run * steps) >= 0 && x + (run * steps) < lines.Length && y + (rise * steps) >= 0 && y + (rise * steps) < lines.Length)
                            {
                                if (workingAsteroids[y + (rise * steps), x + (run * steps)])
                                {
                                    appears = true;
                                    workingAsteroids[y + (rise * steps), x + (run * steps)] = false;
                                }
                                steps--;
                            }
                            if (appears)
                            {
                                visibleCount[y, x]++;
                            }
                        }
                    }
                }
            for (int x = 0; x < lines[0].Length; x++)
            {
                for (int y = 0; y < lines[0].Length; y++)
                    Console.Write(visibleCount[x, y]);
                Console.WriteLine();
            }
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

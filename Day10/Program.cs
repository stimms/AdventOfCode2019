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

            var girdSize = lines.Length;
            var visibleCount = new int[lines[0].Length, lines.Length];
            for (int y = 0; y < lines[0].Length; y++)
                for (int x = 0; x < lines[0].Length; x++)
                {
                    if (asteroids[y, x])
                    {//is an asteroid here

                        //rays to top row
                        var workingAsteroids = asteroids.Clone() as bool[,];
                        //printBoolArray(workingAsteroids);
                        for (int i = 0; y + i < girdSize; i++)
                        {
                            for (int j = 0; x + j < girdSize; j++)
                            {
                                int step = 1;
                                bool found = false;
                                while ((step * i) + y >= 0 && (step * i) + y < girdSize && (step * j) + x >= 0 && (step * j) + x < girdSize && !(i == 0 && j == 0))
                                {//along the ray
                                    if (workingAsteroids[y + (i * step), x + (j * step)])
                                    {
                                        found = true;
                                        workingAsteroids[y + (i * step), x + (j * step)] = false;
                                    }
                                    step++;
                                }
                                if (found)
                                    visibleCount[y, x]++;
                            }

                        }
                        for (int i = 0; y + i >= 0; i--)
                        {
                            for (int j = 0; x + j < girdSize; j++)
                            {
                                int step = 1;
                                bool found = false;
                                while ((step * i) + y >= 0 && (step * i) + y < girdSize && (step * j) + x >= 0 && (step * j) + x < girdSize && !(i == 0 && j == 0))
                                {//along the ray
                                    if (workingAsteroids[y + (i * step), x + (j * step)])
                                    {
                                        found = true;
                                        workingAsteroids[y + (i * step), x + (j * step)] = false;
                                    }
                                    step++;
                                }
                                if (found)
                                    visibleCount[y, x]++;
                            }

                        }
                        for (int i = 0; y + i < girdSize; i++)
                        {
                            for (int j = 0; x + j >= 0; j--)
                            {
                                int step = 1;
                                bool found = false;
                                while ((step * i) + y >= 0 && (step * i) + y < girdSize && (step * j) + x >= 0 && (step * j) + x < girdSize && !(i == 0 && j == 0))
                                {//along the ray
                                    if (workingAsteroids[y + (i * step), x + (j * step)])
                                    {
                                        found = true;
                                        workingAsteroids[y + (i * step), x + (j * step)] = false;
                                    }
                                    step++;
                                }
                                if (found)
                                    visibleCount[y, x]++;
                            }

                        }
                        for (int i = 0; y + i >= 0; i--)
                        {
                            for (int j = 0; x + j >= 0; j--)
                            {
                                int step = 1;
                                bool found = false;
                                while ((step * i) + y >= 0 && (step * i) + y < girdSize && (step * j) + x >= 0 && (step * j) + x < girdSize && !(i == 0 && j == 0))
                                {//along the ray
                                    if (workingAsteroids[y + (i * step), x + (j * step)])
                                    {
                                        found = true;
                                        workingAsteroids[y + (i * step), x + (j * step)] = false;
                                    }
                                    step++;
                                }
                                if (found)
                                    visibleCount[y, x]++;
                            }

                        }
                        //printBoolArray(workingAsteroids);

                    }
                }
            printArray(visibleCount);
            int max = 0;
            int maxX = 0;
            int maxY = 0;
            for (int x = 0; x < Math.Sqrt(visibleCount.Length); x++)
            {
                for (int y = 0; y < Math.Sqrt(visibleCount.Length); y++)
                {
                    if (max < visibleCount[y, x])
                    {
                        max = visibleCount[y, x];
                        maxX = x;
                        maxY = y;
                    }
                }
            }
            Console.WriteLine(max);


            //part 2
            int zappedCount = 0;
            double radIncrement = Math.Atan(1 / (double)girdSize);


            var currentRad = Math.PI / 2;
            while (currentRad >= 0)
            {
                double radius = 1;
                while (radius < girdSize)
                {
                    var opp = Math.Sin(currentRad) * radius;
                    if (Double.IsNaN(opp))
                        opp = 0;
                    var adj = Math.Cos(currentRad) * radius;
                    if (Double.IsNaN(adj))
                        adj = radius;
                    if (Math.Abs(opp % 1) < (0.00000000001) && Math.Abs(adj % 1) < (0.0000000001))
                    {
                        //whole number to check
                        if (adj + maxY < girdSize && opp + maxX < girdSize)
                        {
                            if (asteroids[(int)adj + maxY, (int)opp + maxX])
                            {
                                asteroids[(int)adj + maxY, (int)opp + maxX] = false;
                                zappedCount++;
                                if (zappedCount == 200)
                                {
                                    Console.WriteLine(opp + "," + adj);
                                }
                            }
                        }
                    }
                    radius+=(1/(double)girdSize);
                }
                currentRad -= radIncrement;
            }




            Console.WriteLine("done.");
            Console.ReadLine();
        }

        private static void printArray(int[,] toPrint)
        {
            for (int x = 0; x < Math.Sqrt(toPrint.Length); x++)
            {
                for (int y = 0; y < Math.Sqrt(toPrint.Length); y++)
                    Console.Write(toPrint[x, y] > 0 ? toPrint[x, y].ToString() : ".");
                Console.WriteLine();
            }
        }

        private static void printBoolArray(bool[,] toPrint)
        {
            for (int x = 0; x < Math.Sqrt(toPrint.Length); x++)
            {
                for (int y = 0; y < Math.Sqrt(toPrint.Length); y++)
                    Console.Write(toPrint[x, y] ? "#" : ".");
                Console.WriteLine();
            }
        }

    }
}

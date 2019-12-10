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
            var otherPoints = new List<(int x, int y)>();
            for (int x = 0; x < Math.Sqrt(visibleCount.Length); x++)
            {
                for (int y = 0; y < Math.Sqrt(visibleCount.Length); y++)
                {
                    if (!(x == maxX && y == maxY) && asteroids[y, x])
                    {
                        otherPoints.Add((y, x));
                    }
                }
            }
            var ordered = otherPoints.OrderBy(p =>
            {
                var xSide = p.x - maxX;
                var ySide = p.y - maxY;
                if (xSide > 0 && ySide < 0)
                    return Math.Atan(Math.Abs(xSide) / Math.Abs(ySide));
                if (xSide > 0 && ySide > 0)
                    return Math.PI / 2 + Math.Atan(Math.Abs(ySide) / Math.Abs(xSide));
                if (xSide < 0 && ySide > 0)
                    return Math.PI + Math.Atan(Math.Abs(xSide) / Math.Abs(ySide));
                if (xSide < 0 && ySide < 0)
                    return (1.5 * Math.PI) + Math.Atan(Math.Abs(ySide) / Math.Abs(xSide));
                if (xSide == 0 && ySide < 0)
                    return 0;
                if (xSide > 0 && ySide == 0)
                    return Math.PI / 2;
                if (xSide == 0 && ySide > 0)
                    return Math.PI;
                return 1.5 * Math.PI;
            }).ThenByDescending(p => Math.Sqrt(Math.Pow(maxX - p.x, 2) + Math.Pow(maxY - p.y, 2)));

            var zapOrder = new int[lines[0].Length, lines.Length];
            int t = 0;
            foreach (var item in ordered)
            {
                zapOrder[item.y, item.x] = t;
                t++;
            }
            //printArray(zapOrder);
            if (zapOrder.Length > 200)
            {
                for (int x = 0; x < Math.Sqrt(zapOrder.Length); x++)
                {
                    for (int y = 0; y < Math.Sqrt(zapOrder.Length); y++)
                    if(zapOrder[y,x]==199)
                    Console.WriteLine($"{x},{y}");
                 
                }
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

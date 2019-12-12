using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Advent
{
    class Program
    {

        static int RELATIVE = 2;
        static int IMMEDIATE = 1;
        static int POSITION = 0;


        static long BLACK = 0;
        static long WHITE = 1;
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            var tempMemory = lines[0].Split(",").Select(x => long.Parse(x)).ToList();
            tempMemory.AddRange(new long[10000]);
            memory = tempMemory.ToArray();
            var painted = new Dictionary<(int, int), long>();
            var currentDirection = 0;
            var robotLocation = (0, 0);
            painted.Add((0,0), WHITE);
            bool halted;
            int counter = 0;
            do
            {
                long colorToPaint;
                long direction;
                (colorToPaint, halted) = compute(painted.ContainsKey(robotLocation) ? painted[robotLocation] : BLACK);
                (direction, halted) = compute(painted.ContainsKey(robotLocation) ? painted[robotLocation] : BLACK);
                painted[robotLocation] = colorToPaint;
                if (direction == 1)
                {
                    currentDirection = (currentDirection + 1) % 4;
                }
                if (direction == 0)
                {
                    currentDirection = (currentDirection - 1 + 4) % 4;
                }
                switch (currentDirection)
                {
                    case 0:
                        robotLocation = (robotLocation.Item1, robotLocation.Item2 - 1);
                        break;
                    case 1:
                        robotLocation = (robotLocation.Item1 + 1, robotLocation.Item2);
                        break;
                    case 2:
                        robotLocation = (robotLocation.Item1, robotLocation.Item2 + 1);
                        break;
                    case 3:
                        robotLocation = (robotLocation.Item1 - 1, robotLocation.Item2);
                        break;
                }

            } while (!halted);
            Console.WriteLine(painted.Count);

            var minX = painted.Min(x => x.Key.Item1);
            var minY = painted.Min(x => x.Key.Item2);
            var maxX = painted.Max(x => x.Key.Item1);
            var maxY = painted.Max(x => x.Key.Item2);
            for (int i = 0; i <= Math.Abs(minY) + Math.Abs(maxY); i++)
            {
                for (int j = 0; j <= Math.Abs(minX) + Math.Abs(maxX); j++)
                {
                    Console.Write(painted.ContainsKey((j-Math.Abs(minY),i-Math.Abs(minX))) ? painted[(j-Math.Abs(minY),i-Math.Abs(minX))] == BLACK ? " " : "#" : " ");
                }
                Console.WriteLine();
            }
                    Console.WriteLine("done.");
            Console.ReadLine();
        }


        static long[] memory;
        static int counter = 0;
        static int relatvieBase = 0;
        private static (long output, bool halted) compute(long input = 0)
        {

            

            while (counter < memory.Count())
            {
                (int opCode, int mode1, int mode2, int mode3) = GetOpCode(memory[counter].ToString());
                if (memory[counter] == 99)
                    return (0, true);
                long parameter1 = mode1 == POSITION ? memory[counter + 1] : mode1 == RELATIVE ? memory[counter + 1] + relatvieBase : counter + 1;
                long parameter2 = 0;
                long parameter3 = 0;
                if (opCode == 1 || opCode == 2 || opCode == 7 || opCode == 8)
                {
                    parameter2 = mode2 == POSITION ? memory[counter + 2] : mode2 == RELATIVE ? memory[counter + 2] + relatvieBase : counter + 2;
                    parameter3 = mode3 == POSITION ? memory[counter + 3] : mode3 == RELATIVE ? memory[counter + 3] + relatvieBase : counter + 3;
                }
                if (opCode == 5 || opCode == 6)
                {
                    parameter2 = mode2 == POSITION ? memory[counter + 2] : mode2 == RELATIVE ? memory[counter + 2] + relatvieBase : counter + 2;
                }
                if (opCode == 1)
                {
                    memory[parameter3] = memory[parameter1] + memory[parameter2];
                    counter += 4;
                }
                else if (opCode == 2)
                {
                    memory[parameter3] = memory[parameter1] * memory[parameter2];
                    counter += 4;
                }
                else if (opCode == 3)
                {
                    memory[parameter1] = input;//Int32.Parse(Console.ReadLine());
                    counter += 2;
                }
                else if (opCode == 4)
                {
                    counter += 2;
                    return (memory[parameter1], false);
                }

                else if (opCode == 5 && memory[parameter1] != 0)
                {
                    counter = (int)memory[parameter2];
                }
                else if (opCode == 5)
                {
                    counter += 3;
                }

                else if (opCode == 6 && memory[parameter1] == 0)
                {
                    counter = (int)memory[parameter2];
                }
                else if (opCode == 6)
                {
                    counter += 3;
                }

                else if (opCode == 7)
                {
                    memory[parameter3] = memory[parameter1] < memory[parameter2] ? 1 : 0;
                    counter += 4;
                }
                else if (opCode == 8)
                {
                    memory[parameter3] = memory[parameter1] == memory[parameter2] ? 1 : 0;
                    counter += 4;
                }
                else if (opCode == 9)
                {
                    relatvieBase += (int)memory[parameter1];
                    counter += 2;
                }

            }
            return (0, true);
        }

        private static (int opCode, int mode1, int mode2, int mode3) GetOpCode(string entry)
        {
            int opCode = 0, mode1 = 0, mode2 = 0, mode3 = 0;
            if (entry.Length == 1)
                opCode = Int32.Parse(entry);
            if (entry.Length > 1)
                opCode = Int32.Parse(entry.Substring(entry.Length - 2));
            if (entry.Length > 2)
                mode1 = Int32.Parse(entry.Substring(entry.Length - 3, 1));
            if (entry.Length > 3)
                mode2 = Int32.Parse(entry.Substring(entry.Length - 4, 1));
            if (entry.Length > 4)
                mode3 = Int32.Parse(entry.Substring(entry.Length - 5, 1));
            return (opCode, mode1, mode2, mode3);
        }
    }
}

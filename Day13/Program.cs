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
            var tempMemory = lines[0].Split(",").Select(x => long.Parse(x)).ToList();
            tempMemory.AddRange(new long[10000]);
            memory = tempMemory.ToArray();
            memory[0] = 2;
            var halted = false;
            var tiles = new Dictionary<(long, long), long>();
            var score = 0L;
            var prime = new List<long>{};
            if (File.Exists("output.txt"))
            {
                var outputLines = File.ReadAllLines("output.txt");
                prime.AddRange(String.Join(",", outputLines).Split(",").Where(x => x.Length > 0).Select(x => long.Parse(x)).ToList());
            }
            Queue<long> inputs = new Queue<long>();
            foreach (var primer in prime)
                inputs.Enqueue(primer);

            var inputKeys = new List<long>();
            (long x, long y) ballLocation = (0L, 0L);
            (long x, long y) previousBallLocation = (0L, 0L);
            (long x, long y) paddleLocation = (0L, 0L);
            while (!halted)
            {
                var x = compute(inputs);
                if (x.halted)
                    break;
                var y = compute(inputs);
                if (y.halted)
                    break;
                var type = compute(inputs);
                if (x.output == -1L && y.output == 0L)
                    score = type.output;
                else
                    tiles[(x.output, y.output)] = type.output;
                if (type.output == 3)
                    paddleLocation = (x.output, y.output);
                if (type.output == 4)
                {
                    previousBallLocation = ballLocation;
                    ballLocation = (x.output, y.output);
                }
                if (type.halted)
                    break;
                if (!inputs.Any())
                {
                    Draw(tiles, ballLocation, paddleLocation, score);
                    var expectedX = 0L;
                    if (ballLocation.x == paddleLocation.x)
                    {
                        if (ballLocation.x == 41)
                            inputs.Enqueue(-1);
                        if (ballLocation.x == 1)
                            inputs.Enqueue(1);
                        else if (ballLocation.x > previousBallLocation.x)
                            inputs.Enqueue(1);
                        else if (ballLocation.x < previousBallLocation.x)
                            inputs.Enqueue(-1);
                        else
                            inputs.Enqueue(0);
                    }
                    else if (ballLocation.y > previousBallLocation.y)
                    {
                        var slope = (previousBallLocation.y - ballLocation.y) / (previousBallLocation.x - ballLocation.x);
                        expectedX = ballLocation.x + (slope * (21 - ballLocation.y));

                        if (expectedX < paddleLocation.x)
                            inputs.Enqueue(-1);
                        else if (expectedX > paddleLocation.x)
                            inputs.Enqueue(1);
                        else
                            inputs.Enqueue(0);
                    }
                    else if (ballLocation.y < previousBallLocation.y)//moving away
                    {
                        var slope = (previousBallLocation.y - ballLocation.y) / (previousBallLocation.x - ballLocation.x);
                        expectedX = ballLocation.x + (-1 * slope * (21 - ballLocation.y));

                        if (expectedX < paddleLocation.x)
                            inputs.Enqueue(-1);
                        else if (expectedX > paddleLocation.x)
                            inputs.Enqueue(1);
                        else
                            inputs.Enqueue(0);
                    }
                    else
                        inputs.Enqueue(0);

                    Console.WriteLine(inputs.Peek());
                    inputKeys.Add(inputs.Peek());
                    // var read = long.Parse(Console.ReadLine());
                    // inputKeys.Add(read);
                    // inputs.Enqueue(read);
                }


            }
            // Console.WriteLine(String.Join(",", inputKeys));
            // if (tiles.Any(t => t.Value == 2))
            // {
            //     var extraDirections = ballLocation.x - paddleLocation.x;
            //     for (int i = inputKeys.Count - 1; i > 0 && extraDirections != 0; i--)
            //     {
            //         if (inputKeys[i] > -1 && extraDirections < 0)
            //         {
            //             inputKeys[i]--;
            //             extraDirections += 1;
            //         }
            //         if (inputKeys[i] < 1 && extraDirections > 0)
            //         {
            //             inputKeys[i]++;
            //             extraDirections -= 1;
            //         }
            //     }

            // }
            //Console.WriteLine(tiles.Count(x => x.Item3 == 2));
            File.AppendAllText("output.txt", String.Join(",", inputKeys) + ",");
            Console.WriteLine(String.Join(",", inputKeys));
            Console.WriteLine("Score " + score);
            Console.WriteLine("done.");
            Console.ReadLine();
        }

        private static void Draw(Dictionary<(long, long), long> tiles, (long, long) ballLocation, (long, long) paddleLocation, long score)
        {
            try
            {
                // Console.Clear();
            }
            catch (Exception) { }
            Console.WriteLine(score);
            foreach (var tile in tiles.OrderBy(x => x.Key.Item2).ThenBy(x => x.Key.Item1))
            {
                if (tile.Key.Item1 == 0)
                    Console.WriteLine();
                if (tile.Key.Item1 == ballLocation.Item1 && tile.Key.Item2 == ballLocation.Item2)
                    Console.Write("b");
                else if (tile.Key.Item1 == paddleLocation.Item1 && tile.Key.Item2 == paddleLocation.Item2)
                    Console.Write("^");
                else
                {
                    if (tile.Value == 0)
                        Console.Write(" ");
                    if (tile.Value == 1)
                        Console.Write("|");
                    if (tile.Value == 2)
                        Console.Write("*");

                }
            }
        }

        static long[] memory;
        static int counter = 0;
        static int relatvieBase = 0;
        static int RELATIVE = 2;
        static int IMMEDIATE = 1;
        static int POSITION = 0;
        private static (long output, bool halted) compute(Queue<long> inputs)
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
                    memory[parameter1] = inputs.Dequeue();//Int32.Parse(Console.ReadLine());
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

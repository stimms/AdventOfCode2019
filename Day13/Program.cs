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

            var vm = new IntercodeVM();
            vm.memory = tempMemory.ToArray();
            vm.memory[0] = 2;
            var halted = false;
            var tiles = new Dictionary<(long, long), long>();
            var score = 0L;
            // var prime = new List<long>{};
            // if (File.Exists("output.txt"))
            // {
            //     var outputLines = File.ReadAllLines("output.txt");
            //     prime.AddRange(String.Join(",", outputLines).Split(",").Where(x => x.Length > 0).Select(x => long.Parse(x)).ToList());
            // }
            Queue<long> inputs = new Queue<long>();
            // foreach (var primer in prime)
            //     inputs.Enqueue(primer);

            var inputKeys = new List<long>();
            (long x, long y) ballLocation = (0L, 0L);
            (long x, long y) paddleLocation = (0L, 0L);
            while (!halted)
            {

                (halted, ballLocation, paddleLocation, score) = RunCompute(vm, inputs, tiles);
                var desired = FindIntersect(vm, tiles);
                if (desired > paddleLocation.x)
                    inputs.Enqueue(1);
                else if (desired < paddleLocation.x)
                    inputs.Enqueue(-1);
                else
                    inputs.Enqueue(0);
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

        private static (bool halted, (long x, long y) ballLocation, (long x, long y) paddleLocation, long score) RunCompute(IntercodeVM vm, Queue<long> inputs, Dictionary<(long, long), long> tiles)
        {
            (long x, long y) ballLocation = (0L, 0L);
            long score = 0;
            (long x, long y) paddleLocation = (0L, 0L);
            var halted = false;

            var x = vm.compute(inputs);
            var y = vm.compute(inputs);
            var type = vm.compute(inputs);
            if (x.output == -1L && y.output == 0L)
                score = type.output;
            else
                tiles[(x.output, y.output)] = type.output;
            if (type.output == 3)
                paddleLocation = (x.output, y.output);
            if (type.output == 4)
            {

                ballLocation = (x.output, y.output);
            }
            if (type.halted)
                halted = type.halted;
            if (!inputs.Any())
            {
                Draw(tiles, ballLocation, paddleLocation, score);

            }
            return (halted, ballLocation, paddleLocation, score);
        }
        private static long FindIntersect(IntercodeVM vm, Dictionary<(long, long), long> tiles)
        {
            var moveCountFinder = vm.Clone();
            var tilesClone = new Dictionary<(long, long), long>();
            foreach (var key in tiles.Keys)
                tilesClone.Add(key, tiles[key]);
            var ballLocation = (0L, 0L);

            while (ballLocation.Item2 != 21)
            {
                var input = new Queue<long>();
                input.Enqueue(0);
                (_, ballLocation, _, _) = RunCompute(moveCountFinder, input, tilesClone);
            }
            return ballLocation.Item1;
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


    }
}

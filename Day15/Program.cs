using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Advent
{
    class Node
    {
        List<Node> Children = new List<Node>();
    }
    class Program
    {
        static long HIT_WALL = 0;
        static long MOVED_ONE = 1;
        static long MOVED_AND_AT_02 = 3;
        static int NORTH = 1;
        static int SOUTH = 2;
        static int WEST = 3;
        static int EAST = 4;
        static Dictionary<(int x, int y), long> visited = new Dictionary<(int, int), long>();
        private static void Explore(IntercodeVM vm, (int x, int y) location)
        {
            var newLocation = (location.x, location.y + 1);
            var north = vm.Clone();
            var northQueue = new Queue<long>();
            northQueue.Enqueue(NORTH);
            var northResult = north.compute(northQueue);
            if (northResult.output == MOVED_ONE || northResult.output == MOVED_AND_AT_02)
            {
                if (!visited.ContainsKey(newLocation))
                {
                    visited.Add(newLocation, MOVED_ONE);
                    Explore(north, newLocation);
                }
            }
            else
                visited[newLocation] = northResult.output;

            newLocation = (location.x, location.y - 1);
            var south = vm.Clone();
            var southQueue = new Queue<long>();
            southQueue.Enqueue(SOUTH);
            var southResult = south.compute(southQueue);
            if (southResult.output == MOVED_ONE || southResult.output == MOVED_AND_AT_02)
            {
                if (!visited.ContainsKey(newLocation))
                {
                    visited.Add(newLocation, MOVED_ONE);
                    Explore(south, newLocation);
                }
            }
            else
                visited[newLocation] = southResult.output;

            newLocation = (location.x - 1, location.y);
            var east = vm.Clone();
            var eastQueue = new Queue<long>();
            eastQueue.Enqueue(EAST);
            var eastResult = east.compute(eastQueue);
            if (eastResult.output == MOVED_ONE || eastResult.output == MOVED_AND_AT_02)
            {
                if (!visited.ContainsKey(newLocation))
                {
                    visited.Add(newLocation, MOVED_ONE);
                    Explore(east, newLocation);
                }
            }
            else
                visited[newLocation] = eastResult.output;

            newLocation = (location.x + 1, location.y);
            var west = vm.Clone();
            var westQueue = new Queue<long>();
            westQueue.Enqueue(WEST);
            var westResult = west.compute(westQueue);
            if (westResult.output == MOVED_ONE || westResult.output == MOVED_AND_AT_02)
            {
                if (!visited.ContainsKey(newLocation))
                {
                    visited.Add(newLocation, MOVED_ONE);
                    Explore(west, newLocation);
                }
            }
            else
                visited[newLocation] = westResult.output;

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            var tempMemory = lines[0].Split(",").Select(x => long.Parse(x)).ToList();
            tempMemory.AddRange(new long[10000]);

            var vm = new IntercodeVM();
            vm.memory = tempMemory.ToArray();
            Explore(vm, (0, 0));

            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

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
        static long MOVED_AND_AT_02 = 2;
        static int NORTH = 1;
        static int SOUTH = 2;
        static int WEST = 3;
        static int EAST = 4;
        static Dictionary<(int x, int y), (long type, int steps)> visited = new Dictionary<(int, int), (long, int)>();
        private static void Explore(IntercodeVM vm, (int x, int y) location, int steps)
        {
            var newLocation = (location.x, location.y + 1);
            var north = vm.Clone();
            var northQueue = new Queue<long>();
            northQueue.Enqueue(NORTH);
            var northResult = north.compute(northQueue);
            if (northResult.output == MOVED_ONE || northResult.output == MOVED_AND_AT_02)
            {
                if (!visited.ContainsKey(newLocation) || visited[newLocation].steps > steps)
                {
                    visited[newLocation] = (northResult.output, steps);
                    Explore(north, newLocation, steps + 1);
                }
            }
            else if (!visited.ContainsKey(newLocation) || visited[newLocation].steps > steps)
            {
                visited[newLocation] = (northResult.output, steps);
            }

            newLocation = (location.x, location.y - 1);
            var south = vm.Clone();
            var southQueue = new Queue<long>();
            southQueue.Enqueue(SOUTH);
            var southResult = south.compute(southQueue);
            if (southResult.output == MOVED_ONE || southResult.output == MOVED_AND_AT_02)
            {
                if (!visited.ContainsKey(newLocation) || visited[newLocation].steps > steps)
                {
                    visited[newLocation] = (southResult.output, steps);
                    Explore(south, newLocation, steps + 1);
                }
            }
            else if (!visited.ContainsKey(newLocation) || visited[newLocation].steps > steps)
            {
                visited[newLocation] = (southResult.output, steps);
            }

            newLocation = (location.x - 1, location.y);
            var east = vm.Clone();
            var eastQueue = new Queue<long>();
            eastQueue.Enqueue(EAST);
            var eastResult = east.compute(eastQueue);
            if (eastResult.output == MOVED_ONE || eastResult.output == MOVED_AND_AT_02)
            {
                if (!visited.ContainsKey(newLocation) || visited[newLocation].steps > steps)
                {
                    visited[newLocation] = (eastResult.output, steps);
                    Explore(east, newLocation, steps + 1);
                }
            }
            else if (!visited.ContainsKey(newLocation) || visited[newLocation].steps > steps)
            {
                visited[newLocation] = (eastResult.output, steps);
            }

            newLocation = (location.x + 1, location.y);
            var west = vm.Clone();
            var westQueue = new Queue<long>();
            westQueue.Enqueue(WEST);
            var westResult = west.compute(westQueue);
            if (westResult.output == MOVED_ONE || westResult.output == MOVED_AND_AT_02)
            {
                if (!visited.ContainsKey(newLocation) || visited[newLocation].steps > steps)
                {
                    visited[newLocation] = (westResult.output, steps);
                    Explore(west, newLocation, steps + 1);
                }
            }
            else if (!visited.ContainsKey(newLocation) || visited[newLocation].steps > steps)
            {
                visited[newLocation] = (westResult.output, steps);
            }

        }
        static Dictionary<(int, int), int> filled = new Dictionary<(int, int), int>();
        static void Fill((int x, int y) location, int steps)
        {
            if (filled.ContainsKey(location) && filled[location] > steps)
                filled[location] = steps;
            
            if (!filled.ContainsKey(location))
            {
                filled[location] = steps;
                if (visited.ContainsKey((location.x + 1, location.y)) && visited[(location.x + 1, location.y)].type == MOVED_ONE)
                    Fill((location.x + 1, location.y), steps + 1);
                if (visited.ContainsKey((location.x - 1, location.y)) && visited[(location.x - 1, location.y)].type == MOVED_ONE)
                    Fill((location.x - 1, location.y), steps + 1);
                if (visited.ContainsKey((location.x, location.y + 1)) && visited[(location.x, location.y + 1)].type == MOVED_ONE)
                    Fill((location.x, location.y + 1), steps + 1);
                if (visited.ContainsKey((location.x, location.y - 1)) && visited[(location.x, location.y - 1)].type == MOVED_ONE)
                    Fill((location.x, location.y - 1), steps + 1);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            var tempMemory = lines[0].Split(",").Select(x => long.Parse(x)).ToList();
            tempMemory.AddRange(new long[10000]);

            var vm = new IntercodeVM();
            vm.memory = tempMemory.ToArray();
            Explore(vm, (0, 0), 0);
            Console.WriteLine(visited.Single(x => x.Value.type == MOVED_AND_AT_02).Value.steps);

            Fill(visited.Single(x => x.Value.type == MOVED_AND_AT_02).Key, 0);
            Console.WriteLine(filled.OrderByDescending(x => x.Value).First().Value);
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

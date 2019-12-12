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
            var moons = new List<(int xpos, int ypos, int zpos, int xvel, int yvel, int zvel)>();
            foreach (var line in lines)
            {
                var segments = line.Split(new String[] { "=", ",", ">" }, StringSplitOptions.None);
                moons.Add((Int32.Parse(segments[1]), Int32.Parse(segments[3]), Int32.Parse(segments[5]), 0, 0, 0));
            }
            var initial = (moons.ToArray().Clone() as (int xpos, int ypos, int zpos, int xvel, int yvel, int zvel)[]).ToList();
            for (int i = 0; loopCounter.Count < 12; i++)
            {
                var temp = new List<(int xpos, int ypos, int zpos, int xvel, int yvel, int zvel)>();
                foreach (var moon in moons)
                {
                    temp.Add((
                        moon.xpos + moons.Where(x => x != moon).Sum(x => moon.xpos < x.xpos ? 1 : moon.xpos > x.xpos ? -1 : 0) + moon.xvel,
                        moon.ypos + moons.Where(x => x != moon).Sum(x => moon.ypos < x.ypos ? 1 : moon.ypos > x.ypos ? -1 : 0) + moon.yvel,
                        moon.zpos + moons.Where(x => x != moon).Sum(x => moon.zpos < x.zpos ? 1 : moon.zpos > x.zpos ? -1 : 0) + moon.zvel,
                        moons.Where(x => x != moon).Sum(x => moon.xpos < x.xpos ? 1 : moon.xpos > x.xpos ? -1 : 0) + moon.xvel,
                        moons.Where(x => x != moon).Sum(x => moon.ypos < x.ypos ? 1 : moon.ypos > x.ypos ? -1 : 0) + moon.yvel,
                        moons.Where(x => x != moon).Sum(x => moon.zpos < x.zpos ? 1 : moon.zpos > x.zpos ? -1 : 0) + moon.zvel
                    ));
                }
                moons = temp;

                findLowest(i, 0, moons[0], initial[0]);
                findLowest(i, 1, moons[1], initial[1]);
                findLowest(i, 2, moons[2], initial[2]);
                findLowest(i, 3, moons[3], initial[3]);
            }
            var values = loopCounter.OrderBy(x => x.Key.Item1).Select(x => x.Value + 1).ToArray();
            var moon0Cycle = lcm(values[0], lcm(values[1], values[2]));
            var moon1Cycle = lcm(values[3], lcm(values[4], values[5]));
            var moon2Cycle = lcm(values[6], lcm(values[7], values[8]));
            var moon3Cycle = lcm(values[9], lcm(values[10], values[11]));
            Console.WriteLine(moon0Cycle);
            Console.WriteLine(moon1Cycle);
            Console.WriteLine(moon2Cycle);
            Console.WriteLine(moon3Cycle);


            Console.WriteLine(loopCounter.Select(x=>(long)x.Value+1).Distinct().Aggregate((long)1, (current, next) => {
                return lcm(current, next);
            }));
            //Console.WriteLine(lcm(values[0], lcm(values[1], lcm(values[2], lcm(values[3], lcm(values[4], lcm(values[5], lcm(values[6], lcm(values[7], lcm(values[8], lcm(values[9], lcm(values[10], values[11]))))))))))));
            Console.WriteLine(moons.Sum(x => (Math.Abs(x.xpos) + Math.Abs(x.ypos) + Math.Abs(x.zpos)) * (Math.Abs(x.xvel) + Math.Abs(x.yvel) + Math.Abs(x.zvel))));
            Console.WriteLine("done.");
            Console.ReadLine();
        }

        static long gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static long lcm(long a, long b)
        {
            return (a / gcf(a, b)) * b;
        }
        static Dictionary<(int, int), int> loopCounter = new Dictionary<(int, int), int>();
        private static void findLowest(int i, int moonNumber, (int xpos, int ypos, int zpos, int xvel, int yvel, int zvel) current, (int xpos, int ypos, int zpos, int xvel, int yvel, int zvel) initial)
        {
            if (current.xpos == initial.xpos && current.xvel == initial.xvel && !loopCounter.ContainsKey((moonNumber, 0)))
            {
                loopCounter[(moonNumber, 0)] = i;
            }

            if (current.ypos == initial.ypos && current.yvel == initial.yvel && !loopCounter.ContainsKey((moonNumber, 1)))
            {
                loopCounter[(moonNumber, 1)] = i;
            }

            if (current.zpos == initial.zpos && current.zvel == initial.zvel && !loopCounter.ContainsKey((moonNumber, 2)))
            {
                loopCounter[(moonNumber, 2)] = i;
            }


        }
    }
}

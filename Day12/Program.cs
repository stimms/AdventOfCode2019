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
            for (int i = 0; loopCounter.Count < 3; i++)
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

                findLowest(i, 0, moons[0], moons[1], moons[2], moons[3]);

            }

            Console.WriteLine(loopCounter.Select(x=>x.value).Distinct().Aggregate((long)1, (current, next) =>
            {
                return lcm(current, next);
            }));
            
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
        static List<(int dimension,int value)> loopCounter = new List<(int dimension,int value)>();

        static HashSet<(int, int, int, int, int, int, int, int)> seenX = new HashSet<(int, int, int, int, int, int, int, int)>();
        static HashSet<(int, int, int, int, int, int, int, int)> seenY = new HashSet<(int, int, int, int, int, int, int, int)>();
        static HashSet<(int, int, int, int, int, int, int, int)> seenZ = new HashSet<(int, int, int, int, int, int, int, int)>();
        private static void findLowest(int i, int moonNumber, (int xpos, int ypos, int zpos, int xvel, int yvel, int zvel) moon0, (int xpos, int ypos, int zpos, int xvel, int yvel, int zvel) moon1, (int xpos, int ypos, int zpos, int xvel, int yvel, int zvel) moon2, (int xpos, int ypos, int zpos, int xvel, int yvel, int zvel) moon3)
        {
            if (seenX.Contains((moon0.xpos, moon0.xvel, moon1.xpos, moon1.xvel, moon2.xpos, moon2.xvel, moon3.xpos, moon3.xvel)) && !loopCounter.Any(x=>x.dimension == 0))
            {
                loopCounter.Add((0, i));
            }
            else
            {
                seenX.Add((moon0.xpos, moon0.xvel, moon1.xpos, moon1.xvel, moon2.xpos, moon2.xvel, moon3.xpos, moon3.xvel));
            }

            if (seenY.Contains((moon0.ypos, moon0.yvel, moon1.ypos, moon1.yvel, moon2.ypos, moon2.yvel, moon3.ypos, moon3.yvel)) && !loopCounter.Any(x=>x.dimension == 1))
            {
                loopCounter.Add((1,i));
            }
            else
            {
                seenY.Add((moon0.ypos, moon0.yvel, moon1.ypos, moon1.yvel, moon2.ypos, moon2.yvel, moon3.ypos, moon3.yvel));
            }

            if (seenZ.Contains((moon0.zpos, moon0.zvel, moon1.zpos, moon1.zvel, moon2.zpos, moon2.zvel, moon3.zpos, moon3.zvel)) && !loopCounter.Any(x=>x.dimension == 2))
            {
                loopCounter.Add((2,i));
            }
            else
            {
                seenZ.Add((moon0.zpos, moon0.zvel, moon1.zpos, moon1.zvel, moon2.zpos, moon2.zvel, moon3.zpos, moon3.zvel));
            }


        }
    }
}

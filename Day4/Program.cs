using System;
using System.IO;
using System.Linq;

namespace Advent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            int matching = 0;
            Console.WriteLine(matches(112233));
            Console.WriteLine(matches(123444));
            Console.WriteLine(matches(111122));
            for (int i = 152085; i <= 670283; i++)
            {
                if (matches(i))
                    matching++;
            }
            Console.WriteLine(matching);
            Console.WriteLine("done.");
            Console.ReadLine();
        }

        public static bool matches(int i)
        {
            if (!i.ToString().ToCharArray().OrderBy(x => x).SequenceEqual(i.ToString().ToCharArray()))
                return false;
            char prev = 'a';

            int seenCount = 0;
            foreach (var c in i.ToString().ToCharArray())
            {
                if (prev == c)
                {
                    seenCount++;
                }
                else
                {
                    if (seenCount == 2)
                    {
                        Console.WriteLine(i);
                        return true;
                    }
                    seenCount = 1;
                }
                prev = c;
            }
            if(seenCount == 2)
                return true;
            return false;
        }
    }
}

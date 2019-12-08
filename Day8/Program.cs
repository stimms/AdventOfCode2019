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
            var line = lines[0];
            var layers = new List<string>();
            for (int i = 0; i < line.Length / (6 * 25); i++)
            {
                layers.Add(line.Substring(i * 6 * 25, 6 * 25));
            }
            var workingLayer = layers.OrderBy(x => x.ToCharArray().Count(y => y == '0')).First();
            Console.WriteLine(workingLayer.ToCharArray().Count(x => x == '1') * workingLayer.ToCharArray().Count(x => x == '2'));


            var result = new char[150];
            
            int counter = 0;
            foreach (var pixel in result)
            {
                result[counter] = ' ';
                foreach (var layer in layers)
                {
                    if (layer[counter] == '1')
                    {
                        result[counter] = '*';
                        break;
                    }
                    if(layer[counter] == '0')
                        break;
                }
                counter++;
            }
            Console.WriteLine(String.Join("", result.Skip(0).Take(25).Select(x=>x.ToString())));
            Console.WriteLine(String.Join("", result.Skip(25).Take(25).Select(x=>x.ToString())));
            Console.WriteLine(String.Join("", result.Skip(50).Take(25).Select(x=>x.ToString())));
            Console.WriteLine(String.Join("", result.Skip(75).Take(25).Select(x=>x.ToString())));
            Console.WriteLine(String.Join("", result.Skip(100).Take(25).Select(x=>x.ToString())));
            Console.WriteLine(String.Join("", result.Skip(125).Take(25).Select(x=>x.ToString())));

            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Advent
{
    class Program
    {

        static int IMMEDIATE = 1;
        static int POSITION = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");

            var memory = lines[0].Split(",").Select(x => Int32.Parse(x)).ToArray();
            //Console.WriteLine(GetOpCode("1002"));

            var counter = 0;
            memory = lines[0].Split(",").Select(x => Int32.Parse(x)).ToArray();

            while (counter < memory.Count())
            {
                (int opCode, int mode1, int mode2, int mode3) = GetOpCode(memory[counter].ToString());
                if (memory[counter] == 99)
                    break;
                int parameter1 = mode1 == POSITION ? memory[memory[counter + 1]] : memory[counter + 1];
                int parameter2 = 0;
                int parameter3 = 0;
                if (opCode != 3 && opCode != 4)
                {
                    parameter2 = mode2 == POSITION ? memory[memory[counter + 2]] : memory[counter + 2] ;
                    parameter3 = memory[counter + 3] ;
                }
                if (opCode == 1)
                {
                    memory[parameter3] = parameter1 + parameter2;
                    counter += 4;
                }
                else if (opCode == 2)
                {
                    memory[parameter3] = parameter1 * parameter2;
                    counter += 4;
                }
                else if (opCode == 3)
                {
                    Console.Write(">");
                    memory[225] = 1;//Int32.Parse(Console.ReadLine());
                    counter += 2;
                }
                else if (opCode == 4)
                {
                    Console.Write(parameter1);
                    counter += 2;
                }
                
            }

            Console.WriteLine("done.");
            Console.ReadLine();
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

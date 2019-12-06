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

            Console.WriteLine(compute("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 5));//999
            Console.WriteLine(compute("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 8));//1000
            Console.WriteLine(compute("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 9));//1001
            Console.WriteLine(compute(lines[0], 5));
            Console.WriteLine("done.");
            Console.ReadLine();
        }

        private static string compute(string instructions, int input)
        {
            var counter = 0;
            var memory = instructions.Split(",").Select(x => Int32.Parse(x)).ToArray();

            string output = "";

            while (counter < memory.Count())
            {
                (int opCode, int mode1, int mode2, int mode3) = GetOpCode(memory[counter].ToString());
                if (memory[counter] == 99)
                    break;
                int parameter1 = mode1 == POSITION ? memory[memory[counter + 1]] : memory[counter + 1];
                int parameter2 = 0;
                int parameter3 = 0;
                if (opCode == 1 || opCode == 2 || opCode == 7 || opCode == 8)
                {
                    parameter2 = mode2 == POSITION ? memory[memory[counter + 2]] : memory[counter + 2];
                    parameter3 = mode3 == POSITION ? memory[memory[counter + 3]] : memory[counter + 3];
                }
                if (opCode == 5 || opCode == 6)
                {
                    parameter2 = mode2 == POSITION ? memory[memory[counter + 2]] : memory[counter + 2];
                }
                if (opCode == 1)
                {
                    memory[memory[counter + 3]] = parameter1 + parameter2;
                    counter += 4;
                }
                else if (opCode == 2)
                {
                    memory[memory[counter + 3]] = parameter1 * parameter2;
                    counter += 4;
                }
                else if (opCode == 3)
                {
                    Console.Write(">");
                    memory[memory[counter + 1]] = input;//Int32.Parse(Console.ReadLine());
                    counter += 2;
                }
                else if (opCode == 4)
                {
                    output += parameter1;
                    counter += 2;
                }

                else if (opCode == 5 && parameter1 != 0)
                {
                    counter = parameter2;
                }
                else if (opCode == 5)
                {
                    counter += 3;
                }

                else if (opCode == 6 && parameter1 == 0)
                {
                    counter = parameter2;
                }
                else if (opCode == 6)
                {
                    counter += 3;
                }

                else if (opCode == 7)
                {
                    memory[memory[counter + 3]] = parameter1 < parameter2 ? 1 : 0;
                    counter += 4;
                }
                else if (opCode == 8)
                {
                    memory[memory[counter + 3]] = parameter1 == parameter2 ? 1 : 0;
                    counter += 4;
                }

            }
            return output;
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

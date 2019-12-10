using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Advent
{
    class Program
    {

        static int RELATIVE = 2;
        static int IMMEDIATE = 1;
        static int POSITION = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");

            //compute("22201,1,2,5,99");
            Console.WriteLine(">>>1");
            compute("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", 0);
            Console.WriteLine(">>>2");
            compute("1102,34915192,34915192,7,4,7,99,0");
            Console.WriteLine(">>>3");
            compute("104,1125899906842624,99");
            Console.WriteLine(">>>4");
            compute(lines[0], 1);
            Console.WriteLine(">>>4");
            compute(lines[0], 2);
            Console.WriteLine("done.");
            Console.ReadLine();
        }

        private static string compute(string instructions, int input = 0)
        {
            var counter = 0;
            var relatvieBase = 0;
            var tempMemory = instructions.Split(",").Select(x => long.Parse(x)).ToList();
            tempMemory.AddRange(new long[10000]);
            var memory = tempMemory.ToArray();

            string output = "";

            while (counter < memory.Count())
            {
                (int opCode, int mode1, int mode2, int mode3) = GetOpCode(memory[counter].ToString());
                if (memory[counter] == 99)
                    break;
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
                    memory[parameter1] = input;//Int32.Parse(Console.ReadLine());
                    counter += 2;
                }
                else if (opCode == 4)
                {
                    output += memory[parameter1];
                    Console.WriteLine(memory[parameter1]);
                    counter += 2;
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

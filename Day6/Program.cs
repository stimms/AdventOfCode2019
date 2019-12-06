using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Advent
{

    class Node
    {
        public string Name { get; set; }
        public Node Parent { get; set; }
        public List<Node> Children { get; set; } = new List<Node>();
    }
    class Program
    {
        private static Node YOU;
        private static Node SAN;
        public static bool Add(Node root, string parent, string toAdd)
        {
            if (root.Name == parent)
            {
                var newNode = new Node { Name = toAdd, Parent = root };
                root.Children.Add(newNode);
                if (newNode.Name == "YOU")
                    YOU = newNode;
                if (newNode.Name == "SAN")
                    SAN = newNode;
                return true;
            }
            else
            {
                foreach (var child in root.Children)
                {
                    if (Add(child, parent, toAdd))
                        return true;
                }
            }
            return false;
        }

        public static int CountOrbits(Node root, int levelCount)
        {
            return root.Children.Select(x => CountOrbits(x, levelCount + 1) + levelCount).Sum(); ;
        }

        public static List<string> GetParentChain(Node leaf)
        {
            if (leaf.Parent != null)
            {
                var temp = GetParentChain(leaf.Parent);
                temp.Add(leaf.Name);
                return temp;
            }

            return new List<string>();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt").ToList();

            var root = new Node { Name = "COM" };
            BuildOrbits(lines, root);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(CountOrbits(root, 1));
            Console.ResetColor();
            FindCommonAncestor();

            Console.WriteLine("done.");
            Console.ReadLine();
        }

        private static void FindCommonAncestor()
        {
            var youParents = GetParentChain(YOU);
            var sanParents = GetParentChain(SAN);
            Console.WriteLine(String.Join(",", youParents));
            Console.WriteLine(String.Join(",", sanParents));


            youParents.Reverse();
            sanParents.Reverse();
            int youLevel = 0;
            foreach (var youParent in youParents)
            {
                var index = sanParents.FindIndex(x => x == youParent);
                if (index >= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(youParent + ":" + (index + youLevel - 2));
                    Console.ResetColor();
                    break;
                }
                youLevel++;
            }
        }

        private static void BuildOrbits(List<string> lines, Node root)
        {
            while (lines.Any())
            {
                var visited = new List<string>();
                foreach (var line in lines)
                {
                    if (Add(root, line.Split(")")[0], line.Split(")")[1]))
                        visited.Add(line);
                }
                lines = lines.Where(x => !visited.Contains(x)).ToList();
                //Console.WriteLine("To process: " + lines.Count());
            }
        }
    }
}

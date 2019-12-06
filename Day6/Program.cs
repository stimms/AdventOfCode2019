using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Advent
{

    class Node
    {
        public string Name { get; set; }
        public Node Parent {get;set;}
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
                if(newNode.Name == "YOU")
                    YOU = newNode;
                if(newNode.Name == "SAN")
                    SAN = newNode;
                return true;
            }
            else
            {
                foreach (var child in root.Children)
                {
                    if(Add(child, parent, toAdd))
                        return true;
                }
            }
            return false;
        }

        public static int Visit(Node root, int levelCount)
        {

            return root.Children.Select(x => Visit(x, levelCount + 1) + levelCount).Sum(); ;
        }

        public static string GetParentChain(Node leaf){
            if(leaf.Parent != null)
                return GetParentChain(leaf.Parent) + "," + leaf.Name;
            return "";
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt").ToList();
            
            var root = new Node { Name = "COM" };
            while (lines.Any())
            {
                var visited = new List<string>();
                foreach (var line in lines)
                {
                    if(Add(root, line.Split(")")[0], line.Split(")")[1]))
                        visited.Add(line);
                }
                lines = lines.Where(x=> !visited.Contains(x)).ToList();
                //Console.WriteLine(lines.Count());
            }
            var counter = 0;
            counter = Visit(root, 1);
            Console.WriteLine(counter);

            var youParents = GetParentChain(YOU);
            var sanParents = GetParentChain(SAN);
            Console.WriteLine(youParents);
            Console.WriteLine(sanParents);

            int youLevel = 0;
            foreach(var youParent in youParents.Split(",").Reverse())
            {
                var index = sanParents.Split(",").Reverse().ToList().FindIndex(x=>x==youParent);
                if(index>=0)
                    {
                        Console.WriteLine(youParent + ":"+ index + youLevel);
                        break;
                    }
                youLevel++;
            }

            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Day_3
{
    class Program
    {
        static void Main(string[] args)
        {
            var slope = new Slope();
            var treesCount = slope.TraverseAndCountTrees(3, 1);

            Console.WriteLine($"We found {treesCount} on our path with (Right 3 and down 1) as step!");

            long multiplyStepVariations = 1;
            multiplyStepVariations *= slope.TraverseAndCountTrees(1, 1);
            multiplyStepVariations *= slope.TraverseAndCountTrees(3, 1);
            multiplyStepVariations *= slope.TraverseAndCountTrees(5, 1);
            multiplyStepVariations *= slope.TraverseAndCountTrees(7, 1);
            multiplyStepVariations *= slope.TraverseAndCountTrees(1, 2);

            Console.WriteLine($"When checking multiple variations, the multiplication of trees hit equals {multiplyStepVariations}!");
        }
    }

    class Slope
    {
        private List<List<Space>> tileset;

        public Slope()
        {
            this.tileset = new List<List<Space>>();

            using (var sr = new StreamReader("input"))
            {
                for (; ; )
                {
                    var line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    var slopeLine = new List<Space>();
                    foreach (char c in line)
                    {
                        try
                        {
                            var space = new Space(c);
                            slopeLine.Add(space);
                        }
                        catch (FormatException e)
                        {
                            throw new Exception($"Unable to parse '{line}': '{e.Message}'");
                        }
                    }
                    this.tileset.Add(slopeLine);
                }
            }
        }

        public int TraverseAndCountTrees(int rightStep, int downStep)
        {
            int treesCount = 0;
            // Traverse through our List<List<Space>> as list[y][x]
            int x = 0; // Inner list coordinate
            int y = 0; // Outer list coordinate


            do
            {
                x += rightStep;
                y += downStep;

                var line = tileset[y];
                x %= line.Count; // Pattern repeats to the right

                switch (line[x].GetSpaceType())
                {
                    case SpaceType.Tree:
                        treesCount++;
                        break;
                }
            }
            while (y < tileset.Count - downStep);

            return treesCount;
        }

        public void Print()
        {
            foreach (var slopeLine in tileset)
            {
                foreach (var space in slopeLine)
                {
                    Console.Write($"{space}");
                }
                Console.WriteLine();
            }
        }
    }


    enum SpaceType
    {
        Open,
        Tree
    }

    class Space
    {
        private SpaceType type;

        public Space(char a)
        {
            switch (a)
            {
                case '.':
                    type = SpaceType.Open;
                    break;
                case '#':
                    type = SpaceType.Tree;
                    break;
                default:
                    throw new Exception($"Unknown space char provided '{a}'");
            }
        }

        public SpaceType GetSpaceType()
        {
            return type;
        }

        public override string ToString()
        {
            switch (type)
            {
                case SpaceType.Open:
                    return ".";
                case SpaceType.Tree:
                    return "#";
                default:
                    throw new Exception("What type is this?");
            }
        }
    }
}

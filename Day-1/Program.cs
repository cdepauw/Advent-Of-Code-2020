using System;
using System.Collections.Generic;
using System.IO;

namespace Day_1
{
    class Program
    {
        private List<long> numbers;
        private long target;

        public Program(long target)
        {
            this.target = target;
            this.numbers = new List<long>();

            using (var sr = new StreamReader("input.csv"))
            {
                // Read and process until done
                for (; ; )
                {
                    var line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    try
                    {
                        long n = long.Parse(line);
                        numbers.Add(n);
                    }
                    catch (FormatException e)
                    {
                        throw new Exception($"Unable to parse '{line}': '{e.Message}'");
                    }
                }
            }
        }

        // Find the 2 numbers that sum to this.target.
        // Return the multiplication of these 2 numbers.
        public long Find2Numbers()
        {
            for (int i = 0; i < numbers.Count - 2; i++)
            {
                for (int j = i + 1; j < numbers.Count - 1; j++)
                {
                    if (numbers[i] + numbers[j] == this.target)
                    {
                        return numbers[i] * numbers[j];
                    }
                }
            }

            throw new Exception($"Unable to find numbers that sum to {this.target}");
        }

        // Find the 3 numbers that sum to this.target.
        // Return the multiplication of these 3 numbers.
        public long Find3Numbers()
        {
            for (int i = 0; i < numbers.Count - 2; i++)
            {
                for (int j = i + 1; j < numbers.Count - 1; j++)
                {
                    // Skip if we already exceeded the target
                    if (numbers[i] + numbers[j] > this.target)
                    {
                        continue;
                    }

                    for (int k = i + 2; k < numbers.Count; k++)
                    {
                        if (numbers[i] + numbers[j] + numbers[k] == this.target)
                        {
                            return numbers[i] * numbers[j] * numbers[k];
                        }
                    }
                }
            }

            throw new Exception($"Unable to find numbers that sum to {this.target}");
        }

        static void Main(string[] args)
        {
            var p = new Program(2020);
            Console.WriteLine($"The multiplication of 2 numbers is: {p.Find2Numbers()}");
            Console.WriteLine($"The multiplication of 3 numbers is: {p.Find3Numbers()}");
        }
    }
}

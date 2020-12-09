using System;
using System.Collections.Generic;
using System.IO;

namespace Day_9
{
    class Program
    {
        private List<long> numbers;
        private int preambleLength;

        public Program(int preambleLength)
        {
            this.numbers = new List<long>();
            this.preambleLength = preambleLength;

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

        public List<long> GetNumbers()
        {
            return numbers;
        }

        // AttackXMAS executes the following logic:
        //  1) Find the odd value that is not a sum of 2 of its predecessors.
        //     The number of predecessors to choose from is given by preambleLength.
        //  2) Find the series of numbers that sum to this odd value.
        //  3) Return the sum of the smallest and largest numbers of this series of 2).
        public long AttackXMAS()
        {
            long oddValue = -1;
            for (int i = preambleLength; i < numbers.Count; i++)
            {
                if (!validateNumber(numbers[i], numbers.GetRange(i - preambleLength, preambleLength)))
                {
                    oddValue = numbers[i];
                    break;
                }
            }

            long crackedValue = -1;
            for (int i = 0; i < numbers.Count; i++)
            {
                long calculatedValue = numbers[i];
                for (int j = i + 1; j < numbers.Count; j++)
                {
                    calculatedValue += numbers[j];
                    if (calculatedValue == oddValue)
                    {
                        List<long> vs = numbers.GetRange(i, j - i);
                        vs.Sort();
                        crackedValue = vs[0] + vs[vs.Count - 1];
                        return crackedValue;
                    }
                    else if (calculatedValue > oddValue)
                    {
                        break;
                    }
                }
            }

            return -1;
        }

        private bool validateNumber(long target, List<long> values)
        {
            // Split the values into odds and evens.
            // This will let us pick our search groups.
            List<long> evens = new List<long>();
            List<long> odds = new List<long>();

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] % 2 == 0)
                {
                    evens.Add(values[i]);
                }
                else
                {
                    odds.Add(values[i]);
                }
            }

            // Depending on the target being even or odd, we can 
            // select our search groups defined earlier.
            bool targetFound = false;
            if (target % 2 == 0)
            {
                // Even:
                //  - 2 even numbers
                //  - 2 odd numbers

                for (int i = 0; i < evens.Count; i++)
                {
                    for (int j = 0; j < evens.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        if (evens[i] + evens[j] == target)
                        {
                            targetFound = true;
                            goto TargetFoundSkip;
                        }
                    }
                }

                for (int i = 0; i < odds.Count; i++)
                {
                    for (int j = 0; j < odds.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        if (odds[i] + odds[j] == target)
                        {
                            targetFound = true;
                            goto TargetFoundSkip;
                        }
                    }
                }
            }
            else
            {
                // Odd:
                //  - 1 even and 1 odd number
                for (int i = 0; i < evens.Count; i++)
                {
                    for (int j = 0; j < odds.Count; j++)
                    {
                        if (evens[i] + odds[j] == target)
                        {
                            targetFound = true;
                            goto TargetFoundSkip;
                        }
                    }
                }
            }

        TargetFoundSkip:
            return targetFound;
        }

        static void Main(string[] args)
        {
            var p = new Program(25);

            Console.WriteLine($"Loaded input sequence, totalling {p.GetNumbers().Count} numbers.");
            Console.WriteLine("Attacking XMAS...");

            var attackResult = p.AttackXMAS();
            if (attackResult == -1)
            {
                Console.WriteLine($"Attack Failed..");

            }
            else
            {
                Console.WriteLine($"Attack successful! Value: {p.AttackXMAS()}");

            }
        }
    }
}

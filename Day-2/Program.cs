using System;
using System.IO;
using System.Resources;

namespace Day_2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sr = new StreamReader("input.csv"))
            {
                var validCount = 0;
                var invalidCount = 0;
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
                        var p = new Password(line);
                        if (!p.IsValid())
                        {
                            Console.WriteLine($"Invalid password found. Raw input: {line}");
                            invalidCount++;
                        }
                        else
                        {
                            validCount++;
                        }
                    }
                    catch (FormatException e)
                    {
                        throw new Exception($"Unable to parse '{line}': '{e.Message}'");
                    }
                }
                Console.WriteLine($"Valid passwords: {validCount}");
                Console.WriteLine($"Invalid passwords: {invalidCount}");
            }
        }
    }

    class Password
    {
        private char requiredLetter;
        private int positionOne, positionTwo;
        private string password;

        // Create a new password type from a raw description.
        // Raw description example: 
        //  1-5 k: kkkkhkkkkkkkkkk
        public Password(string rawDescription)
        {
            var split = rawDescription.Split(' ');
            if (split.Length < 3)
            {
                throw new Exception($"Password line contained an incorrect number of fields. Input: {rawDescription}");
            }

            var positions = split[0].Split('-');
            this.positionOne = int.Parse(positions[0]) - 1;
            this.positionTwo = int.Parse(positions[1]) - 1;

            this.requiredLetter = split[1].Substring(0, 1).ToCharArray()[0]; // Truncate ':'
            this.password = split[2];
        }

        public bool IsValid()
        {
            if (positionOne > password.Length - 1 && positionTwo > password.Length - 1)
            {
                return false;
            }
            else if (positionOne > password.Length - 1)
            {
                return password[positionTwo] == requiredLetter;
            }
            else if (positionTwo > password.Length - 1)
            {
                return password[positionOne] == requiredLetter;
            }

            return password[positionOne] == requiredLetter ^ password[positionTwo] == requiredLetter;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY2
    {
        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(2));
            Console.WriteLine(Problem1(linesInput));
            Console.WriteLine(Problem2(linesInput));
        }
        public static int Problem1(string[] linesInput)
        {
            int twoTimes = 0;
            int threeTimes = 0;

            foreach (string lines in linesInput)
            {
                var lstChars = lines.GroupBy(r => r);
                if (lstChars.Any(r => r.Count() == 3))
                    threeTimes++;
                if (lstChars.Any(r => r.Count() == 2))
                    twoTimes++;
            }

            int leChecksum = twoTimes * threeTimes;

            //output
            return leChecksum;
        }

        public static void Problem2_OLD(string[] linesInput)
        {
            for (int i = 0; i < linesInput.Length; i++)
            {
                for (int j = i + 1; j < linesInput.Length; j++)
                {
                    if (Util.oneDifference(linesInput[i], linesInput[j]))
                    {
                        Console.WriteLine(linesInput[i]);
                        Console.WriteLine(linesInput[j]);
                        Console.WriteLine(i);
                        Console.WriteLine(j);
                    }
                }
            }
        }

        public static void Problem2_OLD_Leven(string[] linesInput)
        {
            for (int i = 0; i < linesInput.Length; i++)
            {
                for (int j = i + 1; j < linesInput.Length; j++)
                {
                    if (Util.LevenshteinDistance.Compute(linesInput[i], linesInput[j]) == 1)
                    {
                        Console.WriteLine(linesInput[i]);
                        Console.WriteLine(linesInput[j]);
                    }
                }
            }
        }

        public static string Problem2(string[] linesInput)
        {
            var targetLength = linesInput[0].Length - 1;

            for (var i = 0; i < linesInput.Length; i++)
            {
                var match = linesInput.Skip(i + 1)
                    .Select(it => it.Zip(linesInput[i], (a, b) => new { A = a, B = b }).Where(rw => rw.A == rw.B))
                    .Where(it => it.Count() == targetLength)
                    .FirstOrDefault();

                if (match != null)
                    return string.Join(string.Empty, match.Select(it => it.A));
            }

            return null;
        }

    }
}

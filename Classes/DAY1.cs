using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY1
    {
        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(1));
            Console.WriteLine(Problem1(linesInput));
            Console.WriteLine(Problem2(linesInput));
        }

        public static int Problem1(string[] lines)
        {
            List<int> lstCalibration = new List<int>();
            foreach (string line in lines)
            {
                int result = 0;
                Int32.TryParse(line, out result);
                lstCalibration.Add(result);
            }

            int calibrationResult = lstCalibration.Sum(r => r);

            return calibrationResult;
        }

        public static long Problem2(string[] lines)
        {
            bool keepGoing = true;
            long repeatedFreq = 0;

            long totalCalibration = 0;

            Dictionary<long, long> calibrationResults = new Dictionary<long, long>();

            while (keepGoing)
            {
                foreach (string line in lines)
                {
                    int result = 0;
                    Int32.TryParse(line, out result);
                    totalCalibration += result;
                    if (calibrationResults.ContainsKey(totalCalibration))
                    {
                        keepGoing = false;
                        repeatedFreq = totalCalibration;
                        break;
                    }
                    calibrationResults.Add(totalCalibration, totalCalibration);
                }
            }

            return repeatedFreq;
        }
    }
}

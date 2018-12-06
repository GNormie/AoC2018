using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY1
    {
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
    class DAY2
    {
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
    class DAY3
    {
        // Can problem 1 be done with squares intersect? 
        public static void Problem1(string[] linesInput)
        {
            int overlaps = 0;

            int[,] bigFabric = new int[1000, 1000];

            foreach (var line in linesInput)
            {
                var parts = line.Split(' ');

                var coords = parts[2].Remove(parts[2].Length - 1, 1).Split(',');
                var xCoord = int.Parse(coords[0]);
                var yCoord = int.Parse(coords[1]);

                var size = parts[3].Split('x');
                var xSize = int.Parse(size[0]);
                var ySize = int.Parse(size[1]);

                for (int i = xCoord; i < xCoord + xSize; i++)
                {
                    for (int y = yCoord; y < yCoord + ySize; y++)
                    {
                        bigFabric[i, y] = bigFabric[i, y] + 1;
                    }
                }
            }

            for (int i = 0; i < bigFabric.GetLength(0); i++)
            {
                for (int j = 0; j < bigFabric.GetLength(1); j++)
                {
                    if (bigFabric[i, j] > 1)
                        overlaps++;
                }
            }


            Console.WriteLine(overlaps);
        }

        public static void Problem2(string[] linesInput)
        {
            Dictionary<int, Rectangle> lstRectangle = new Dictionary<int, Rectangle>();
            for (int i = 0; i < linesInput.Length; i++)
            {
                var parts = linesInput[i].Split(' ');
                int key = Int32.Parse(linesInput[i].Between("#", " @"));
                var coords = parts[2].Remove(parts[2].Length - 1, 1).Split(',');
                var xCoord = int.Parse(coords[0]);
                var yCoord = int.Parse(coords[1]);
                var size = parts[3].Split('x');
                var xSize = int.Parse(size[0]);
                var ySize = int.Parse(size[1]);
                lstRectangle.Add(key, new Rectangle(xCoord, yCoord, xSize, ySize));
            }

            for (int i = 1; i < lstRectangle.Count() + 1; i++)
            {
                var THE_ONE = lstRectangle.Where(r => r.Key != i && r.Value.IntersectsWith(lstRectangle[i]) == true).ToList();
                if (THE_ONE != null && THE_ONE.Count == 0)
                {
                    Console.WriteLine(i);
                    break;
                }     
            }
        }
    }
    class DAY4
    {
        public static void Problem1and2(string[] linesInput)
        {
            Dictionary<DateTime, string> dctEntries = new Dictionary<DateTime, string>();
            foreach (string line in linesInput)
            {
                //[1518-11-09 00:03]
                string dateSegment = line.Between("[", "]");
                DateTime timeLog = DateTime.ParseExact(dateSegment, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                dctEntries.Add(timeLog, line.Between("] "));
            }

            var orderInum = dctEntries.OrderBy(r => r.Key);
            Dictionary<int, List<Sonambulus>> Guards = new Dictionary<int, List<Sonambulus>>();

            int currentGuardID = 0;
            foreach (var entry in orderInum)
            {
                if (entry.Value.Contains("Guard"))
                {
                    string strGuardID = entry.Value.Between("Guard #", " begins");
                    currentGuardID = Convert.ToInt32(strGuardID);
                    continue;
                }

                if (Guards.ContainsKey(currentGuardID) == false)
                {
                    Guards.Add(currentGuardID, new List<Sonambulus>());
                }

                if (entry.Value.Contains("asleep"))
                {
                    Guards[currentGuardID].Add(new Sonambulus(false, entry.Key.Minute, entry.Key));
                }
                else if (entry.Value.Contains("wakes"))
                {
                    Guards[currentGuardID].Add(new Sonambulus(true, entry.Key.Minute, entry.Key));
                }

            }

            Dictionary<int, List<WakeSleepPair>> GuardsSleep = new Dictionary<int, List<WakeSleepPair>>();

            foreach (var guardLog in Guards)
            {
                List<Sonambulus> lstGuardSleeps = guardLog.Value;
                var GuardDays = lstGuardSleeps.GroupBy(r => r.eventDate.Date).OrderByDescending(r => r.Key).ToList();

                foreach (var a in GuardDays)
                {
                    var OEnumGuardSleeps = lstGuardSleeps.Where(r => r.eventDate.Date == a.Key).OrderByDescending(r => r.eventDate).ToList();
                    while (OEnumGuardSleeps.Count > 0)
                    {
                        var firstAwake = OEnumGuardSleeps.FirstOrDefault(r => r.awake == true);
                        var firstSleep = OEnumGuardSleeps.FirstOrDefault(r => r.awake == false);

                        if (firstAwake != null && firstSleep != null)
                        {
                            WakeSleepPair sleepyTimes = new WakeSleepPair(firstAwake.switchStateMin, firstSleep.switchStateMin);
                            if (GuardsSleep.ContainsKey(guardLog.Key) == false)
                                GuardsSleep.Add(guardLog.Key, new List<WakeSleepPair>());
                            GuardsSleep[guardLog.Key].Add(sleepyTimes);
                            OEnumGuardSleeps.Remove(firstAwake);
                            OEnumGuardSleeps.Remove(firstSleep);
                        };
                    }
                }
            }

            //minute asleep the most
            List<ResultStruct> lstResults = new List<ResultStruct>();

            foreach (var entry in GuardsSleep)
            {
                List<int> totalAsleep = new List<int>();
                List<int> sleepHoursRange = new List<int>();

                foreach (var sleepyTimes in entry.Value)
                {
                    var sleepyRanges = Enumerable.Range(sleepyTimes.sleep, sleepyTimes.wake - sleepyTimes.sleep);
                    totalAsleep.Add(sleepyTimes.wake - sleepyTimes.sleep);
                    foreach (var sleepMin in sleepyRanges)
                    {
                        sleepHoursRange.Add(sleepMin);
                    }
                }

                var sleepRangeSorted = sleepHoursRange.GroupBy(r => r).ToList();
                var mostCommonSleepMin = sleepRangeSorted.OrderByDescending(r => r.Count()).First();

                lstResults.Add(new ResultStruct(entry.Key, mostCommonSleepMin.Key, mostCommonSleepMin.Count(), totalAsleep.Sum()));
            }

            //PART 1
            var finalResult = lstResults.OrderByDescending(r => r.TotalAmountOfSleep).First();

            Console.WriteLine("GUARD ID: " + finalResult.GuardID);
            Console.WriteLine("COMMON MIN: " + finalResult.CommonHour);
            Console.WriteLine("FOUND: " + finalResult.AmountOfTimes);
            Console.WriteLine("SLEEP TOTAL: " + finalResult.TotalAmountOfSleep);
            Console.WriteLine("Answer: " + finalResult.GuardID * finalResult.CommonHour);

            Console.WriteLine();
            //PART 2
            finalResult = lstResults.OrderByDescending(r => r.AmountOfTimes).First();

            Console.WriteLine("GUARD ID: " + finalResult.GuardID);
            Console.WriteLine("COMMON MIN: " + finalResult.CommonHour);
            Console.WriteLine("FOUND: " + finalResult.AmountOfTimes);
            Console.WriteLine("SLEEP TOTAL: " + finalResult.TotalAmountOfSleep);
            Console.WriteLine("Answer: " + finalResult.GuardID * finalResult.CommonHour);

            Console.ReadLine();
        }
        public class ResultStruct
        {
            public int GuardID = 0;
            public int CommonHour = 0;
            public int AmountOfTimes = 0;
            public int TotalAmountOfSleep = 0;

            public ResultStruct(int _GuardID, int _CommonHour, int _AmountOfTimes, int _TotalAmountOfSleep)
            {
                GuardID = _GuardID;
                CommonHour = _CommonHour;
                AmountOfTimes = _AmountOfTimes;
                TotalAmountOfSleep = _TotalAmountOfSleep;
            }
        }

        public class WakeSleepPair
        {
            public int wake = 0;
            public int sleep = 0;

            public WakeSleepPair(int _wake, int _sleep)
            {
                wake = _wake;
                sleep = _sleep;
            }
        }

        public class Guard
        {
            public int ID;
            public List<Sonambulus> lstSonambulus = new List<Sonambulus>();
        }

        public class Sonambulus
        {
            public bool awake = true;
            public int switchStateMin;
            public DateTime eventDate;

            public Sonambulus(bool _awake, int _switchStateMin, DateTime _eventDate)
            {
                awake = _awake;
                switchStateMin = _switchStateMin;
                eventDate = _eventDate;
            }
        }
    }
    class DAY5
    {
        /// <summary>
        /// NOTE: Accept only first line of input
        /// </summary>
        /// <param name="linesInput"></param>
        /// <returns></returns>
        public static int Problem1(string linesInput)
        {
            List<char> originalInput = linesInput.ToList();
            string result = ReactPolymerStack(originalInput);
            return result.Length;
        }

        /// <summary>
        /// NOTE: Accept only first line of input
        /// </summary>
        /// <param name="linesInput"></param>
        /// <returns></returns>
        public static int Problem2(string linesInput)
        {
            List<char> originalInput = linesInput.ToList();
            List<Result> lstResult = new List<Result>();

            var DistinctChars = originalInput.GroupBy(r => Char.ToLower(r));

            foreach (char caract in DistinctChars.Select(r => r.Key))
            {
                List<char> line = originalInput;
                line = line.Where(r => Char.ToLower(r) != caract).ToList();
                string result = ReactPolymerStack(line); //ReactPolymer(line);
                lstResult.Add(new Result(caract, result.Length, result));
            }

            return lstResult.OrderBy(r => r.lineLength).First().lineLength;
        }

        private class Result
        {
            public char offendingCharacter;
            public int lineLength;
            public string strResult;

            public Result(char _o, int _l, string _s)
            {
                offendingCharacter = _o;
                lineLength = _l;
                strResult = _s;
            }
        }

        private static string ReactPolymerStack(List<char> line)
        {
            Stack<char> characterStack = new Stack<char>();

            foreach (char currentChar in line)
            {
                if (characterStack.Count == 0)
                    characterStack.Push(currentChar);
                else
                {
                    char topChar = characterStack.Peek();
                    if (Char.ToLower(topChar) == Char.ToLower(currentChar) && topChar != currentChar)
                    {
                        characterStack.Pop();
                    }
                    else
                    {
                        characterStack.Push(currentChar);
                    }
                }
            }
            return (String.Concat(characterStack.ToArray()));
        }

        private static string ReactPolymer(List<char> line)
        {
            bool removalTookPlace = true;

            do
            {
                removalTookPlace = false;
                for (int i = 0; i < line.Count(); i++)
                {
                    if (i + 1 > line.Count - 1)
                        break;
                    if (Char.ToLower(line[i]) == Char.ToLower(line[i + 1]))
                    {
                        if (line[i] != line[i + 1])
                        {
                            line.RemoveAt(i + 1);
                            line.RemoveAt(i);
                            removalTookPlace = true;
                        }
                    }
                }

            } while (removalTookPlace == true);

            return (String.Concat(line.ToArray()));
        }
    }
}

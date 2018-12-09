using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY4
    {
        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(4));
            Problem1and2(linesInput);
        }

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
}

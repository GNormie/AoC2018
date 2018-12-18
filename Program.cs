using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.Diagnostics;
using System.Drawing;

namespace AoC2018
{
    class Program
    {
        static char[,] grid = new char[2000, 2000];
        static Dictionary<Point, char> dctMap = new Dictionary<Point, char>();
        static char Water = '~';
        static char Wet = '|';
        static char Clay = '#';

        public static int minYBound = 0;
        public static int maxYBound = 0;

        static void Main(string[] args)
        {
            #region CONEMU
#if DEBUG
            ProcessStartInfo pi = new ProcessStartInfo(@"C:\cmder\vendor\conemu-maximus5\ConEmu\ConEmuC.exe", "/AUTOATTACH");
            pi.CreateNoWindow = false;
            pi.UseShellExecute = false;
            //pi.WindowStyle = ProcessWindowStyle.Maximized;
            Process.Start(pi);
#endif
            #endregion
            string[] linesInput = File.ReadAllLines(@"C:\puzzle17.txt");

            //build map
            foreach (string line in linesInput)
            {
                //x=571, y=1864..1875
                //y=1700, x=570..588

                int xValueFrom = -1;
                int xValueTo = -1;

                int yValueFrom = -1;
                int yValueTo = -1;

                string[] splitLine = line.Split(',');
                string leftValue = splitLine[0].Substring(2).Trim();
                if (splitLine[0][0] == 'x')
                {
                    xValueFrom = Convert.ToInt32(leftValue);
                    string[] rightValues = splitLine[1].Trim().Substring(2).Split('.');
                    yValueFrom = Convert.ToInt32(rightValues[0]);
                    yValueTo = Convert.ToInt32(rightValues[2]);
                    for (int y = yValueFrom; y < yValueTo + 1; y++)
                    {
                        if (dctMap.ContainsKey(new Point(xValueFrom, y)) == false)
                            dctMap.Add(new Point(xValueFrom, y), '#');
                    }
                }
                else
                {
                    yValueFrom = Convert.ToInt32(leftValue);
                    string[] rightValues = splitLine[1].Trim().Substring(2).Split('.');
                    xValueFrom = Convert.ToInt32(rightValues[0]);
                    xValueTo = Convert.ToInt32(rightValues[2]);
                    for (int x = xValueFrom; x < xValueTo + 1; x++)
                    {
                        if (dctMap.ContainsKey(new Point(x, yValueFrom)) == false)
                            dctMap.Add(new Point(x, yValueFrom), '#');
                    }
                }
            }

            minYBound = dctMap.Min(r => r.Key.Y);
            maxYBound = dctMap.Max(r => r.Key.Y);
            Queue<Point> waterFlow = new Queue<Point>();

            //PrintMap();
            int lastHydroCount = 0;
            int sameCounter = 0;
            bool keepGoing = true;
            while (keepGoing)
            {
                waterFlow.Enqueue(new Point(500, minYBound - 1));
                while (waterFlow.Count() > 0)
                {
                    Flow(waterFlow);
                }
                tryMorph();
                /*
                var stillCandidate = dctMap.Where(r => r.Value == '|').GroupBy(r => r.Key.Y).ToList();
                foreach (var candidate in stillCandidate)
                {
                    if (candidate.All(r => stillWaterCandidate(r.Key)))
                    {
                        foreach (var can in candidate)
                            dctMap[can.Key] = '~';
                    }
                }*/

                //PrintMap();
                //Console.WriteLine(hydroCount());
                if (lastHydroCount == hydroCount())
                    sameCounter++;
                else
                    sameCounter = 0;
                lastHydroCount = hydroCount();
                if (sameCounter >= 3)
                {
                    keepGoing = false;
                    Console.WriteLine("PART 1: " + lastHydroCount);
                    ClearWet();
                    Console.WriteLine("PART 2: " + hydroCount());
                }
                //PrintMap();
                ClearWet();
            }
            Console.ReadLine();
        }

        public static void tryMorph()
        {
            var greaterCandidate = dctMap.Where(r => r.Value == '-').OrderBy(r => r.Key.X).GroupBy(w => w.Key.Y).ToList();
            foreach (var stillCandidate in greaterCandidate)
            {
                if (stillCandidate.All(r => isHidrated(onMyLeft(r.Key)) && isHidrated(onMyRight(r.Key))))
                {
                    foreach (var can in stillCandidate)
                        dctMap[can.Key] = '~';
                }
                else
                {
                    var barriersOnThisLevel = dctMap.Where(r => r.Key.Y == stillCandidate.Key && r.Value == '#').OrderBy(w => w.Key.X).ToList();
                    if (barriersOnThisLevel.Count >= 2)
                    {
                        for (int i = 0; i < barriersOnThisLevel.Count() - 1; i++)
                        {
                            var leftBound = barriersOnThisLevel[i];
                            var rightBound = barriersOnThisLevel[i + 1];
                            var muchosElementos = dctMap.Where(r => r.Key.Y == stillCandidate.Key && r.Key.X > leftBound.Key.X && r.Key.X < rightBound.Key.X && r.Value == '-').ToList();
                            if (muchosElementos.Count > 0)
                            {
                                if (muchosElementos.All(r => isHidrated(onMyLeft(r.Key)) && isHidrated(onMyRight(r.Key))))
                                {
                                    foreach (var thing in muchosElementos)
                                    {
                                        dctMap[thing.Key] = '~';
                                    }
                                }
                                else
                                {
                                    foreach (var thing in muchosElementos)
                                    {
                                        dctMap[thing.Key] = '|';
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var can in stillCandidate)
                            dctMap[can.Key] = '|';
                    }
                }
            }
        }

        public static void PrintMap()
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < 2000; j++)
            {
                for (int i = 0; i < 2000; i++)
                {
                    if (dctMap.ContainsKey(new Point(i, j)))
                        sb.Append(dctMap[new Point(i, j)]);
                    else
                        sb.Append('.');
                }
                sb.Append(Environment.NewLine);
            }
            Util.WriteToFile(sb);
        }

        public static void ClearWet()
        {
            foreach (var item in dctMap.Where(r => r.Value == '|' || r.Value == '-').ToList())
            {
                dctMap.Remove(item.Key);
            }
        }


        public static int hydroCount()
        {
            return dctMap.Count(r => (r.Key.Y >= minYBound && r.Key.Y <= maxYBound) && (r.Value == '-' || r.Value == Wet || r.Value == Water));
        }

        public static bool isHidrated(Point P)
        {
            if (dctMap.ContainsKey(P) == false)
                return false;
            else
            {
                if (dctMap[P] == Clay || dctMap[P] == Water || dctMap[P] == '-')
                    return true;
                else
                    return false;
            }
        }

        public static bool isObstructed(Point P)
        {
            if (dctMap.ContainsKey(P) == false)
                return false;
            else
            {
                if (dctMap[P] == Clay || dctMap[P] == Water)
                    return true;
                else
                    return false;
            }
        }

        public static bool isOpen(Point P)
        {
            if (dctMap.ContainsKey(P) == false || dctMap[P] == Wet)
                return true;
            else
            {
                if (dctMap[P] == Clay || dctMap[P] == Water)
                    return false;
                else
                    return true;
            }
        }

        public static bool stillWaterCandidate(Point P)
        {
            if (!dctMap.ContainsKey(P) || !dctMap.ContainsKey(onMyBottom(P)) || !dctMap.ContainsKey(onMyLeft(P)) || !dctMap.ContainsKey(onMyRight(P)))
                return false;
            else
            {
                if (dctMap[P] == Wet && isObstructed(onMyBottom(P)) &&
                    (isObstructed(onMyLeft(P)) || dctMap[onMyLeft(P)] == Wet) &&
                    (isObstructed(onMyRight(P)) || dctMap[onMyRight(P)] == Wet))
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public static Queue<Point> Flow(Queue<Point> returnValue)
        {
            Point P = returnValue.Dequeue();
            if (P.Y > maxYBound)
                return returnValue;
            if (isObstructed(onMyBottom(P)) == false)
            {
                if (dctMap.ContainsKey(onMyBottom(P)) == false)
                    dctMap.Add(onMyBottom(P), '|');
                returnValue.Enqueue(onMyBottom(P));
                return returnValue;
            }
            else
            {
                dctMap[P] = '-';
                if (isObstructed(onMyLeft(P)) == false)
                {
                    if (dctMap.ContainsKey(onMyLeft(P)) == false)
                    {
                        dctMap.Add(onMyLeft(P), '|');
                        returnValue.Enqueue(onMyLeft(P));
                    }
                }
                if (isObstructed(onMyRight(P)) == false)
                {
                    if (dctMap.ContainsKey(onMyRight(P)) == false)
                    {
                        dctMap.Add(onMyRight(P), '|');
                        returnValue.Enqueue(onMyRight(P));
                    }
                }
                //PrintMap();
                return returnValue;
            }
        }

        public static Point onMyLeft(Point p)
        {
            return new Point(p.X - 1, p.Y);
        }
        public static Point onMyRight(Point p)
        {
            return new Point(p.X + 1, p.Y);
        }
        public static Point onMyBottom(Point p)
        {
            return new Point(p.X, p.Y + 1);
        }
        public static Point onMyTop(Point p)
        {
            return new Point(p.X, p.Y - 1);
        }

    }
}
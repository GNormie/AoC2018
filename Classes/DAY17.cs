using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY17
    {
        static char[,] grid = new char[2000, 2000];
        static Dictionary<Point, char> dctMap = new Dictionary<Point, char>();
        static char Water = '~';
        static char Wet = '|';
        static char Clay = '#';

        public static int minYBound = 0;
        public static int maxYBound = 0;

        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(17));
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

            int lastHydroCount = 0;
            int sameCounter = 0;
            bool keepGoing = true;
            while (keepGoing)
            {
                waterFlow.Enqueue(new Point(500, minYBound - 1));
                while (waterFlow.Count() > 0)
                {
                    Flow(waterFlow);
                    //Console.WriteLine(hydroCount()); <--really slows it down
                }

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
                {
                    dctMap.Add(onMyBottom(P), '|');
                    returnValue.Enqueue(onMyBottom(P));
                }
                return returnValue;
            }
            else
            {
                HorizontalFlow(P, returnValue);
                return returnValue;
            }
        }

        public static void HorizontalFlow(Point P, Queue<Point> leQueue)
        {
            List<Point> toTheLeft = new List<Point>();
            List<Point> toTheRight = new List<Point>();

            bool continueLeft = true;
            bool leftLeaks = false;
            Point workingLeftPoint = P;
            while (continueLeft)
            {
                if (isObstructed(onMyLeft(workingLeftPoint)) == false)
                {
                    workingLeftPoint = onMyLeft(workingLeftPoint);
                    toTheLeft.Add(workingLeftPoint);
                    if (isObstructed(onMyBottom(workingLeftPoint)) == false)
                    {
                        continueLeft = false;
                        leftLeaks = true;
                    }
                }
                else
                    continueLeft = false;
            }

            bool continueRight = true;
            bool rightLeaks = false;
            Point workingRightPoint = P;
            while (continueRight)
            {
                if (isObstructed(onMyRight(workingRightPoint)) == false)
                {
                    workingRightPoint = onMyRight(workingRightPoint);
                    toTheRight.Add(workingRightPoint);
                    if (isObstructed(onMyBottom(workingRightPoint)) == false)
                    {
                        continueRight = false;
                        rightLeaks = true;
                    }
                }
                else
                    continueRight = false;
            }

            if (leftLeaks == false && rightLeaks == false)
            {
                dctMap[P] = '~';
                toTheLeft.ForEach(r => dctMap[r] = '~');
                toTheRight.ForEach(r => dctMap[r] = '~');
                leQueue.Enqueue(onMyTop(P));
            }
            else
            {
                if (toTheLeft.Count > 0 && leftLeaks)
                    leQueue.Enqueue(toTheLeft.Last());
                if (toTheRight.Count > 0 && rightLeaks)
                    leQueue.Enqueue(toTheRight.Last());
                dctMap[P] = '|';
                toTheLeft.ForEach(r => dctMap[r] = '|');
                toTheRight.ForEach(r => dctMap[r] = '|');
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

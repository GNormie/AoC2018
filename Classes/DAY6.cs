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
    class DAY6
    {
        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(6));
            Problem1(linesInput);
            Problem2(linesInput);
        }

        public static void Problem1(string[] linesInput)
        {
            Dictionary<int, Point> dctPoint = new Dictionary<int, Point>();
            //part 1
            int dctPointID = 1;
            foreach (string line in linesInput)
            {
                int X = Convert.ToInt32(line.Split(',')[0]);
                int Y = Convert.ToInt32(line.Split(',')[1].Trim());
                dctPoint.Add(dctPointID, new Point(X, Y));
                dctPointID++;
            }

            var minX = dctPoint.Select(r => r.Value.X).Min();
            var minY = dctPoint.Select(r => r.Value.Y).Min();

            var maxX = dctPoint.Select(r => r.Value.X).Max();
            var maxY = dctPoint.Select(r => r.Value.Y).Max();

            int[,] fakeGrid = new int[maxX, maxY];

            Dictionary<int, int> dctCountPoint = new Dictionary<int, int>();

            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    Point CurrentPoint = new Point(i, j);
                    //part 1
                    var OrderedValues = dctPoint.Select(r => r).OrderBy(r => ManhattanDist(CurrentPoint, r.Value));
                    if (ManhattanDist(CurrentPoint, OrderedValues.First().Value) == ManhattanDist(CurrentPoint, OrderedValues.Skip(1).First().Value))
                    {
                        fakeGrid[i, j] = -1;
                        continue;
                    }
                    int KEY = dctPoint.Select(r => r).OrderBy(r => ManhattanDist(CurrentPoint, r.Value)).First().Key;
                    fakeGrid[i, j] = KEY;
                    if (fakeGrid[i, j] != 0 && dctCountPoint.ContainsKey(KEY) == false)
                    {
                        dctCountPoint.Add(KEY, 0);
                    }
                    dctCountPoint[KEY]++;
                }
            }

            Console.WriteLine("PART 1: " + dctCountPoint.OrderByDescending(r => r.Value).First().Value);
        }

        public static void Problem2(string[] linesInput)
        {
            Dictionary<int, Point> dctPoint = new Dictionary<int, Point>();

            int dctPointID = 1;
            foreach (string line in linesInput)
            {
                int X = Convert.ToInt32(line.Split(',')[0]);
                int Y = Convert.ToInt32(line.Split(',')[1].Trim());
                dctPoint.Add(dctPointID, new Point(X, Y));
                dctPointID++;
            }

            var minX = dctPoint.Select(r => r.Value.X).Min();
            var minY = dctPoint.Select(r => r.Value.Y).Min();

            var maxX = dctPoint.Select(r => r.Value.X).Max();
            var maxY = dctPoint.Select(r => r.Value.Y).Max();

            int[,] fakeGrid = new int[maxX, maxY];

            Dictionary<int, int> dctCountPoint = new Dictionary<int, int>();

            int validRegions = 0;

            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    Point CurrentPoint = new Point(i, j);
                    int ManhattanSum = 0;
                    foreach (var dctPointEntry in dctPoint)
                    {
                        ManhattanSum += ManhattanDist(CurrentPoint, dctPointEntry.Value);
                        if (ManhattanSum >= 10000)
                            break;
                    }
                    if (ManhattanSum < 10000)
                        validRegions++;
                }
            }

            Console.WriteLine("PART 2: " + validRegions);
        }

        public static int ManhattanDist(Point a, Point b)
        {
            //(x1 - x2) + (y1 - y2)?
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}

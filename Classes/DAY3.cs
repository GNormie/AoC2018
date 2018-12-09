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
    class DAY3
    {
        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(3));
            Problem1(linesInput);
            Problem2(linesInput);
        }

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
}

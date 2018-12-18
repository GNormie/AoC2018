using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY18
    {
        public static Dictionary<Point, char> dctMap = new Dictionary<Point, char>();
        public static Dictionary<Point, char> dctFutureMap = new Dictionary<Point, char>();
        public static Dictionary<string, int> dctRepetitions = new Dictionary<string, int>();

        static int width;
        static int height;

        static char openGround = '.';
        static char trees = '|';
        static char lumberyard = '#';

        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(18));

            width = linesInput[0].Length;
            height = linesInput.Count();

            int j = 0;
            foreach (string line in linesInput)
            {
                for (int w = 0; w < line.Count(); w++)
                {
                    dctMap.Add(new Point(w, j), line[w]);
                }
                j++;
            }

            int Minutes = 1000000000;
            for (int i = 0; i < Minutes; i++)
            {
                dctFutureMap.Clear();
                foreach (var element in dctMap)
                {
                    MorphLogic(element.Key);
                }
                var futureGen = new Dictionary<Point, char>(dctFutureMap);
                dctMap = futureGen;
                string uniqueDct = DctAsString();
                if (dctRepetitions.ContainsKey(uniqueDct) == false)
                    dctRepetitions.Add(uniqueDct, i);
                else
                {
                    int minInitial = dctRepetitions[uniqueDct];
                    int currRepetition = i;

                    Console.WriteLine("Repetition detected at: " + i);
                    Console.WriteLine("Originally found first at: " + minInitial);
                    Console.WriteLine("Cycles each: " + (i - minInitial));

                    //Detecting which repetition cycle corresponds to the target
                    //offset by -1, since 0 start index and we breakin'
                    for (int w = minInitial; w < i; w++)
                    {
                        if ((Minutes - w) % (i - minInitial) == 0)
                        {
                            var leValue = dctRepetitions.Single(R => R.Value == w - 1).Key.ToList();
                            Console.WriteLine("PART 2: " + leValue.Count(r => r == lumberyard) * leValue.Count(r => r == trees));
                        }
                    }
                    break;
                }
            }

            //offset by -1, since 0 start index
            int lumberValue = dctRepetitions.Single(R => R.Value == 9).Key.ToList().Count(r => r == lumberyard);
            int woodValue = dctRepetitions.Single(R => R.Value == 9).Key.ToList().Count(r => r == trees);

            Console.WriteLine("PART 1: " + (lumberValue * woodValue));
        }

        public static void MorphLogic(Point P)
        {
            //open ground (.)
            //trees (|)
            //lumberyard (#)
            var Surround = GetAllSurroundings(P);
            if (dctMap[P] == openGround)
            {
                if (Surround.Count(r => r == trees) >= 3)
                    dctFutureMap[P] = '|';
                else
                    dctFutureMap[P] = '.';
            }
            else if (dctMap[P] == trees)
            {
                if (Surround.Count(r => r == lumberyard) >= 3)
                    dctFutureMap[P] = '#';
                else
                    dctFutureMap[P] = '|';
            }
            else if (dctMap[P] == lumberyard)
            {
                if (Surround.Count(r => r == lumberyard) >= 1 && Surround.Count(r => r == trees) >= 1)
                    dctFutureMap[P] = '#';
                else
                    dctFutureMap[P] = '.';
            }
            else
            {
                Debug.Assert(false);
            }
        }

        public static List<char> GetAllSurroundings(Point P)
        {
            char p77 = ' ';
            char p88 = ' ';
            char p99 = ' ';
            char p44 = ' ';
            char p66 = ' ';
            char p11 = ' ';
            char p22 = ' ';
            char p33 = ' ';

            dctMap.TryGetValue(onMyLeftTopDiagonal(P), out p77);
            dctMap.TryGetValue(onMyTop(P), out p88);
            dctMap.TryGetValue(onMyRightTopDiagonal(P), out p99);
            dctMap.TryGetValue(onMyLeft(P), out p44);
            dctMap.TryGetValue(onMyRight(P), out p66);
            dctMap.TryGetValue(onMyLeftBottomDiagonal(P), out p11);
            dctMap.TryGetValue(onMyBottom(P), out p22);
            dctMap.TryGetValue(onMyRightBottomDiagonal(P), out p33);

            List<char> rtrnList = new List<char>();
            rtrnList.Add(p77);
            rtrnList.Add(p88);
            rtrnList.Add(p99);
            rtrnList.Add(p44);
            rtrnList.Add(p66);
            rtrnList.Add(p11);
            rtrnList.Add(p22);
            rtrnList.Add(p33);
            return rtrnList;
        }

        public static void PrintMap()
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
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

        public static string DctAsString()
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (dctMap.ContainsKey(new Point(i, j)))
                        sb.Append(dctMap[new Point(i, j)]);
                }
            }
            return sb.ToString();
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
        public static Point onMyLeftTopDiagonal(Point p)
        {
            return onMyLeft(onMyTop(p));
        }
        public static Point onMyRightTopDiagonal(Point p)
        {
            return onMyRight(onMyTop(p));
        }
        public static Point onMyLeftBottomDiagonal(Point p)
        {
            return onMyLeft(onMyBottom(p));
        }
        public static Point onMyRightBottomDiagonal(Point p)
        {
            return onMyRight(onMyBottom(p));
        }
    }
}

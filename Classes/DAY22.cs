using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY22
    {
        //my input
        public static int depth = 11820;
        public static Point target = new Point(7, 782);

        public static Dictionary<Point, Int64> dctErosionIndex = new Dictionary<Point, Int64>();
        public static Dictionary<Point, long> dctRiskLevel = new Dictionary<Point, long>();

        public static Dictionary<Int64, Int64> dctPastErosions = new Dictionary<long, long>();

        public static void Run()
        {
            dctErosionIndex.Add(new Point(0, 0), getErosionIndex(0));
            dctErosionIndex.Add(new Point(target.X, target.Y), getErosionIndex(0));

            dctRiskLevel.Add(new Point(0, 0), dctErosionIndex[new Point(target.X, target.Y)] % 3);
            dctRiskLevel.Add(new Point(target.X, target.Y), dctErosionIndex[new Point(target.X, target.Y)] % 3);

            for (int W = 0; W < Math.Min(target.X, target.Y) * 11; W++)
            {
                for (int i = W; i < target.X * 11; i++)
                {
                    if (dctErosionIndex.ContainsKey(new Point(i, W)) == false)
                    {
                        long leIndex = getGeoIndex(new Point(i, W));
                        dctErosionIndex.Add(new Point(i, W), getErosionIndex(leIndex));
                        if (dctRiskLevel.ContainsKey(new Point(i, W)) == false)
                        {
                            dctRiskLevel.Add(new Point(i, W), dctErosionIndex[new Point(i, W)] % 3);
                        }
                    }
                }

                for (int j = W; j < target.Y * 11; j++)
                {
                    if (dctErosionIndex.ContainsKey(new Point(W, j)) == false)
                    {
                        long leIndex = getGeoIndex(new Point(W, j));
                        dctErosionIndex.Add(new Point(W, j), getErosionIndex(leIndex));
                        if (dctRiskLevel.ContainsKey(new Point(W, j)) == false)
                        {
                            dctRiskLevel.Add(new Point(W, j), dctErosionIndex[new Point(W, j)] % 3);
                        }
                    }
                }
                //PrintGeoIndex();
            }

            long part1result = 0;
            for (int j = 0; j < target.Y + 1; j++)
            {
                for (int i = 0; i < target.X + 1; i++)
                {
                    part1result += dctRiskLevel[new Point(i, j)];
                }
            }
            Console.WriteLine(part1result);
            PrintGeoIndex();
            NeoPathfinding(new Point(0, 0), target);
            /*
            Dictionary<Point, PrevPoint> dctPoints = new Dictionary<Point, PrevPoint>();
            PathFinding(new Point(0, 0), dctPoints, 0);
            Console.WriteLine(dctPoints[new Point(target.X, target.Y)].costSoFar);
            */
            Console.ReadLine();
        }

        public static void NeoPathfinding(Point P, Point Goal)
        {
            Util.PriorityQueue<Point> frontier = new Util.PriorityQueue<Point>();
            frontier.Enqueue(P, 0);
            Dictionary<Point, Point> came_from = new Dictionary<Point, Point>();
            Dictionary<Point, int> cost_so_far = new Dictionary<Point, int>();
            Dictionary<Point, Tool> toolset = new Dictionary<Point, Tool>();
            //came_from.Add(new Point(P.X, P.Y), );
            cost_so_far.Add(new Point(P.X, P.Y), 0);
            toolset.Add(new Point(P.X, P.Y), Tool.Torch);

            while (frontier.Count > 0)
            {
                Point current = frontier.Dequeue();
                if (current == Goal)
                {
                    if (toolset[current] != Tool.Torch)
                        Console.WriteLine("PART 2 T: " + (cost_so_far[current] + 11));
                    else
                        Console.WriteLine("PART 2: " + (cost_so_far[current]));
                    break;
                }

                List<Point> chkPoints = new List<Point>();
                if (doesExist(onMyTop(current)))
                    chkPoints.Add(onMyTop(current));
                if (doesExist(onMyLeft(current)))
                    chkPoints.Add(onMyLeft(current));
                if (doesExist(onMyRight(current)))
                    chkPoints.Add(onMyRight(current));
                if (doesExist(onMyBottom(current)))
                    chkPoints.Add(onMyBottom(current));
                foreach (var next in chkPoints)
                {
                    int new_cost = cost_so_far[current] + costCalc(toolset[current], next);
                    if (cost_so_far.ContainsKey(next) == false || new_cost < cost_so_far[next])
                    {

                        cost_so_far[next] = new_cost;
                        int priority = new_cost + Util.ManhattanDist(Goal, next);
                        frontier.Enqueue(next, priority);
                        came_from[next] = current;
                        if (isObstructed(next, toolset[current]))
                            toolset[next] = switchTool(toolset[current], next);
                        else
                            toolset[next] = toolset[current];
                    }
                }
            }
        }

        public static int costCalc(Tool T, Point S)
        {
            if (isObstructed(S, T))
                return 6;
            else
                return 1;
        }

        public static Tool switchTool(Tool T, Point S)
        {
            if (dctRiskLevel[S] == 0) //ROCKY
            {
                if (T == Tool.None)
                    return Tool.Torch;
                else
                    return Tool.Torch;
            }
            else if (dctRiskLevel[S] == 1) //WET
            {
                if (T == Tool.Torch)
                    return Tool.ClimbingGear;
                else
                    return Tool.Torch;
            }
            else if (dctRiskLevel[S] == 2) //NARROW
            {
                if (T == Tool.ClimbingGear)
                    return Tool.Torch;
                else
                    return Tool.Torch;
            }
            else
                return Tool.Torch;
        }

        public enum Tool
        {
            None = 1,
            Torch = 0,
            ClimbingGear = 2
        }

        public static bool doesExist(Point P)
        {
            if (dctRiskLevel.ContainsKey(P))
                return true;
            else
                return false;
        }

        public static bool isObstructed(Point P, Tool T)
        {
            if (dctRiskLevel[P] == 0) //ROCKY
            {
                if (T == Tool.None)
                    return true;
                else
                    return false;
            }
            else if (dctRiskLevel[P] == 1) //WET
            {
                if (T == Tool.Torch)
                    return true;
                else
                    return false;
            }
            else if (dctRiskLevel[P] == 2) //NARROW
            {
                if (T == Tool.ClimbingGear)
                    return true;
                else
                    return false;
            }
            else
            {
                Debug.Assert(false);
                return true;
            }
        }


        public static Int64 getGeoIndex(Point P)
        {
            if (P == new Point(0, 0))
                return 0;
            else if (P == target)
                return 0;
            else if (P.Y == 0 && P.X != 0)
            {
                return P.X * 16807;
            }
            else if (P.X == 0 && P.Y != 0)
            {
                return P.Y * 48271;
            }
            else
            {
                if (dctErosionIndex.ContainsKey(onMyLeft(P)) && dctErosionIndex.ContainsKey(onMyTop(P)))
                {
                    return dctErosionIndex[onMyLeft(P)] * dctErosionIndex[onMyTop(P)];
                }
                Debug.Assert(false);
                return -1;
            }
        }

        public static long getErosionIndex(long geoIndex)
        {
            if (dctPastErosions.ContainsKey(geoIndex))
            {
                return dctPastErosions[geoIndex];
            }
            else
            {
                long leErosion = ((geoIndex + depth) % 20183);
                dctPastErosions.Add(geoIndex, leErosion);
                return leErosion;
            }
        }


        public static void PrintGeoIndex()
        {
            int minX = dctErosionIndex.Min(r => r.Key.X);
            int maxX = dctErosionIndex.Max(r => r.Key.X);
            int minY = dctErosionIndex.Min(r => r.Key.Y);
            int maxY = dctErosionIndex.Max(r => r.Key.Y);

            StringBuilder sb = new StringBuilder();
            for (int j = minY; j < maxY + 1; j++)
            {
                for (int i = minX; i < maxX + 1; i++)
                {
                    if (dctErosionIndex.ContainsKey(new Point(i, j)))
                    {
                        if (new Point(i, j) == target)
                            sb.Append('T');
                        else
                        {
                            if (dctErosionIndex[new Point(i, j)] % 3 == 0)
                            {
                                sb.Append('.');
                            }
                            if (dctErosionIndex[new Point(i, j)] % 3 == 1)
                            {
                                sb.Append('=');
                            }
                            if (dctErosionIndex[new Point(i, j)] % 3 == 2)
                            {
                                sb.Append('|');
                            }
                        }
                    }
                    else
                        sb.Append('?');
                }
                sb.Append(Environment.NewLine);
            }
            Util.WriteToFile(sb);
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

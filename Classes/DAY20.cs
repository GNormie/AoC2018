using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY20
    {
        public static Dictionary<Point, char> dctMap = new Dictionary<Point, char>();
        public static Point player = new Point(500, 500);

        public static Stack<char> parenthesisStack = new Stack<char>(); //Just for mental clarity's sake, not needed really
        public static Stack<Point> lastPositionStack = new Stack<Point>();

        public static void Run()
        {
            dctMap.Add(new Point(500, 500), '.');
            GetAllSurroundings(player);

            string linesInput = File.ReadAllLines(Util.ReadFromInputFolder(20))[0];

            foreach (char C in linesInput)
            {
                if (C == '(')
                {
                    parenthesisStack.Push('(');
                    lastPositionStack.Push(player);
                }
                else if (C == ')')
                {
                    parenthesisStack.Pop();
                    player = lastPositionStack.Pop();
                }
                else if (C == '|')
                {
                    player = lastPositionStack.Peek();
                }
                else
                {
                    Logic(C);
                    GetAllSurroundings(player);
                }
            }

            ClearUnknown();

            // Using DAY15 Pathfinding, since doors and rooms both count as open spaces we need to 
            // cut the total of cost in half
            Dictionary<Point, PrevPoint> dctPoints = new Dictionary<Point, PrevPoint>();
            PathFinding(new Point(500, 500), dctPoints, 0);
            var elementor = (dctPoints.OrderByDescending(r => r.Value.cost).First().Value.cost) / 2;
            Console.WriteLine("PART 1: " + elementor);

            // Unlike the maximum cost room, there might be multiple in this category and dividing by 2
            // could produce off by 1 errors so we will have to count them manually
            var prelimList = dctPoints.Where(r => r.Value.cost >= 2000).ToList();
            int properRoomCount = 0;
            foreach (var prelim in prelimList)
            {
                if (dctMap[prelim.Key] == '.')
                    properRoomCount++;
            }
            Console.WriteLine("PART 2: " + properRoomCount);
        }

        public static void Logic(char C)
        {
            if (C == 'N')
                GoNorth();
            else if (C == 'W')
                GoWest();
            else if (C == 'E')
                GoEast();
            else if (C == 'S')
                GoSouth();
        }

        public static void TryAdd(Point P, char C)
        {
            if (dctMap.ContainsKey(P) == false)
                dctMap.Add(P, C);
            else if (dctMap[P] == '?')
                dctMap[P] = C;
        }

        public static void GoNorth()
        {
            var leDoor = onMyTop(player);
            TryAdd(leDoor, '-');
            TryAdd(onMyLeft(leDoor), '#');
            TryAdd(onMyRight(leDoor), '#');
            var blankSpace = onMyTop(leDoor);
            TryAdd(blankSpace, '.');
            player = blankSpace;
        }

        public static void GoWest()
        {
            var leDoor = onMyLeft(player);
            TryAdd(leDoor, '|');
            TryAdd(onMyTop(leDoor), '#');
            TryAdd(onMyBottom(leDoor), '#');
            var blankSpace = onMyLeft(leDoor);
            TryAdd(blankSpace, '.');
            player = blankSpace;
        }

        public static void GoEast()
        {
            var leDoor = onMyRight(player);
            TryAdd(leDoor, '|');
            TryAdd(onMyTop(leDoor), '#');
            TryAdd(onMyBottom(leDoor), '#');
            var blankSpace = onMyRight(leDoor);
            TryAdd(blankSpace, '.');
            player = blankSpace;
        }

        public static void GoSouth()
        {
            var leDoor = onMyBottom(player);
            TryAdd(leDoor, '-');
            TryAdd(onMyLeft(leDoor), '#');
            TryAdd(onMyRight(leDoor), '#');
            var blankSpace = onMyBottom(leDoor);
            TryAdd(blankSpace, '.');
            player = blankSpace;
        }

        public static void GetAllSurroundings(Point P)
        {
            if (dctMap.ContainsKey(onMyTop(P)) == false)
                dctMap.Add(onMyTop(P), '?');
            if (dctMap.ContainsKey(onMyLeft(P)) == false)
                dctMap.Add(onMyLeft(P), '?');
            if (dctMap.ContainsKey(onMyRight(P)) == false)
                dctMap.Add(onMyRight(P), '?');
            if (dctMap.ContainsKey(onMyBottom(P)) == false)
                dctMap.Add(onMyBottom(P), '?');


            if (dctMap.ContainsKey(onMyLeftTopDiagonal(P)) == false)
                dctMap.Add(onMyLeftTopDiagonal(P), '?');
            if (dctMap.ContainsKey(onMyRightTopDiagonal(P)) == false)
                dctMap.Add(onMyRightTopDiagonal(P), '?');
            if (dctMap.ContainsKey(onMyLeftBottomDiagonal(P)) == false)
                dctMap.Add(onMyLeftBottomDiagonal(P), '?');
            if (dctMap.ContainsKey(onMyRightBottomDiagonal(P)) == false)
                dctMap.Add(onMyRightBottomDiagonal(P), '?');
        }

        public static void ClearUnknown()
        {
            foreach (var item in dctMap.Where(r => r.Value == '?').ToList())
            {
                dctMap[item.Key] = '#';
            }
        }

        public static void PrintMap()
        {
            int minX = dctMap.Min(r => r.Key.X);
            int maxX = dctMap.Max(r => r.Key.X);
            int minY = dctMap.Min(r => r.Key.Y);
            int maxY = dctMap.Max(r => r.Key.Y);

            StringBuilder sb = new StringBuilder();
            for (int j = minY; j < maxY + 1; j++)
            {
                for (int i = minX; i < maxX + 1; i++)
                {
                    if (dctMap.ContainsKey(new Point(i, j)))
                        sb.Append(dctMap[new Point(i, j)]);
                    else
                        sb.Append(' ');
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

        public static Dictionary<Point, PrevPoint> PathFinding(Point myPosition, Dictionary<Point, PrevPoint> dctPoints, int cost)
        {
            Queue<Point> thisRevision = new Queue<Point>();
            thisRevision.Enqueue(myPosition);
            while (thisRevision.Any())
            {
                var qPoint = thisRevision.Dequeue();
                Point TOP = onMyTop(qPoint);
                Point LEFT = onMyLeft(qPoint);
                Point RIGHT = onMyRight(qPoint);
                Point BOTTOM = onMyBottom(qPoint);
                PrevPoint prevPoint;
                if (dctPoints.ContainsKey(qPoint))
                {
                    prevPoint.cost = dctPoints[qPoint].cost + 1;
                    prevPoint.prevPoint = qPoint;
                }
                else
                {
                    prevPoint.cost = 1;
                    prevPoint.prevPoint = qPoint;
                }

                if (isNotObstructed(TOP) && !dctPoints.ContainsKey(TOP))
                {
                    dctPoints.Add(TOP, prevPoint);
                    thisRevision.Enqueue(TOP);
                }
                if (isNotObstructed(LEFT) && !dctPoints.ContainsKey(LEFT))
                {
                    dctPoints.Add(LEFT, prevPoint);
                    thisRevision.Enqueue(LEFT);
                }
                if (isNotObstructed(RIGHT) && !dctPoints.ContainsKey(RIGHT))
                {
                    dctPoints.Add(RIGHT, prevPoint);
                    thisRevision.Enqueue(RIGHT);
                }
                if (isNotObstructed(BOTTOM) && !dctPoints.ContainsKey(BOTTOM))
                {
                    dctPoints.Add(BOTTOM, prevPoint);
                    thisRevision.Enqueue(BOTTOM);
                }
            }
            return dctPoints;
        }

        public static bool isNotObstructed(Point P)
        {
            if (dctMap[P] == '#')
                return false;
            else
                return true;
        }

        public struct PrevPoint
        {
            public Point prevPoint;
            public int cost;
        }
    }
}

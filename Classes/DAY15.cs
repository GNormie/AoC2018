using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing;
using System.Text;
using System.IO;

namespace AoC2018
{
    public static class DAY15
    {
        static List<Unit> lstUnits = new List<Unit>();
        static bool FinishGame = false;
        static char[,] grid;

        public static void Run()
        {
            int basePower = 3;
            while (SimulateBattle(basePower) == false)
            {
                basePower++;
            }
        }

        public static bool SimulateBattle(int elvenPower = 3)
        {
            lstUnits = new List<Unit>();
            FinishGame = false;

            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(15));
            int Height = linesInput.Count();
            int Width = linesInput.OrderByDescending(r => r.Length).First().Length;

            grid = new char[Width, Height];

            int j = 0;
            int ID = 1;
            foreach (string line in linesInput)
            {
                for (int w = 0; w < line.Count(); w++)
                {
                    if (line[w] == 'G' || line[w] == 'E')
                    {
                        grid[w, j] = '.';
                        if (line[w] == 'G')
                        {
                            lstUnits.Add(new Unit(ID, new Point(w, j), Faction.Goblin));
                        }
                        else
                        {
                            lstUnits.Add(new Unit(ID, new Point(w, j), Faction.Elf, elvenPower));
                        }
                        ID++;
                    }
                    else
                        grid[w, j] = line[w];
                }
                j++;
            }

            int originalAmountOfElves = lstUnits.Where(r => r.faction == Faction.Elf).Count();

            int Round = 0;
            PrintMap(Width, Height, grid);
            while (FinishGame == false)
            {
                var orderedUnits = lstUnits.OrderBy(r => r.Position.Y).ThenBy(r => r.Position.X);
                foreach (Unit soldier in orderedUnits)
                {
                    if (soldier.HP <= 0)
                        continue;
                    Logic(soldier);
                    if (FinishGame == true)
                        break;
                }
                PrintMap(Width, Height, grid);
                if (FinishGame == false)
                    Round++;
            }

            if (lstUnits.Where(r => r.faction == Faction.Elf).Count() == originalAmountOfElves)
            {
                Console.WriteLine("TRUE: " + lstUnits.Sum(r => r.HP) * Round + " | ElvenPower=" + elvenPower);
                return true;
            }
            else
            {
                Console.WriteLine("FALSE: " + lstUnits.Sum(r => r.HP) * Round + " | ElvenPower=" + elvenPower);
                return false;
            }
        }

        public class Unit
        {
            public Point Position;

            public int ID = 0;

            public Faction faction = Faction.Elf;
            public int HP = 200;
            public int attackPower = 3;

            public Point onMyLeft
            {
                get { return new Point(this.Position.X - 1, this.Position.Y); }
            }
            public Point onMyRight
            {
                get { return new Point(this.Position.X + 1, this.Position.Y); }
            }
            public Point onMyTop
            {
                get { return new Point(this.Position.X, this.Position.Y - 1); }
            }
            public Point onMyBottom
            {
                get { return new Point(this.Position.X, this.Position.Y + 1); }
            }

            public static bool isObstructed(Point P)
            {
                if (P.X < 0 || P.X >= grid.GetLength(0) || P.Y < 0 || P.Y >= grid.GetLength(1))
                    return false;
                else if (grid[P.X, P.Y] != '.' || lstUnits.Any(r => r.Position == P))
                    return true;
                else
                    return false;
            }

            public Faction enemyFaction
            {
                get
                {
                    if (this.faction == Faction.Goblin)
                        return Faction.Elf;
                    else
                        return Faction.Goblin;
                }
            }

            public Unit(int _id, Point point, Faction _faction)
            {
                ID = _id;
                Position = point;
                faction = _faction;
            }

            public Unit(int _id, Point point, Faction _faction, int _attackPower)
            {
                ID = _id;
                Position = point;
                faction = _faction;
                attackPower = _attackPower;
            }

            public void Attack(Unit enemy)
            {
                enemy.HP = enemy.HP - attackPower;
                if (enemy.HP <= 0)
                    lstUnits.Remove(enemy);
            }
        }


        public static void Logic(Unit soldier)
        {
            bool didAttacked = AttackLogic(soldier);

            if (didAttacked || FinishGame)
                return;

            //CHECK SURROUNDED BY OBSTACLES
            if (Unit.isObstructed(soldier.onMyTop))
                if (Unit.isObstructed(soldier.onMyLeft))
                    if (Unit.isObstructed(soldier.onMyRight))
                        if (Unit.isObstructed(soldier.onMyBottom))
                            return;

            //Computes distance to all points
            Dictionary<Point, PrevPoint> dctPoints = new Dictionary<Point, PrevPoint>();
            PathFinding(soldier.Position, dctPoints, 0);
            Point movePoint;
            if (GetTargets(soldier, dctPoints, out movePoint))
            {
                soldier.Position = movePoint;
            }
            //Can attack after moving
            didAttacked = AttackLogic(soldier);
        }

        public static bool GetTargets(Unit soldier, Dictionary<Point, PrevPoint> dctPoints, out Point newPosition)
        {
            bool gotOne = false;
            newPosition = Point.Empty;
            Dictionary<Point, int> potentialTargets = new Dictionary<Point, int>();
            int minCost = 99999999;

            var possibleTargets = lstUnits.Where(r => r.faction == soldier.enemyFaction);
            if (possibleTargets.Count() == 0)
            {
                FinishGame = true;
                return false;
            }
            foreach (var target in possibleTargets)
            {
                List<Point> localPoints = new List<Point>();
                localPoints.Add(onMyTop(target.Position));
                localPoints.Add(onMyLeft(target.Position));
                localPoints.Add(onMyRight(target.Position));
                localPoints.Add(onMyBottom(target.Position));
                foreach (var point in localPoints)
                {
                    if (dctPoints.ContainsKey(point))
                    {
                        if (gotOne == false)
                        {
                            potentialTargets.Add(point, dctPoints[point].cost);
                            gotOne = true;
                            minCost = dctPoints[point].cost;
                        }
                        else
                        {
                            if (dctPoints[point].cost <= minCost)
                            {
                                if (potentialTargets.ContainsKey(point) == false)
                                {
                                    potentialTargets.Add(point, dctPoints[point].cost);
                                    minCost = dctPoints[point].cost;
                                }
                            }
                        }
                    }
                }
            }

            if (potentialTargets.Count == 0)
                return false;
            else
            {
                Point lastTarget;
                if (potentialTargets.Where(r => r.Value == minCost).Count() == 1)
                    lastTarget = potentialTargets.Where(r => r.Value == minCost).First().Key;
                else
                {
                    lastTarget = potentialTargets.Where(r => r.Value == minCost).OrderBy(r => r.Key.Y).ThenBy(w => w.Key.X).First().Key;
                }
                var dissectTarget = dctPoints[lastTarget];
                if (dissectTarget.cost == 1)
                {
                    newPosition = dctPoints.Single(r => r.Key == lastTarget).Key;
                    return true;
                }
                while (dissectTarget.cost != 2)
                {
                    dissectTarget = dctPoints[dissectTarget.prevPoint];
                }
                newPosition = dctPoints.Single(r => r.Key == dissectTarget.prevPoint).Key;
                return true;
            }

        }

        public static Dictionary<Point, PrevPoint> PathFinding(Point punto, Dictionary<Point, PrevPoint> dctPoints, int cost)
        {
            Queue<Point> thisRevision = new Queue<Point>();
            thisRevision.Enqueue(punto);
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


                if (!Unit.isObstructed(TOP) && !dctPoints.ContainsKey(TOP))
                {
                    dctPoints.Add(TOP, prevPoint);
                    thisRevision.Enqueue(TOP);
                }
                if (!Unit.isObstructed(LEFT) && !dctPoints.ContainsKey(LEFT))
                {
                    dctPoints.Add(LEFT, prevPoint);
                    thisRevision.Enqueue(LEFT);
                }
                if (!Unit.isObstructed(RIGHT) && !dctPoints.ContainsKey(RIGHT))
                {
                    dctPoints.Add(RIGHT, prevPoint);
                    thisRevision.Enqueue(RIGHT);
                }
                if (!Unit.isObstructed(BOTTOM) && !dctPoints.ContainsKey(BOTTOM))
                {
                    dctPoints.Add(BOTTOM, prevPoint);
                    thisRevision.Enqueue(BOTTOM);
                }
            }
            return dctPoints;
        }

        public struct PrevPoint
        {
            public Point prevPoint;
            public int cost;
        }

        public static bool AttackLogic(Unit soldier)
        {
            //TRY ATTACK
            bool didAttacked = false;
            List<Unit> possibleAttackTargets = new List<Unit>();
            if (lstUnits.Any(r => r.Position == soldier.onMyTop && r.faction == soldier.enemyFaction))
                possibleAttackTargets.Add(lstUnits.Single(r => r.Position == soldier.onMyTop && r.faction == soldier.enemyFaction));
            if (lstUnits.Any(r => r.Position == soldier.onMyLeft && r.faction == soldier.enemyFaction))
                possibleAttackTargets.Add(lstUnits.Single(r => r.Position == soldier.onMyLeft && r.faction == soldier.enemyFaction));
            if (lstUnits.Any(r => r.Position == soldier.onMyRight && r.faction == soldier.enemyFaction))
                possibleAttackTargets.Add(lstUnits.Single(r => r.Position == soldier.onMyRight && r.faction == soldier.enemyFaction));
            if (lstUnits.Any(r => r.Position == soldier.onMyBottom && r.faction == soldier.enemyFaction))
                possibleAttackTargets.Add(lstUnits.Single(r => r.Position == soldier.onMyBottom && r.faction == soldier.enemyFaction));
            if (possibleAttackTargets.Count > 0)
            {
                var attackTarget = possibleAttackTargets.OrderBy(r => r.HP).ThenBy(w => w.Position.Y).ThenBy(w => w.Position.X).First();
                soldier.Attack(attackTarget);
                didAttacked = true;
            }
            return didAttacked;
        }
        public static void PrintMap(int width, int height, char[,] map)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Unit unit = lstUnits.SingleOrDefault(r => r.Position.X == i && r.Position.Y == j);
                    if (unit != null)
                    {
                        if (unit.faction == Faction.Elf)
                            sb.Append(unit.ID);//sb.Append((char)'E');
                        else
                            sb.Append(unit.ID);//sb.Append((char)'G');
                    }

                    else
                        sb.Append(map[i, j]);
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
        public static Point onMyTop(Point p)
        {
            return new Point(p.X, p.Y - 1);
        }
        public static Point onMyBottom(Point p)
        {
            return new Point(p.X, p.Y + 1);
        }

        public static int ManhattanDist(Unit a, Unit b)
        {
            //(x1 - x2) + (y1 - y2)?
            return Math.Abs(a.Position.X - b.Position.X) + Math.Abs(a.Position.Y - b.Position.Y);
        }

        public static int ManhattanDist(Point a, Point b)
        {
            //(x1 - x2) + (y1 - y2)?
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        public enum Faction
        {
            Goblin = 1,
            Elf = 2
        }
    }
}
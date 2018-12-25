using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY25
    {
        public static List<List<fPoint>> constellations = new List<List<fPoint>>();
        public static List<fPoint> stars = new List<fPoint>();

        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(25));
            foreach (string line in linesInput)
            {
                string[] pPoint = line.Split(',');
                long pX = Convert.ToInt64(pPoint[0].Trim());
                long pY = Convert.ToInt64(pPoint[1].Trim());
                long pZ = Convert.ToInt64(pPoint[2].Trim());
                long pA = Convert.ToInt64(pPoint[3].Trim());
                stars.Add(new fPoint(pX, pY, pZ, pA));
            }

            while (stars.Count() > 0)
            {
                var leStar = stars.First();
                if (constellations.Count == 0)
                {
                    constellations.Add(new List<fPoint>());
                    constellations.First().Add(leStar);
                }
                else
                {
                    var starGroup = constellations.Where(r => r.Any(wr => ManhattanDist(leStar, wr) <= 3));
                    if (starGroup.Count() > 0)
                    {
                        if (starGroup.Count() > 1)
                        {
                            var leJoinFactor = starGroup.First();
                            foreach (var littleStar in starGroup.Skip(1))
                            {
                                leJoinFactor.AddRange(littleStar);
                                littleStar.Clear();
                            }
                            leJoinFactor.Add(leStar);
                        }
                        else
                            starGroup.Last().Add(leStar);
                    }
                    else
                    {
                        constellations.Add(new List<fPoint>());
                        constellations.Last().Add(leStar);
                    }
                }
                stars.RemoveAt(0);
            }

            Console.WriteLine("PART 1: "+constellations.Where(r => r.Count() > 0).Count());
        }

        public struct fPoint
        {
            public fPoint(long _X, long _Y, long _Z, long _A)
            {
                X = _X;
                Y = _Y;
                Z = _Z;
                A = _A;
            }
            public long X { get; set; }
            public long Y { get; set; }
            public long Z { get; set; }
            public long A { get; set; }
        }

        public static long ManhattanDist(fPoint a, fPoint b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z) + Math.Abs(a.A - b.A);
        }
    }
}

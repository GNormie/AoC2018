using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY23
    {
        public static Dictionary<fPoint, long> dctNanoBots = new Dictionary<fPoint, long>();
        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(23));
            foreach (string line in linesInput)
            {
                string[] posCoords = line.Between("<", ">").Split(',');
                long signalStrength = Convert.ToInt64(line.Between("r=").Trim());
                long xPoint = Convert.ToInt64(posCoords[0]);
                long yPoint = Convert.ToInt64(posCoords[1]);
                long zPoint = Convert.ToInt64(posCoords[2]);
                fPoint BotPosition = new fPoint(xPoint, yPoint, zPoint);
                dctNanoBots.Add(BotPosition, signalStrength);
            }

            var strongestNano = dctNanoBots.OrderByDescending(r => r.Value).First();
            Console.WriteLine("Part 1: " + dctNanoBots.Where(r => ManhattanDist(r.Key, strongestNano.Key) <= strongestNano.Value).Count());
            Console.WriteLine("Loading part 2, should take 1 minute");

            // Reduces the lookout on the points to make complexity manageable
            // Finds best match for this small region
            // Amplifies the result to set it up for the next factor
            fPoint lePoint = new fPoint();
            for (int factor = 10000000; factor >= 1; factor /= 10)
            {
                lePoint = OperateDict(reduceDict(factor), lePoint);
                lePoint.X *= 10;
                lePoint.Y *= 10;
                lePoint.Z *= 10;
            }
            string answer = ManhattanDist(new fPoint(0, 0, 0), lePoint).ToString();
            answer = answer.Remove(answer.Length - 1); //remove last character for the last extra computation
            Console.WriteLine("Part 2: " + answer);
        }

        public static Dictionary<fPoint, long> reduceDict(int factor)
        {
            Dictionary<fPoint, long> dctReturn = new Dictionary<fPoint, long>();
            foreach (var bot in dctNanoBots)
            {
                long newCordX = bot.Key.X / factor;
                long newCordY = bot.Key.Y / factor;
                long newCordZ = bot.Key.Z / factor;
                long val = bot.Value / factor;
                fPoint simplePoint = new fPoint(newCordX, newCordY, newCordZ);
                if (dctReturn.ContainsKey(simplePoint) == false)
                    dctReturn.Add(simplePoint, val);
            }
            return dctReturn;
        }

        public static fPoint OperateDict(Dictionary<fPoint, long> dct, fPoint aproxPoint)
        {
            long minX = aproxPoint.X + -20;
            long minY = aproxPoint.Y + -20;
            long minZ = aproxPoint.Z + -20;
            long maxX = aproxPoint.X + (20);
            long maxY = aproxPoint.Y + (20);
            long maxZ = aproxPoint.Z + (20);

            long maxBois = 0;
            fPoint lePoint = new fPoint(0, 0, 0);

            for (long i = minX; i < maxX + 1; i++)
                for (long j = minY; j < maxY + 1; j++)
                    for (long k = minZ; k < maxZ + 1; k++)
                    {
                        int currBois = dct.Where(r => ManhattanDist(r.Key, new fPoint(i, j, k)) <= r.Value).Count();
                        if (currBois > maxBois)
                        {
                            maxBois = currBois;
                            lePoint = new fPoint(i, j, k);
                        }
                    }
            return lePoint;
        }

        public struct fPoint
        {
            public fPoint(long _X, long _Y, long _Z)
            {
                X = _X;
                Y = _Y;
                Z = _Z;
            }
            public long X { get; set; }
            public long Y { get; set; }
            public long Z { get; set; }
        }

        public static long ManhattanDist(fPoint a, fPoint b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
        }
    }
}

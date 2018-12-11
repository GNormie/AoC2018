using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY10
    {
        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(10));

            List<SkyLight> lstLights = new List<SkyLight>();

            Point maxPoint = new Point(999999, 999999);

            foreach (string line in linesInput)
            {
                var rawInfoPosition = line.Between("tion=<", "> velo");
                string[] positionArray = rawInfoPosition.Split(',');
                var rawInfoSpeed = line.Between("velocity=<", ">");
                string[] speedArray = rawInfoSpeed.Split(',');

                Point startPosition = new Point(Convert.ToInt32(positionArray[0].Trim()), Convert.ToInt32(positionArray[1].Trim()));
                Point speed = new Point(Convert.ToInt32(speedArray[0].Trim()), Convert.ToInt32(speedArray[1].Trim()));

                lstLights.Add(new SkyLight(startPosition, speed));
            }

            int seconds = 0;
            while (true)
            {
                foreach (SkyLight light in lstLights)
                    light.Tick();

                int currentMaxX = lstLights.Max(r => r.position.X);
                int currentMaxY = lstLights.Max(r => r.position.Y);

                if (currentMaxX < maxPoint.X && currentMaxY < maxPoint.Y)
                    maxPoint = new Point(currentMaxX, currentMaxY);
                else
                {
                    Console.WriteLine(seconds);
                    break;
                }
                seconds++;
                /*
                if (lstLights.GroupBy(r => r.position.X).Where(w => w.Count() >= 10).Count() >= 10)
                {
                    leCondition = false;
                    Console.WriteLine(seconds);
                }*/
            }

            foreach (SkyLight light in lstLights)
            {
                light.TickBackwards();
            }

            int minX = lstLights.Min(r => r.position.X);
            int minY = lstLights.Min(r => r.position.Y);
            int maxX = lstLights.Max(r => r.position.X);
            int maxY = lstLights.Max(r => r.position.Y);

            StringBuilder sb = new StringBuilder();
            for (int j = minY; j < maxY + 1; j++)
            {
                sb.Clear();
                for (int i = minX; i < maxX + 1; i++)
                {
                    if (lstLights.Where(r => r.position.X == i && r.position.Y == j).Count() > 0)
                        sb.Append('#');
                    else
                        sb.Append('.');
                }
                Console.WriteLine(sb.ToString());
            }
        }

        public class SkyLight
        {
            public Point position = new Point(0, 0);
            public Point speed = new Point(0, 0);

            public SkyLight(Point pos, Point spd)
            {
                position = pos;
                speed = spd;
            }

            public void Tick()
            {
                position.X = position.X + speed.X;
                position.Y = position.Y + speed.Y;
            }

            public void TickBackwards()
            {
                position.X = position.X - speed.X;
                position.Y = position.Y - speed.Y;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY11
    {
        public static int GridSerialNumber = 9110; //The input
        public static int gridSize = 300;
        public static int[,] powerGrid = new int[gridSize, gridSize];

        public static int negativesCount = 0;

        public static int baseGridTotalPower = 0;
        public static EnergyReading singlePowerMax = new EnergyReading(0, 1, new Point(0, 0));

        public static Dictionary<Point, int> energyCuadrants = new Dictionary<Point, int>();
        public static Dictionary<int, EnergyReading> maxEnergyCuadrants = new Dictionary<int, EnergyReading>();

        public static void Run(int input = 9110)
        {
            GridSerialNumber = input;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    int basePowerLevel = GetPowerCellLevel(i, j);
                    powerGrid[i, j] = basePowerLevel;
                    baseGridTotalPower += basePowerLevel;
                    if (basePowerLevel > singlePowerMax.totalPower)
                    {
                        singlePowerMax.coordinates = new Point(i + 1, j + 1);
                        singlePowerMax.totalPower = basePowerLevel;
                    }
                }
            }

            //part 1
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    getCuadrantEnergy(powerGrid, i, j, 3);
                }
            }
            Console.WriteLine(energyCuadrants.OrderByDescending(r => r.Value).First());
            energyCuadrants.Clear();

            //part 2
            int MaxEnergyMinSize = (from int v in powerGrid select v).Max();
            int MaxEnergyMaxSize = baseGridTotalPower;

            maxEnergyCuadrants.Add(1, singlePowerMax);
            maxEnergyCuadrants.Add(300, new EnergyReading(MaxEnergyMaxSize, 300, new Point(0, 0)));

            for (int r = 1; r < gridSize - 1; r++)
            {
                energyCuadrants.Clear();
                for (int i = 0; i < gridSize; i++)
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        getCuadrantEnergy(powerGrid, i, j, r + 1);
                    }
                }
                var currentTopEnergy = energyCuadrants.OrderByDescending(wr => wr.Value).First();
                if (currentTopEnergy.Value < 0)
                    negativesCount++;
                else
                    negativesCount = 0;
                if (negativesCount >= 5)
                    break;
                maxEnergyCuadrants.Add(r + 1, new EnergyReading(currentTopEnergy.Value, r + 1, currentTopEnergy.Key));
            }

            var topEnergy = maxEnergyCuadrants.OrderByDescending(wr => wr.Value.totalPower).First();
            Console.WriteLine(topEnergy.Value.coordinates.X + "," + topEnergy.Value.coordinates.Y + "," + topEnergy.Key);
            Console.WriteLine(topEnergy.Value.totalPower);
            Console.ReadLine();
        }

        public static int GetPowerCellLevel(int X, int Y)
        {
            int powerLevel = 0;
            X++; Y++;
            int RackID = X + 10;
            powerLevel = (RackID * Y);
            powerLevel += GridSerialNumber;
            powerLevel = powerLevel * RackID;
            int finalValue = digitCent(powerLevel);
            return (finalValue - 5);
        }

        public static int digitCent(int val)
        {
            //(10250 / 100) % 10 = 2
            return (int)(val / 100D) % 10;
        }

        public static void getCuadrantEnergy(int[,] powerGrid, int X, int Y, int size)
        {
            if (X > (gridSize - size) || Y > gridSize - size)
                return;

            int totalPower = 0;

            for (int i = X; i < X + size; i++)
            {
                for (int j = Y; j < Y + size; j++)
                {
                    totalPower += powerGrid[i, j];
                }
            }
            energyCuadrants.Add(new Point((X + 1), (Y + 1)), totalPower);
        }

        public class EnergyReading
        {
            public int totalPower = 0;
            public int size = 0;
            public Point coordinates;

            public EnergyReading(int _totalPower, int _size, Point _coordinates)
            {
                totalPower = _totalPower;
                size = _size;
                coordinates = _coordinates;
            }
        }
    }
}

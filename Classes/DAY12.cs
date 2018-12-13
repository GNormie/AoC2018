using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY12
    {
        public static void Run()
        {
            StringBuilder sb = new StringBuilder();
            List<string> linesInput = File.ReadAllLines(Util.ReadFromInputFolder(12)).ToList();
            string baseGeneration = linesInput[0].Between("initial state: ");

            //Prepare initial state
            int potCounter = 0;
            List<PotPlant> lstPots = new List<PotPlant>();
            foreach (char A in baseGeneration)
            {
                lstPots.Add(new PotPlant(potCounter, A));
                potCounter++;
            }

            //Get rid of consumed lines
            linesInput.RemoveAt(0);
            linesInput.RemoveAt(0);

            Dictionary<string, string> producePlantsCombo = new Dictionary<string, string>();
            Dictionary<string, string> killPlantsCombo = new Dictionary<string, string>();

            LinkedList<long> Differences = new LinkedList<long>();

            long mysteryConstant = 0;

            //Get sprout n wither combinations
            foreach (string line in linesInput)
            {
                //#.... => .
                string[] initialInput = line.Split(' ');
                string potCombi = initialInput[0]; //Pot Combination
                string producesPlant = initialInput[2]; //Produces plant or not
                if (producesPlant == "#")
                {
                    producePlantsCombo.Add(potCombi, potCombi);
                }
                else
                {
                    killPlantsCombo.Add(potCombi, potCombi);
                }
            }

            LinkedList<PotPlant> llBaseGeneration = new LinkedList<PotPlant>(lstPots);

            var firstElement = llBaseGeneration.First;

            //Extra room left n' right
            for (int i = -20; i < 0; i++)
            {
                llBaseGeneration.AddBefore(firstElement, new PotPlant(i));
            }
            int lastPotNumber = llBaseGeneration.Last().potNumber;
            for (int i = lastPotNumber + 1; i < lastPotNumber + 1000; i++)
            {
                llBaseGeneration.AddLast(new PotPlant(i));
            }

            for (long Gens = 1; Gens < 501; Gens++)
            {
                var lastGeneration = llBaseGeneration;
                LinkedList<PotPlant> upComingGeneration = new LinkedList<PotPlant>();
                var currentNode = llBaseGeneration.First;
                for (int i = 0; i < llBaseGeneration.Count; i++)
                {
                    string plantConfig = ConstructPotStructure(currentNode);
                    if (producePlantsCombo.ContainsKey(plantConfig))
                    {
                        PotPlant futurePlant = new PotPlant(currentNode.Value.potNumber, '#');
                        upComingGeneration.AddLast(futurePlant);
                    }
                    else
                    {
                        PotPlant futurePlant = new PotPlant(currentNode.Value.potNumber, '.');
                        upComingGeneration.AddLast(futurePlant);
                    }
                    currentNode = currentNode.Next;

                }
                long prevDif = upComingGeneration.Where(r => r.plant == '#').Sum(w => w.potNumber) - llBaseGeneration.Where(r => r.plant == '#').Sum(w => w.potNumber);
                Differences.AddLast(prevDif);
                if (Differences.Any(r => r != prevDif))
                    Differences.Clear();
                else
                {
                    // After some point, the output seems displaced on a constant basis forever
                    // Mystery = TOTAL - (sameDif * Gen)
                    // so
                    // TOTAL = Mystery + (sameDif * Gen)
                    if (Differences.Count >= 4)
                    {
                        long total = upComingGeneration.Where(r => r.plant == '#').Sum(w => w.potNumber);
                        mysteryConstant = total - (Differences.First() * Gens);
                        long part2Result = mysteryConstant + (Differences.First() * 50000000000);
                        Console.WriteLine("part 2: " + part2Result);
                        break;
                    }
                }
                llBaseGeneration = upComingGeneration;
                if (Gens == 20)
                    Console.WriteLine("part 1: " + llBaseGeneration.Where(r => r.plant == '#').Sum(w => w.potNumber));
            }
            Console.ReadLine();
        }

        static string ConstructPotStructure(LinkedListNode<PotPlant> currentNode)
        {
            char[] potConfig = new char[5];
            potConfig[0] = currentNode.Previous == null || currentNode.Previous.Previous == null ? '.' : currentNode.Previous.Previous.Value.plant;
            potConfig[1] = currentNode.Previous == null ? '.' : currentNode.Previous.Value.plant;
            potConfig[2] = currentNode.Value.plant;
            potConfig[3] = currentNode.Next == null ? '.' : currentNode.Next.Value.plant;
            potConfig[4] = currentNode.Next == null || currentNode.Next.Next == null ? '.' : currentNode.Next.Next.Value.plant;
            return new string(potConfig);
        }

        //Gen output code
        static string PrintGen(LinkedList<PotPlant> pots, long gen, StringBuilder sb)
        {
            sb.Clear();
            sb.Append("GEN: " + gen + " |SUM: " + pots.Where(r => r.plant == '#').Sum(w => w.potNumber) + "| > ");
            foreach (PotPlant plant in pots)
            {
                sb.Append(plant.plant);
            }
            return sb.ToString();
        }

        public class PotPlant
        {
            public char plant = '.';
            public int potNumber = 0;

            public PotPlant(int _potNumber)
            {
                potNumber = _potNumber;
            }

            public PotPlant(int _potNumber, char _plant)
            {
                potNumber = _potNumber;
                plant = _plant;
            }
        }
    }
}

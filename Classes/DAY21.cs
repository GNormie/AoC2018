using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY21
    {
        public static Dictionary<int, Tuple<string, int[]>> dctInstructions = new Dictionary<int, Tuple<string, int[]>>();
        public static int[] baseRegister = new int[6];

        public static void Run()
        {
            List<string> linesInput = File.ReadAllLines(Util.ReadFromInputFolder(21)).ToList();

            int controlPointer = Convert.ToInt32(linesInput.First().Between("#ip "));
            linesInput.RemoveAt(0);
            int instructionPosition = 0;
            foreach (string line in linesInput)
            {
                string[] lineInstruc = line.Split(' ');
                Tuple<string, int[]> instruct = new Tuple<string, int[]>(lineInstruc[0], new int[6]);
                instruct.Item2[1] = Convert.ToInt32(lineInstruc[1]);
                instruct.Item2[2] = Convert.ToInt32(lineInstruc[2]);
                instruct.Item2[3] = Convert.ToInt32(lineInstruc[3]);
                dctInstructions.Add(instructionPosition, instruct);
                instructionPosition++;
            }

            Dictionary<int, int> uniqueValues = new Dictionary<int, int>();
            bool printPartOne = false;

            int indexToWatch = 0;
            var keyInstruction = dctInstructions[28];
            if (keyInstruction.Item1 == "eqrr" && (keyInstruction.Item2[1] == 0 || keyInstruction.Item2[2] == 0))
            {
                indexToWatch = Math.Max(keyInstruction.Item2[1], keyInstruction.Item2[2]);
            }

            Console.WriteLine("Computing both parts, part 2 takes a while");

            while (true)
            {
                if (baseRegister[controlPointer] == 28)
                {
                    if (printPartOne == false)
                    {
                        Console.WriteLine("PART 1: " + baseRegister[indexToWatch]);
                        printPartOne = true;
                    }
                    if (uniqueValues.ContainsKey(baseRegister[indexToWatch]) == false)
                        uniqueValues.Add(baseRegister[indexToWatch], baseRegister[indexToWatch]);
                    else
                    {
                        Console.WriteLine("PART 2: " + uniqueValues.Last().Key);
                        break;
                    }
                }

                var currRegister = dctInstructions[baseRegister[controlPointer]];
                ExecuteInstruction(currRegister.Item1, currRegister.Item2);
                baseRegister[controlPointer] = (baseRegister[controlPointer] + 1);
            }

            Console.ReadLine();
        }

        public static void ExecuteInstruction(string opCode, int[] instruction)
        {
            if (opCode == "addi")
                baseRegister = DAY16.AsmEmu.addi(baseRegister, instruction);
            else if (opCode == "addr")
                baseRegister = DAY16.AsmEmu.addr(baseRegister, instruction);
            else if (opCode == "bani")
                baseRegister = DAY16.AsmEmu.bani(baseRegister, instruction);
            else if (opCode == "banr")
                baseRegister = DAY16.AsmEmu.banr(baseRegister, instruction);
            else if (opCode == "bori")
                baseRegister = DAY16.AsmEmu.bori(baseRegister, instruction);
            else if (opCode == "borr")
                baseRegister = DAY16.AsmEmu.borr(baseRegister, instruction);
            else if (opCode == "eqir")
                baseRegister = DAY16.AsmEmu.eqir(baseRegister, instruction);
            else if (opCode == "eqri")
                baseRegister = DAY16.AsmEmu.eqri(baseRegister, instruction);
            else if (opCode == "eqrr")
                baseRegister = DAY16.AsmEmu.eqrr(baseRegister, instruction);
            else if (opCode == "gtir")
                baseRegister = DAY16.AsmEmu.gtir(baseRegister, instruction);
            else if (opCode == "gtri")
                baseRegister = DAY16.AsmEmu.gtri(baseRegister, instruction);
            else if (opCode == "gtrr")
                baseRegister = DAY16.AsmEmu.gtrr(baseRegister, instruction);
            else if (opCode == "muli")
                baseRegister = DAY16.AsmEmu.muli(baseRegister, instruction);
            else if (opCode == "mulr")
                baseRegister = DAY16.AsmEmu.mulr(baseRegister, instruction);
            else if (opCode == "seti")
                baseRegister = DAY16.AsmEmu.seti(baseRegister, instruction);
            else if (opCode == "setr")
                baseRegister = DAY16.AsmEmu.setr(baseRegister, instruction);
        }
    }
}

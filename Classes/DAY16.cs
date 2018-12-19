using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AoC2018
{
    public static class DAY16
    {
        private static Dictionary<int, List<string>> opCodeDict = new Dictionary<int, List<string>>();

        public static void Run()
        {
            List<string> linesInputFirst = File.ReadAllLines(Util.ReadFromInputFolder("16_A")).Where(arg => !string.IsNullOrWhiteSpace(arg)).ToList();
            List<string> linesInputSecond = File.ReadAllLines(Util.ReadFromInputFolder("16_B")).ToList();

            int countBehavior = 0;

            string[] opBase = new string[]
                { "addi", "addr", "bani", "banr",
                  "bori", "borr", "eqir", "eqri",
                  "eqrr", "gtir", "gtri", "gtrr",
                  "muli", "mulr", "seti", "setr"};
            for (int i = 0; i < 16; i++)
            {
                opCodeDict.Add(i, new List<string>(opBase));
            }

            //part 1
            while (linesInputFirst.Count > 0)
            {
                string line1 = linesInputFirst.First();
                string line2 = linesInputFirst.Skip(1).First();
                string line3 = linesInputFirst.Skip(2).First();
                linesInputFirst.RemoveRange(0, 3);

                string[] lin1array = line1.Between("[", "]").Split(',');
                string[] lin3array = line3.Between("[", "]").Split(',');

                int[] input = Array.ConvertAll(lin1array, s => Convert.ToInt32(s.Trim()));
                int[] instruction = Array.ConvertAll(line2.Split(' '), s => Convert.ToInt32(s.Trim()));
                int[] expectedOutput = Array.ConvertAll(lin3array, s => Convert.ToInt32(s.Trim()));

                FigureOutCodes(input, instruction, expectedOutput);
                if (part1Check(input, instruction, expectedOutput))
                    countBehavior++;
            }

            //part 2 figure the registers
            while (opCodeDict.Any(r => r.Value.Count > 1))
            {
                var orderedCandidates = opCodeDict.OrderBy(r => r.Value.Count);
                foreach (var pair in orderedCandidates)
                {
                    if (pair.Value.Count == 1)
                    {
                        opCodeDict.Where(r => r.Key != pair.Key).AsParallel().ForAll(r => r.Value.RemoveAll(w => w == pair.Value.First()));
                    }
                }
            }

            //part 2 run program
            int[] baseRegister = new int[4] { 0, 0, 0, 0 };
            foreach (string line in linesInputSecond)
            {
                int[] instruction = Array.ConvertAll(line.Split(' '), s => Convert.ToInt32(s.Trim()));
                string opCode = opCodeDict[instruction[0]].First();
                if (opCode == "addi")
                    baseRegister = AsmEmu.addi(baseRegister, instruction);
                else if (opCode == "addr")
                    baseRegister = AsmEmu.addr(baseRegister, instruction);
                else if (opCode == "bani")
                    baseRegister = AsmEmu.bani(baseRegister, instruction);
                else if (opCode == "banr")
                    baseRegister = AsmEmu.banr(baseRegister, instruction);
                else if (opCode == "bori")
                    baseRegister = AsmEmu.bori(baseRegister, instruction);
                else if (opCode == "borr")
                    baseRegister = AsmEmu.borr(baseRegister, instruction);
                else if (opCode == "eqir")
                    baseRegister = AsmEmu.eqir(baseRegister, instruction);
                else if (opCode == "eqri")
                    baseRegister = AsmEmu.eqri(baseRegister, instruction);
                else if (opCode == "eqrr")
                    baseRegister = AsmEmu.eqrr(baseRegister, instruction);
                else if (opCode == "gtir")
                    baseRegister = AsmEmu.gtir(baseRegister, instruction);
                else if (opCode == "gtri")
                    baseRegister = AsmEmu.gtri(baseRegister, instruction);
                else if (opCode == "gtrr")
                    baseRegister = AsmEmu.gtrr(baseRegister, instruction);
                else if (opCode == "muli")
                    baseRegister = AsmEmu.muli(baseRegister, instruction);
                else if (opCode == "mulr")
                    baseRegister = AsmEmu.mulr(baseRegister, instruction);
                else if (opCode == "seti")
                    baseRegister = AsmEmu.seti(baseRegister, instruction);
                else if (opCode == "setr")
                    baseRegister = AsmEmu.setr(baseRegister, instruction);
                else
                    Debug.Assert(false);
            }

            Console.WriteLine("PART 1: " + countBehavior);
            Console.WriteLine("PART 2: " + baseRegister[0] + " " + baseRegister[1] + " " + baseRegister[2] + " " + baseRegister[3]);
        }

        private static void FigureOutCodes(int[] input, int[] instruction, int[] output)
        {
            if (!AsmEmu.addi(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("addi");
            if (!AsmEmu.addr(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("addr");
            if (!AsmEmu.bani(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("bani");
            if (!AsmEmu.banr(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("banr");
            if (!AsmEmu.bori(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("bori");
            if (!AsmEmu.borr(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("borr");
            if (!AsmEmu.eqir(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("eqir");
            if (!AsmEmu.eqri(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("eqri");
            if (!AsmEmu.eqrr(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("eqrr");
            if (!AsmEmu.gtir(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("gtir");
            if (!AsmEmu.gtri(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("gtri");
            if (!AsmEmu.gtrr(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("gtrr");
            if (!AsmEmu.muli(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("muli");
            if (!AsmEmu.mulr(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("mulr");
            if (!AsmEmu.seti(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("seti");
            if (!AsmEmu.setr(input, instruction).SequenceEqual(output)) opCodeDict[instruction[0]].Remove("setr");
        }

        private static bool part1Check(int[] input, int[] instruction, int[] output)
        {
            int i = 0;
            i += AsmEmu.addi(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.addr(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.bani(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.banr(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.bori(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.borr(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.eqir(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.eqri(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.eqrr(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.gtir(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.gtri(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.gtrr(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.muli(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.mulr(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.seti(input, instruction).SequenceEqual(output) ? 1 : 0;
            i += AsmEmu.setr(input, instruction).SequenceEqual(output) ? 1 : 0;
            if (i >= 3)
                return true;
            else
                return false;
        }

        public static class AsmEmu
        {
            //Addition
            public static int[] addi(int[] register, int[] instruction)
            {
                // addi(add immediate) stores into register C the result of adding register A and value B.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                returnValue[C] = returnValue[A] + B;
                return returnValue;
            }

            public static int[] addr(int[] register, int[] instruction)
            {
                // addr(add register) stores into register C the result of adding register A and register B.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                returnValue[C] = returnValue[A] + returnValue[B];
                return returnValue;
            }

            //Multiplication
            public static int[] muli(int[] register, int[] instruction)
            {
                //muli(multiply immediate) stores into register C the result of multiplying register A and value B.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                returnValue[C] = returnValue[A] * B;
                return returnValue;
            }

            public static int[] mulr(int[] register, int[] instruction)
            {
                //mulr (multiply register) stores into register C the result of multiplying register A and register B.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                returnValue[C] = returnValue[A] * returnValue[B];
                return returnValue;
            }

            //Bitwise AND:
            public static int[] banr(int[] register, int[] instruction)
            {
                //banr(bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                returnValue[C] = BitwiseAnd(returnValue[A], returnValue[B]);
                return returnValue;
            }

            public static int[] bani(int[] register, int[] instruction)
            {
                //bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                returnValue[C] = BitwiseAnd(returnValue[A], B);
                return returnValue;
            }

            //Bitwise OR:
            public static int[] borr(int[] register, int[] instruction)
            {
                //borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                returnValue[C] = BitwiseOr(returnValue[A], returnValue[B]);
                return returnValue;
            }

            public static int[] bori(int[] register, int[] instruction)
            {
                //bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                returnValue[C] = BitwiseOr(returnValue[A], B);
                return returnValue;
            }

            //Assignment
            public static int[] setr(int[] register, int[] instruction)
            {
                //setr (set register) copies the contents of register A into register C. (Input B is ignored.)
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                returnValue[C] = returnValue[A];
                return returnValue;
            }

            public static int[] seti(int[] register, int[] instruction)
            {
                //seti (set immediate) stores value A into register C. (Input B is ignored.)
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                returnValue[C] = A;
                return returnValue;
            }

            //Greater-than testing
            public static int[] gtir(int[] register, int[] instruction)
            {
                //gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                if (A > returnValue[B])
                    returnValue[C] = 1;
                else
                    returnValue[C] = 0;
                return returnValue;
            }

            public static int[] gtri(int[] register, int[] instruction)
            {
                //gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                if (returnValue[A] > B)
                    returnValue[C] = 1;
                else
                    returnValue[C] = 0;
                return returnValue;
            }

            public static int[] gtrr(int[] register, int[] instruction)
            {
                //gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                if (returnValue[A] > returnValue[B])
                    returnValue[C] = 1;
                else
                    returnValue[C] = 0;
                return returnValue;
            }

            //Equality testing:
            public static int[] eqir(int[] register, int[] instruction)
            {
                //eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                if (A == returnValue[B])
                    returnValue[C] = 1;
                else
                    returnValue[C] = 0;
                return returnValue;
            }

            public static int[] eqri(int[] register, int[] instruction)
            {
                //eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                if (returnValue[A] == B)
                    returnValue[C] = 1;
                else
                    returnValue[C] = 0;
                return returnValue;
            }

            public static int[] eqrr(int[] register, int[] instruction)
            {
                //eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
                int[] returnValue = (int[])register.Clone();

                int opCode = instruction[0];
                int A = instruction[1];
                int B = instruction[2];
                int C = instruction[3];

                if (returnValue[A] == returnValue[B])
                    returnValue[C] = 1;
                else
                    returnValue[C] = 0;
                return returnValue;
            }

            //Binary literal support exists only from C# 7.0 onwards so this will do for now
            private static int BitwiseAnd(int A, int B)
            {
                char[] result = new char[4];

                string binaryA = Convert.ToString(A, 2).PadLeft(4, '0');
                string binaryB = Convert.ToString(B, 2).PadLeft(4, '0');

                for (int i = 0; i < binaryA.Length; i++)
                {
                    if (binaryA[i] == '1' && binaryB[i] == '1')
                        result[i] = '1';
                    else
                        result[i] = '0';
                }

                return Convert.ToInt32(new string(result), 2);
            }
            private static int BitwiseOr(int A, int B)
            {
                char[] result = new char[4];

                string binaryA = Convert.ToString(A, 2).PadLeft(4, '0');
                string binaryB = Convert.ToString(B, 2).PadLeft(4, '0');

                for (int i = 0; i < binaryA.Length; i++)
                {
                    if (binaryA[i] == '1' || binaryB[i] == '1')
                        result[i] = '1';
                    else
                        result[i] = '0';
                }

                return Convert.ToInt32(new string(result), 2);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY5
    {
        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(5));
            Console.WriteLine(Problem1(linesInput[0]));
            Console.WriteLine(Problem2(linesInput[0]));
        }

        /// <summary>
        /// NOTE: Accept only first line of input
        /// </summary>
        /// <param name="linesInput"></param>
        /// <returns></returns>
        public static int Problem1(string linesInput)
        {
            List<char> originalInput = linesInput.ToList();
            string result = ReactPolymerStack(originalInput);
            return result.Length;
        }

        /// <summary>
        /// NOTE: Accept only first line of input
        /// </summary>
        /// <param name="linesInput"></param>
        /// <returns></returns>
        public static int Problem2(string linesInput)
        {
            List<char> originalInput = linesInput.ToList();
            List<Result> lstResult = new List<Result>();

            var DistinctChars = originalInput.GroupBy(r => Char.ToLower(r));

            foreach (char caract in DistinctChars.Select(r => r.Key))
            {
                List<char> line = originalInput;
                line = line.Where(r => Char.ToLower(r) != caract).ToList();
                string result = ReactPolymerStack(line); //ReactPolymer(line);
                lstResult.Add(new Result(caract, result.Length, result));
            }

            return lstResult.OrderBy(r => r.lineLength).First().lineLength;
        }

        private class Result
        {
            public char offendingCharacter;
            public int lineLength;
            public string strResult;

            public Result(char _o, int _l, string _s)
            {
                offendingCharacter = _o;
                lineLength = _l;
                strResult = _s;
            }
        }

        private static string ReactPolymerStack(List<char> line)
        {
            Stack<char> characterStack = new Stack<char>();

            foreach (char currentChar in line)
            {
                if (characterStack.Count == 0)
                    characterStack.Push(currentChar);
                else
                {
                    char topChar = characterStack.Peek();
                    if (Char.ToLower(topChar) == Char.ToLower(currentChar) && topChar != currentChar)
                    {
                        characterStack.Pop();
                    }
                    else
                    {
                        characterStack.Push(currentChar);
                    }
                }
            }
            return (String.Concat(characterStack.ToArray()));
        }

        private static string ReactPolymer(List<char> line)
        {
            bool removalTookPlace = true;

            do
            {
                removalTookPlace = false;
                for (int i = 0; i < line.Count(); i++)
                {
                    if (i + 1 > line.Count - 1)
                        break;
                    if (Char.ToLower(line[i]) == Char.ToLower(line[i + 1]))
                    {
                        if (line[i] != line[i + 1])
                        {
                            line.RemoveAt(i + 1);
                            line.RemoveAt(i);
                            removalTookPlace = true;
                        }
                    }
                }

            } while (removalTookPlace == true);

            return (String.Concat(line.ToArray()));
        }
    }
}

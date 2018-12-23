using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace AoC2018
{
    public static class Util
    {
        public static int ManhattanDist(Point a, Point b)
        {
            //(x1 - x2) + (y1 - y2)?
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        public class PriorityQueue<T>
        {
            private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

            public int Count
            {
                get { return elements.Count; }
            }

            public void Enqueue(T item, float priority)
            {
                elements.Add(new KeyValuePair<T, float>(item, priority));
            }

            // Returns the Location that has the lowest priority
            public T Dequeue()
            {
                int bestIndex = 0;

                for (int i = 0; i < elements.Count; i++)
                {
                    if (elements[i].Value < elements[bestIndex].Value)
                    {
                        bestIndex = i;
                    }
                }

                T bestItem = elements[bestIndex].Key;
                elements.RemoveAt(bestIndex);
                return bestItem;
            }
        }

        public static LinkedListNode<T> GetNextCircular<T>(LinkedListNode<T> currentNode, int? count = null)
        {
            if (count == null || count == 1)
            {
                currentNode = currentNode.Next != null ? currentNode.Next : currentNode.List.First;
                return currentNode;
            }
            for (int i = 0; i < count; i++)
            {
                currentNode = currentNode.Next != null ? currentNode.Next : currentNode.List.First;
            }
            return currentNode;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                throw new ArgumentNullException();
            if(action == null)
                throw new ArgumentNullException();
            foreach (T element in source)
            {
                action(element);
            }
        }

        // From: https://www.geeksforgeeks.org/sum-factors-number/
        // Function to calculate sum of all divisors of a given number
        public static int divSum(int n)
        {
            // Final result of summation  
            // of divisors 
            int result = 0;

            // find all divisors which divides 'num' 
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                // if 'i' is divisor of 'n' 
                if (n % i == 0)
                {
                    // if both divisors are same 
                    // then add it once else add 
                    // both 
                    if (i == (n / i))
                        result += i;
                    else
                        result += (i + n / i);
                }
            }

            // Add 1 and n to result as above loop 
            // considers proper divisors greater 
            // than 1. 
            return (result + n + 1);
        }

        public static LinkedListNode<T> GetPreviousCircular<T>(LinkedListNode<T> currentNode, int? count = null)
        {
            if (count == null || count == 1)
            {
                currentNode = currentNode.Previous != null ? currentNode.Previous : currentNode.List.Last;
                return currentNode;
            }
            for (int i = 0; i < count; i++)
            {
                currentNode = currentNode.Previous != null ? currentNode.Previous : currentNode.List.Last;
            }
            return currentNode;
        }

        public static int GetPreviousCircular(int Count, int index, int? count = null)
        {
            if (count != null)
            {
                for (int i = 0; i < count; i++)
                {
                    index--; // decrement index
                    if (index < 0)
                    {
                        index = Count - 1;
                    }
                }
            }
            else
            {
                index--; // decrement index
                if (index < 0)
                {
                    index = Count - 1;
                }
            }
            return index;
        }

        public static int GetNextCircular(int Count, int index, int? count = null)
        {
            if (count != null)
            {
                for (int i = 0; i < count; i++)
                {
                    index++; // increment index
                    index %= Count;
                }
            }
            else
            {
                index++; // increment index
                index %= Count;
            }
            return index;
        }

        public static void WriteToFile(StringBuilder sb)
        {
            File.WriteAllText(@"C:\OUTPUT.txt", String.Empty);
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\OUTPUT.txt", true))
            {
                file.WriteLine(sb.ToString());
            }
        }

        public static void WriteToFile(string sb)
        {
            File.WriteAllText(@"C:\OUTPUT.txt", String.Empty);
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\OUTPUT.txt", true))
            {
                file.WriteLine(sb);
            }
        }
        public static string ReadFromInputFolder(int problemNumber)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Inputs\puzzle"+problemNumber+".txt");
        }

        public static string ReadFromInputFolder(string problemNumber)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Inputs\puzzle" + problemNumber + ".txt");
        }

        public static string Between(this string value, string a, string b = null)
        {
            int posA = value.IndexOf(a);
            int posB;
            if (b != null)
                posB = value.LastIndexOf(b);
            else
                posB = value.Length;
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        public static bool oneDifference(String a, String b)
        {
            int differences = 0;

            char[] charA = a.ToArray();
            char[] charB = b.ToArray();

            for (int i = 0; i < charA.Length; i++)
            {
                if (charA[i] != charB[i])
                {
                    differences++;
                }

                if (differences > 1)
                {
                    return false;
                }
            }

            return true;
        }

        public static class LevenshteinDistance
        {
            /// <summary>
            /// Compute the distance between two strings.
            /// </summary>
            public static int Compute(string s, string t)
            {
                int n = s.Length;
                int m = t.Length;
                int[,] d = new int[n + 1, m + 1];

                // Step 1
                if (n == 0)
                {
                    return m;
                }

                if (m == 0)
                {
                    return n;
                }

                // Step 2
                for (int i = 0; i <= n; d[i, 0] = i++)
                {
                }

                for (int j = 0; j <= m; d[0, j] = j++)
                {
                }

                // Step 3
                for (int i = 1; i <= n; i++)
                {
                    //Step 4
                    for (int j = 1; j <= m; j++)
                    {
                        // Step 5
                        int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                        // Step 6
                        d[i, j] = Math.Min(
                            Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                            d[i - 1, j - 1] + cost);
                    }
                }
                // Step 7
                return d[n, m];
            }
        }
    }
}

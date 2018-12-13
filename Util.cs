using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    public static class Util
    {
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

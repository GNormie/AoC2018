using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.Diagnostics;
using System.Drawing;

namespace AoC2018
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] linesInput = File.ReadAllLines(@"C:\puzzle5.txt");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Console.WriteLine(DAY5.Problem1(linesInput[0]));
            Console.WriteLine(DAY5.Problem2(linesInput[0]));

            sw.Stop();
            Console.WriteLine("ms: "+sw.ElapsedMilliseconds);

            Console.ReadLine();
        }

        
    }
}
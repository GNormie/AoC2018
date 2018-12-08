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
            string[] linesInput = File.ReadAllLines(@"C:\puzzle7.txt");
            string Part1Result = DAY7.Problem1(linesInput);
            int Part2Result = DAY7.Problem2(linesInput, Part1Result);
            Console.WriteLine("P1: "+ Part1Result);
            Console.WriteLine("P2: "+ Part2Result);
            Console.ReadLine();
        }
    }
}
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
            string[] linesInput = File.ReadAllLines(@"C:\puzzle6.txt");
            DAY6.Problem1(linesInput);
            DAY6.Problem2(linesInput);
            Console.ReadLine();
        }
    }
}
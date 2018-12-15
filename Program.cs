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
            #region CONEMU
#if DEBUG
            ProcessStartInfo pi = new ProcessStartInfo(@"C:\cmder\vendor\conemu-maximus5\ConEmu\ConEmuC.exe", "/AUTOATTACH");
            pi.CreateNoWindow = false;
            pi.UseShellExecute = false;
            //pi.WindowStyle = ProcessWindowStyle.Maximized;
            Process.Start(pi);
#endif
            #endregion
            string[] linesInput = File.ReadAllLines(@"C:\puzzle14.txt");
            foreach (string line in linesInput)
            {
            
            }
            Console.ReadLine();
        }  
    }
}
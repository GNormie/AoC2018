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
            string[] linesInput = File.ReadAllLines(@"C:\puzzle8.txt");
            List<string> dataValuesSTR = linesInput[0].Split(' ').ToList();

            int baseID = (int)'A' - 1;

            //test input
            //List<int> dataValues = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2".Split(' ').ToList().Select(r => Convert.ToInt32(r)).ToList();
            List<int> dataValues = dataValuesSTR.Select(r => Convert.ToInt32(r)).ToList();

            List<int> sumNumbers = new List<int>();

            QuirkNode baseNode = new QuirkNode();
            baseNode.ID = (char)baseID;


            while (dataValues.Count > 0)
            {
                baseNode = RecursiveEat(ref dataValues, ref sumNumbers, ref baseID);
            }

            Console.WriteLine("PART 1: "+sumNumbers.Sum(r => r));

            List<int> lstValueNodes = new List<int>();
            foreach (var metadataEntry in baseNode.lstMetadataEntries)
            {
                CheckValue(baseNode.ChildNodes.ElementAtOrDefault(metadataEntry-1), ref lstValueNodes);
            }
            Console.WriteLine("PART 2: " + lstValueNodes.Sum());
            Console.ReadLine();
        }

        public static void CheckValue(QuirkNode nodum, ref List<int> lstValuesNodes)
        {
            if (nodum == null)
            {
                lstValuesNodes.Add(0);
                return;
            }
            if (nodum.ChildNodes.Count() == 0)
            {
                lstValuesNodes.Add(nodum.lstMetadataEntries.Sum());
            }
            else
            {
                foreach (var Metadata in nodum.lstMetadataEntries)
                {
                     CheckValue(nodum.ChildNodes.ElementAtOrDefault(Metadata-1), ref lstValuesNodes);
                }
            }
        }

        public static QuirkNode RecursiveEat(ref List<int> dataValues, ref List<int> sumNumbers, ref int IDCarrier)
        {
            IDCarrier++;
            int NumberOfChildNodes = dataValues.First();
            int NumberOfMetadataEntries = dataValues.Skip(1).First();

            dataValues.RemoveAt(1);
            dataValues.RemoveAt(0);

            QuirkNode node = new QuirkNode();
            node.ID = (char)IDCarrier;

            if (NumberOfChildNodes == 0)
            {
                for (int w = 0; w < NumberOfMetadataEntries; w++)
                {
                    sumNumbers.Add(dataValues.First());
                    node.lstMetadataEntries.Add(dataValues.First());
                    dataValues.RemoveAt(0);
                }
                return node;
            }
            else
            {
                for (int i = 0; i < NumberOfChildNodes; i++)
                {
                    QuirkNode childNode = RecursiveEat(ref dataValues, ref sumNumbers, ref IDCarrier);
                    node.ChildNodes.Add(childNode);
                }
                for (int w = 0; w < NumberOfMetadataEntries; w++)
                {
                    sumNumbers.Add(dataValues.First());
                    node.lstMetadataEntries.Add(dataValues.First());
                    dataValues.RemoveAt(0);
                }
                return node;
            }
        }

        public class QuirkNode
        {
            public char ID;
            public List<QuirkNode> ChildNodes = new List<QuirkNode>();
            public List<int> lstMetadataEntries = new List<int>();

            /*
            public int getValue()
            {
                foreach (var ChildNode in ChildNodes)
                {
                    if (ChildNode.ChildNodes.Count == 0)
                    {
                        return lstMetadataEntries.Sum();
                    }
                    else
                    {

                    }
                }
                
            }*/
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY8
    {
        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(8));
            QuirkNode baseNode;
            Console.WriteLine(Problem1(linesInput, out baseNode));
            Console.WriteLine(Problem2(baseNode));
        }
        public static int Problem1(string[] linesInput, out QuirkNode baseNode)
        {
            List<string> dataValuesSTR = linesInput[0].Split(' ').ToList();
            List<int> dataValues = dataValuesSTR.Select(r => Convert.ToInt32(r)).ToList();

            List<int> sumNumbers = new List<int>();

            int baseID = (int)'A' - 1;
            baseNode = new QuirkNode();
            baseNode.ID = (char)baseID;

            while (dataValues.Count > 0)
            {
                baseNode = RecursiveEat(ref dataValues, ref sumNumbers, ref baseID);
            }

            return sumNumbers.Sum(r => r);
        }

        public static int Problem2(QuirkNode baseNode)
        {
            List<int> lstValueNodes = new List<int>();
            foreach (var metadataEntry in baseNode.lstMetadataEntries)
            {
                AddNodesValue(baseNode.ChildNodes.ElementAtOrDefault(metadataEntry - 1), ref lstValueNodes);
            }
            return lstValueNodes.Sum();
        }

        public class QuirkNode
        {
            public char ID;
            public List<QuirkNode> ChildNodes = new List<QuirkNode>();
            public List<int> lstMetadataEntries = new List<int>();
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

        public static void AddNodesValue(QuirkNode nodum, ref List<int> lstValuesNodes)
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
                    AddNodesValue(nodum.ChildNodes.ElementAtOrDefault(Metadata - 1), ref lstValuesNodes);
                }
            }
        }
    }
}

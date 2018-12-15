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
    public static class DAY14
    {
        //static int initialRecipe = 37;


        static int iterations = 920831;
        static string strIterations = iterations.ToString();
        static LinkedList<byte> lstRecipes = new LinkedList<byte>();

        static LinkedListNode<byte> firstRecipe;// = lstRecipes.First;
        static LinkedListNode<byte> secondRecipe;// = lstRecipes.Last;

        static bool breakPart2 = false;

        static LinkedList<byte> lstResult = new LinkedList<byte>();

        public static void Run()
        {
            Part1();
            Part2();
        }

        private static void Part1()
        {
            lstRecipes.AddFirst(7);
            lstRecipes.AddFirst(3);
            firstRecipe = lstRecipes.First;
            secondRecipe = lstRecipes.First.Next;

            for (int i = 0; i < 10 + iterations; i++)
            {
                OperateRecipe(false);
            }

            LinkedListNode<byte> startPoint = lstRecipes.First;
            var properPoint = Util.GetNextCircular(startPoint, iterations);
            for (int i = 0; i < 10; i++)
            {
                lstResult.AddLast(properPoint.Value);
                properPoint = properPoint.Next;
            }
            Console.WriteLine("PART 1: "+string.Concat(lstResult));
        }

        private static void Part2()
        {
            while (breakPart2 == false)
            {
                OperateRecipe(true);
            }
        }

        public static void OperateRecipe(bool part2)
        {
            byte firstRecipeByte = firstRecipe.Value;
            byte secondRecipeByte = secondRecipe.Value;
            byte newResult = (byte)(firstRecipeByte + secondRecipeByte);
            var newRecipes = newResult.ToString().ToArray();
            foreach (char recipe in newRecipes)
            {
                lstRecipes.AddLast((byte)Char.GetNumericValue(recipe));
                if (part2 == true)
                {
                    if (lstRecipes.Count > strIterations.Length && getLastString() == strIterations)
                    {
                        Console.WriteLine("PART 2: " + (lstRecipes.Count - strIterations.Length));
                        breakPart2 = true;
                    }
                }
                
            }
            firstRecipe = Util.GetNextCircular(firstRecipe, firstRecipeByte + 1);
            secondRecipe = Util.GetNextCircular(secondRecipe, secondRecipeByte + 1);
        }

        public static string getLastString()
        {
            LinkedList<byte> currResult = new LinkedList<byte>();
            var lastElement = lstRecipes.Last;
            for (byte i = 0; i < strIterations.Length; i++)
            {
                currResult.AddFirst(lastElement.Value);
                lastElement = lastElement.Previous;
            }
            return string.Concat(currResult);
        }
    }
}
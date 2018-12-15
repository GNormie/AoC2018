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
        static int initialRecipe = 37;
        static int iterations = 920831;
        static string strIterations = iterations.ToString();
        static LinkedList<char> lstRecipes = new LinkedList<char>(initialRecipe.ToString().ToArray());

        static LinkedListNode<char> firstRecipe = lstRecipes.First;
        static LinkedListNode<char> secondRecipe = lstRecipes.Last;

        static bool breakPart2 = false;

        static LinkedList<char> lstResult = new LinkedList<char>();

        public static void Part1()
        {
            for (int i = 0; i < 10 + iterations; i++)
            {
                OperateRecipe(false);
            }

            LinkedListNode<char> startPoint = lstRecipes.First;
            var properPoint = GetNextCircular(startPoint, iterations);
            for (int i = 0; i < 10; i++)
            {
                lstResult.AddLast(properPoint.Value);
                properPoint = properPoint.Next;
            }
            Console.WriteLine("PART 1: "+string.Concat(lstResult));
        }

        public static void Part2()
        {
            while (breakPart2 == false)
            {
                OperateRecipe(true);
            }
        }

        public static void OperateRecipe(bool part2)
        {
            double firstRecipeInt = Char.GetNumericValue(firstRecipe.Value);
            double secondRecipeInt = Char.GetNumericValue(secondRecipe.Value);
            double newResult = firstRecipeInt + secondRecipeInt;
            var newRecipes = newResult.ToString().ToArray();
            foreach (char recipe in newRecipes)
            {
                lstRecipes.AddLast(recipe);
                if (part2 == true)
                {
                    if (lstRecipes.Count > strIterations.Length && getLastString() == strIterations)
                    {
                        Console.WriteLine("PART 2: " + (lstRecipes.Count - strIterations.Length));
                        breakPart2 = true;
                    }
                }
                
            }
            firstRecipe = GetNextCircular(firstRecipe, firstRecipeInt + 1);
            secondRecipe = GetNextCircular(secondRecipe, secondRecipeInt + 1);
        }

        public static string getLastString()
        {
            LinkedList<char> currResult = new LinkedList<char>();
            var dude = lstRecipes.Last;
            currResult.AddFirst(dude.Value);
            currResult.AddFirst(dude.Previous.Value);
            currResult.AddFirst(dude.Previous.Previous.Value);
            currResult.AddFirst(dude.Previous.Previous.Previous.Value);
            currResult.AddFirst(dude.Previous.Previous.Previous.Previous.Value);
            currResult.AddFirst(dude.Previous.Previous.Previous.Previous.Previous.Value);
            return string.Concat(currResult);
        }

        public static LinkedListNode<T> GetNextCircular<T>(LinkedListNode<T> currentNode, double? count = null)
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
    }
}
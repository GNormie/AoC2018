using System.Collections.Generic;
using System.Linq;
using System;

namespace AoC2018
{
    public static class DAY14
    {
        //static int initialRecipe = 37;

        static byte[] temporalQueue = new byte[2];
        static byte howManyGet = 1;

        static int iterations = 920831;
        static string strIterations = iterations.ToString();
        static LinkedList<byte> lstRecipes = new LinkedList<byte>();

        static LinkedListNode<byte> firstRecipe;// = lstRecipes.First;
        static LinkedListNode<byte> secondRecipe;// = lstRecipes.Last;

        static bool breakPart2 = false;

        static LinkedList<byte> lstResult = new LinkedList<byte>();

        static LinkedList<byte> currResult = new LinkedList<byte>();
        static double tempoResult = 0;

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

        private static void OperateRecipe(bool part2)
        {
            byte firstRecipeByte = firstRecipe.Value;
            byte secondRecipeByte = secondRecipe.Value;
            byte newResult = (byte)(firstRecipeByte + secondRecipeByte);
            if (newResult >= 10)
            {
                temporalQueue[0] = 1;
                temporalQueue[1] = (byte)(newResult % 10);
                howManyGet = 2;
            }
            else
            {
                temporalQueue[0] = newResult;
                howManyGet = 1;
            }                
            var newRecipes = newResult.ToString().ToArray();
            for(byte w = 0; w < howManyGet; w++)
            {
                lstRecipes.AddLast(temporalQueue[w]);
                if (part2 == true)
                {
                    if (getLastNumber() == iterations)
                    {
                        Console.WriteLine("PART 2: " + (lstRecipes.Count - strIterations.Length));
                        breakPart2 = true;
                    }
                }
                
            }
            firstRecipe = Util.GetNextCircular(firstRecipe, firstRecipeByte + 1);
            secondRecipe = Util.GetNextCircular(secondRecipe, secondRecipeByte + 1);
        }

        private static double getLastNumber()
        {
            tempoResult = 0;
            var lastElement = lstRecipes.Last;
            for (byte i = 0; i < strIterations.Length; i++)
            {
                tempoResult += lastElement.Value * (Math.Pow(10, i));
                lastElement = lastElement.Previous;
            }
            return tempoResult;
        }
    }
}
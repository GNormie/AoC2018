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
            Dictionary<char, List<char>> dctRequirements = new Dictionary<char, List<char>>();

            Dictionary<char, List<char>> dctReqSecondPass = new Dictionary<char, List<char>>();


            string[] linesInput = File.ReadAllLines(@"C:\puzzle7.txt");

            Dictionary<char, int> timeCostDict = new Dictionary<char, int>();
            List<char> Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();
            foreach (char S in Alphabet)
            {
                int timeCost = Convert.ToInt32(S) - 4;
                timeCostDict.Add(S, timeCost);
            }

            foreach (string line in linesInput)
            {
                char preReq = line.Between("Step ", " must")[0];
                char Step = line.Between("e step ", " can begin")[0];

                if (dctRequirements.ContainsKey(preReq) == false)
                {
                    dctRequirements.Add(preReq, new List<char>());
                }
                if (dctRequirements.ContainsKey(Step) == false)
                {
                    dctRequirements.Add(Step, new List<char>());
                }
                dctRequirements[Step].Add(preReq);
            }

            List<char> stepsToFollow = new List<char>();
            while (dctRequirements.Count != 0)
            {
                var Requeriment = dctRequirements.OrderBy(w => w.Value.Count()).ThenBy(r => r.Key).First();
                ResolveRecursive(ref dctRequirements, ref stepsToFollow, Requeriment.Key);
            }

            Console.WriteLine(string.Concat(stepsToFollow));

            //PART 2

            foreach (string line in linesInput)
            {
                char preReq = line.Between("Step ", " must")[0];
                char Step = line.Between("e step ", " can begin")[0];

                if (dctReqSecondPass.ContainsKey(preReq) == false)
                {
                    dctReqSecondPass.Add(preReq, new List<char>());
                }
                if (dctReqSecondPass.ContainsKey(Step) == false)
                {
                    dctReqSecondPass.Add(Step, new List<char>());
                }
                dctReqSecondPass[Step].Add(preReq);
            }

            int seconds = 0;
            List<Workers> lstWorkers = new List<Workers>();
            lstWorkers.Add(new Workers(1));
            lstWorkers.Add(new Workers(2));
            lstWorkers.Add(new Workers(3));
            lstWorkers.Add(new Workers(4));
            lstWorkers.Add(new Workers(5));
            List<char> taskDone = new List<char>();
            bool canContinue = true;
            while(canContinue/*stepsToFollow.Count != 0*/)
            {
                foreach (Workers pawn in lstWorkers)
                {
                    char taskComplete = pawn.Work(seconds);
                    if (taskComplete != ' ')
                    {
                        taskDone.Add(taskComplete);
                    }

                    if (pawn.available)
                    {
                        if (stepsToFollow.Count == 0)
                            continue;

                        foreach (char S in stepsToFollow)
                        {
                            char TASK = S;

                            bool canAssignTask = false;
                            if (dctReqSecondPass[TASK].Count == 0)
                                canAssignTask = true;
                            else if (checkRequirements(taskDone, dctReqSecondPass[TASK]))
                                canAssignTask = true;

                            if (canAssignTask)
                            {
                                pawn.AssignTask(TASK, timeCostDict, seconds);
                                stepsToFollow.Remove(TASK);
                                break;
                            }
                        }
                              
                    }
                    
                }
                seconds++;
                if (stepsToFollow.Count() == 0 && lstWorkers.All(r => r.available == true))
                    canContinue = false;
            } 

            Console.WriteLine(seconds-2);
            Console.ReadLine();
        }

        public static bool checkRequirements(List<char> taskDone, List<char> dctReqSecondPass)
        {
            if (taskDone.Count == 0)
                return false;
            foreach (var taskReq in dctReqSecondPass)
            {
                if (taskDone.Contains(taskReq) == false)
                    return false;
            }
            return true;
        }

        public class Workers
        {
            int workerID = 0;
            public bool available = true;
            public char currentTask = ' ';
            public int timeToComplete = 0;

            public Workers(int _ID)
            {
                workerID = _ID;
            }

            public void AssignTask(char TASK, Dictionary<char, int> timeCostDict, int seconds)
            {
                available = false;
                currentTask = TASK;
                timeToComplete = timeCostDict[TASK];
                Console.WriteLine(seconds + " - ASSIGNING: " + TASK + " to  Worker(" + workerID + ") / Time Cost: " + timeCostDict[TASK] +" / Expected Finish: "+ (seconds + (int)timeCostDict[TASK]));
            }

            public char Work(int seconds)
            {
                if (currentTask == ' ')
                    return ' ';
                if (available == false && currentTask != ' ')
                {
                    timeToComplete--;
                }
                if (timeToComplete == 0)
                {
                   var returnValue = currentTask;
                   currentTask = ' ';
                   available = true;
                   Console.WriteLine(seconds+" - Worker("+workerID+") COMPLETED: " + returnValue);
                   return returnValue;
                }
                return ' ';
            }
        }

        static char ResolveRecursive(ref Dictionary<char, List<char>> dctRequirements, ref List<char> stepsToFollow, char SYMBOL)
        {
            if (dctRequirements.ContainsKey(SYMBOL) == false)
                return SYMBOL;

            var Requeriment = dctRequirements[SYMBOL];
            if (Requeriment.Any() == false)
            {
                stepsToFollow.Add(SYMBOL);
                dctRequirements.Remove(SYMBOL);
                dctRequirements.AsParallel().ForAll(r => r.Value.RemoveAll(w => w == SYMBOL));
                return SYMBOL;
            }
            else
            {
                var leValor = Requeriment.OrderBy(r => r).First();
                if (stepsToFollow.Contains(leValor))
                {
                    Requeriment.Remove(leValor);
                    return SYMBOL;
                }
                var altSymbol = ResolveRecursive(ref dctRequirements, ref stepsToFollow, leValor);

                stepsToFollow.Add(altSymbol);
                dctRequirements[SYMBOL].Remove(altSymbol);
                return SYMBOL;
            }
        }
    }
}
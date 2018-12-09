using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY7
    {
        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(7));
            string problem1Solution = Problem1(linesInput);
            Console.WriteLine(problem1Solution);
            Console.WriteLine(Problem2(linesInput, problem1Solution));
        }

        public static string Problem1(string[] linesInput)
        {
            Dictionary<char, List<char>> dctRequirements = new Dictionary<char, List<char>>();

            foreach (string line in linesInput)
            {
                char preReq = line.Between("Step ", " must")[0];
                char Step = line.Between("e step ", " can begin")[0];

                if (dctRequirements.ContainsKey(preReq) == false)
                    dctRequirements.Add(preReq, new List<char>());
                if (dctRequirements.ContainsKey(Step) == false)
                    dctRequirements.Add(Step, new List<char>());
                dctRequirements[Step].Add(preReq);
            }

            List<char> stepsToFollow = new List<char>();
            while (dctRequirements.Count != 0)
            {
                var Requeriment = dctRequirements.OrderBy(w => w.Value.Count()).ThenBy(r => r.Key).First();
                Resolve(ref dctRequirements, ref stepsToFollow, Requeriment.Key);
            }

            return string.Concat(stepsToFollow);
        }

        public static int Problem2(string[] linesInput, string stepsToFollowSTR)
        {
            List<char> stepsToFollow = stepsToFollowSTR.ToList();
            Dictionary<char, List<char>> dctReqSecondPass = new Dictionary<char, List<char>>();

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
                if (dctReqSecondPass.ContainsKey(preReq) == false)
                    dctReqSecondPass.Add(preReq, new List<char>());
                if (dctReqSecondPass.ContainsKey(Step) == false)
                    dctReqSecondPass.Add(Step, new List<char>());
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
            while (canContinue)
            {
                foreach (Workers pawn in lstWorkers)
                {
                    char taskComplete = pawn.Work(seconds);
                    if (taskComplete != ' ')
                        taskDone.Add(taskComplete);

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
            return (seconds - 2);
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
            public bool debugWorker = false;

            public Workers(int _ID, bool _debugWorker = false)
            {
                workerID = _ID;
                debugWorker = _debugWorker;
            }

            public void AssignTask(char TASK, Dictionary<char, int> timeCostDict, int seconds)
            {
                available = false;
                currentTask = TASK;
                timeToComplete = timeCostDict[TASK];
                if (debugWorker)
                    Console.WriteLine(seconds + " - ASSIGNING: " + TASK + " to  Worker(" + workerID + ") / Time Cost: " + timeCostDict[TASK] + " / Expected Finish: " + (seconds + (int)timeCostDict[TASK]));
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
                    if (debugWorker)
                        Console.WriteLine(seconds + " - Worker(" + workerID + ") COMPLETED: " + returnValue);
                    return returnValue;
                }
                return ' ';
            }
        }

        static char Resolve(ref Dictionary<char, List<char>> dctRequirements, ref List<char> stepsToFollow, char SYMBOL)
        {
            if (dctRequirements.ContainsKey(SYMBOL) == false)
                return SYMBOL;

            var Requeriment = dctRequirements[SYMBOL];

            stepsToFollow.Add(SYMBOL);
            dctRequirements.Remove(SYMBOL);
            dctRequirements.AsParallel().ForAll(r => r.Value.RemoveAll(w => w == SYMBOL));

            return SYMBOL;
        }
    }
}

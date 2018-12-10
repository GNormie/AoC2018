using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY9
    {
        public static void Run()
        {
            string[] linesInput = File.ReadAllLines(Util.ReadFromInputFolder(9));
            string INPUT = linesInput[0];
            var inputArray = INPUT.Split(' ');
            int numberOfPlayers = Convert.ToInt32(inputArray[0]);
            long marbles = Convert.ToInt32(inputArray[6]);

            Console.WriteLine(Problem1(numberOfPlayers, marbles));
            Console.WriteLine(Problem2(numberOfPlayers, marbles));
        }

        public static long Problem1(int numberOfPlayers, long marbles)
        {
            LinkedList<int> marbleCircle = new LinkedList<int>();
            marbleCircle.AddFirst(1);
            marbleCircle.AddFirst(0);

            List<Player> lstPlayers = new List<Player>();
            for (int i = 1; i < numberOfPlayers + 1; i++)
                lstPlayers.Add(new Player(i));

            int currentPlayer = 3;

            LinkedListNode<int> leCurrentNode = marbleCircle.Last;
            for (int currentMarble = 2; currentMarble < marbles; currentMarble++)
            {
                var marble = currentMarble;

                if (marble % 23 != 0)
                {
                    var insertPosition = GetNextCircular(leCurrentNode);
                    leCurrentNode = marbleCircle.AddAfter(insertPosition, currentMarble);
                }
                else
                {
                    var player = lstPlayers.SingleOrDefault(r => r.playerID == currentPlayer);
                    player.score += marble;

                    var insertPosition = GetPreviousCircular(leCurrentNode, 7);
                    player.score += insertPosition.Value;
                    leCurrentNode = GetNextCircular(insertPosition);
                    marbleCircle.Remove(insertPosition);
                }
                currentPlayer = lstPlayers.ElementAt(GetNextCircular(lstPlayers.Count, currentPlayer - 1)).playerID;
            }

            return lstPlayers.Max(r => r.score);
        }

        public static long Problem2(int numberOfPlayers, long marbles)
        {
            return Problem1(numberOfPlayers, marbles * 100);
        }

        public class Player
        {
            public int playerID;
            public long score = 0;

            public Player(int _playerID)
            {
                playerID = _playerID;
            }
        }

        public static LinkedListNode<int> GetNextCircular(LinkedListNode<int> currentNode, int? count = null)
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

        public static LinkedListNode<int> GetPreviousCircular(LinkedListNode<int> currentNode, int? count = null)
        {
            if (count == null || count == 1)
            {
                currentNode = currentNode.Previous != null ? currentNode.Previous : currentNode.List.Last;
                return currentNode;
            }
            for (int i = 0; i < count; i++)
            {
                currentNode = currentNode.Previous != null ? currentNode.Previous : currentNode.List.Last;
            }
            return currentNode;
        }

        public static int GetPreviousCircular(int Count, int index, int? count = null)
        {
            if (count != null)
            {
                for (int i = 0; i < count; i++)
                {
                    index--; // decrement index
                    if (index < 0)
                    {
                        index = Count - 1;
                    }
                }
            }
            else
            {
                index--; // decrement index
                if (index < 0)
                {
                    index = Count - 1;
                }
            }
            return index;
        }

        public static int GetNextCircular(int Count, int index, int? count = null)
        {
            if (count != null)
            {
                for (int i = 0; i < count; i++)
                {
                    index++; // increment index
                    index %= Count;
                }
            }
            else
            {
                index++; // increment index
                index %= Count;
            }
            return index;
        }
    }
}

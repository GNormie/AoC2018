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

            LinkedList<Player> lstPlayers = new LinkedList<Player>();
            for (int i = 1; i < numberOfPlayers + 1; i++)
                lstPlayers.AddLast(new Player(i));

            var currentPlayer = lstPlayers.First.Next.Next;
            //int currentPlayer = 3;

            LinkedListNode<int> leCurrentNode = marbleCircle.Last;
            for (int currentMarble = 2; currentMarble < marbles; currentMarble++)
            {
                var marble = currentMarble;

                if (marble % 23 != 0)
                {
                    var insertPosition = Util.GetNextCircular(leCurrentNode);
                    leCurrentNode = marbleCircle.AddAfter(insertPosition, currentMarble);
                }
                else
                {
                    var player = currentPlayer;//lstPlayers.SingleOrDefault(r => r.playerID == currentPlayer);
                    player.Value.score += marble;

                    var insertPosition = Util.GetPreviousCircular(leCurrentNode, 7);
                    player.Value.score += insertPosition.Value;
                    leCurrentNode = Util.GetNextCircular(insertPosition);
                    marbleCircle.Remove(insertPosition);
                }
                currentPlayer = Util.GetNextCircular(currentPlayer);//lstPlayers.ElementAt(GetNextCircular(lstPlayers.Count, currentPlayer - 1)).playerID;
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
            public long score;

            public Player(int _playerID)
            {
                playerID = _playerID;
            }
        }
    }
}

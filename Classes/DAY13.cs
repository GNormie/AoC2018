using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class DAY13
    {
        static int Ticks = 1;
        static List<Minecart> lstCarts = new List<Minecart>();
        static bool firstCrash = false;

        public static void Run(string path = null)
        {
            List<string> linesInput = File.ReadAllLines(Util.ReadFromInputFolder(13)).ToList();
            int Height = linesInput.Count();
            int Width = linesInput.OrderByDescending(r => r.Length).First().Length;

            int leCartID = 1;

            char[,] grid = new char[Width, Height];

            int j = 0;
            foreach (string line in linesInput)
            {
                for (int w = 0; w < line.Count(); w++)
                {
                    if (isCart(line[w]))
                    {
                        lstCarts.Add(new Minecart(leCartID, w, j, (Direction)line[w]));
                        if ((Direction)line[w] == Direction.Down || (Direction)line[w] == Direction.Up)
                            grid[w, j] = '|';
                        else
                            grid[w, j] = '-';
                        leCartID++;
                    }
                    else
                        grid[w, j] = line[w];
                }
                j++;
            }
            while (true)
            {
                foreach (Minecart cart in lstCarts.OrderBy(r => r.positionX))
                    cart.moveAhead(grid);
                Ticks++;
                if (lstCarts.Count == 1)
                {
                    //Console.WriteLine("LAST CART: [" + lstCarts.First().positionX + "][" + lstCarts.First().positionY + "]");
                    Console.WriteLine("PART 2: " + lstCarts.First().positionX + "," + lstCarts.First().positionY);
                    break;
                }
            }
        }

        public static bool isCart(char S)
        {
            if (S == 'v' || S == '>' || S == '^' || S == '<')
                return true;
            else
                return false;
        }

        public class Minecart
        {
            public int cartID = 0;

            public int positionX = 0;
            public int positionY = 0;

            public Direction currentDirection = Direction.Down;
            public Direction nextTurn = Direction.Left;

            public Minecart(int _cartID, int X, int Y, Direction currDir)
            {
                cartID = _cartID;
                positionX = X;
                positionY = Y;
                currentDirection = currDir;
            }

            public void moveAhead(char[,] grid)
            {
                if (currentDirection == Direction.Right)
                    positionX++;
                else if (currentDirection == Direction.Left)
                    positionX--;
                else if (currentDirection == Direction.Up)
                    positionY--;
                else if (currentDirection == Direction.Down)
                    positionY++;
                Logic(grid);
            }

            public void Logic(char[,] grid)
            {
                if ((lstCarts.Where(r => r.cartID != this.cartID).Any(r => r.positionX == this.positionX && r.positionY == this.positionY)))
                {
                    if (firstCrash == false)
                    {
                        //Console.WriteLine("CRASH: [" + positionX + "][" + positionY + "]");
                        Console.WriteLine("PART 1: " + positionX + "," + positionY);
                        firstCrash = true;
                    }
                    lstCarts.RemoveAll(r => r.positionX == this.positionX && r.positionY == this.positionY);
                }
                else if (grid[positionX, positionY] == '+')
                    this.intersectionTurn();
                else if (grid[positionX, positionY] == ' ')
                    Console.WriteLine("CART: " + cartID + "went offtrack somehow at [" + positionX + "][" + positionY + "] on turn:" + Ticks);
                else
                    Turn(grid[positionX, positionY]);
            }

            public void Turn(char S)
            {
                if (S == '/')
                {
                    if (currentDirection == Direction.Up)
                        currentDirection = Direction.Right;
                    else if (currentDirection == Direction.Left)
                        currentDirection = Direction.Down;
                    else if (currentDirection == Direction.Down)
                        currentDirection = Direction.Left;
                    else if (currentDirection == Direction.Right)
                        currentDirection = Direction.Up;
                }
                else if (S == '\\')
                {
                    if (currentDirection == Direction.Left)
                        currentDirection = Direction.Up;
                    else if (currentDirection == Direction.Down)
                        currentDirection = Direction.Right;
                    else if (currentDirection == Direction.Up)
                        currentDirection = Direction.Left;
                    else if (currentDirection == Direction.Right)
                        currentDirection = Direction.Down;
                }
            }

            public void intersectionTurn()
            {
                if (nextTurn == Direction.Left)
                {
                    if (currentDirection == Direction.Down)
                        currentDirection = Direction.Right;
                    else if (currentDirection == Direction.Right)
                        currentDirection = Direction.Up;
                    else if (currentDirection == Direction.Up)
                        currentDirection = Direction.Left;
                    else if (currentDirection == Direction.Left)
                        currentDirection = Direction.Down;
                    nextTurn = Direction.Down;
                }
                else if (nextTurn == Direction.Down || nextTurn == Direction.Up)
                {
                    nextTurn = Direction.Right;
                }
                else
                {
                    //RIGHT
                    if (currentDirection == Direction.Down)
                        currentDirection = Direction.Left;
                    else if (currentDirection == Direction.Right)
                        currentDirection = Direction.Down;
                    else if (currentDirection == Direction.Up)
                        currentDirection = Direction.Right;
                    else if (currentDirection == Direction.Left)
                        currentDirection = Direction.Up;
                    nextTurn = Direction.Left;
                }
            }
        }

        public enum Direction
        {
            Up = '^',
            Down = 'v',
            Left = '<',
            Right = '>'
        }

        public static void PrintMap(int width, int height, char[,] map)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Minecart prospectiveCart = lstCarts.SingleOrDefault(r => r.positionX == i && r.positionY == j);
                    if (prospectiveCart != null)
                        sb.Append((char)prospectiveCart.currentDirection);
                    else
                        sb.Append(map[i, j]);
                }
                sb.Append(Environment.NewLine);
            }
            Util.WriteToFile(sb);
        }

        public static void PrintMapConsole(int width, int height, char[,] map)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    Minecart prospectiveCart = lstCarts.SingleOrDefault(r => r.positionX == i && r.positionY == j);
                    if (prospectiveCart != null)
                        sb.Append((char)prospectiveCart.currentDirection);
                    else
                        sb.Append(map[i, j]);
                }
                sb.Append(Environment.NewLine);
            }
            Console.WriteLine(sb);
        }

    }
}

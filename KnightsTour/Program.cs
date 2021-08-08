using System;
using System.Collections.Generic;

namespace KnightsTour
{
    public class Program
    {
        private static readonly Random rnd = new();
        private static int width;
        private static int height;
        private static Cell[,] board;

        public class Cell
        {
            // Surrounding squares
            public Cell Top;
            public Cell Bottom;
            public Cell Left;
            public Cell Right;
            public Cell TopLeft;
            public Cell TopRight;
            public Cell BottomLeft;
            public Cell BottomRight;
            // Tour step number
            public int? step;

            public bool Tour(int step)
            {
                // Final step, solution found
                if ((this.step = step) == width * height)
                    return true;

                // Find all possible routes from current point
                List<Cell> possibleRoutes = new();
                if (Top != null)
                {
                    if (Top.TopLeft != null && Top.TopLeft.step == null)
                        possibleRoutes.Add(Top.TopLeft);
                    if (Top.TopRight != null && Top.TopRight.step == null)
                        possibleRoutes.Add(Top.TopRight);
                }
                if (Bottom != null)
                {
                    if (Bottom.BottomLeft != null && Bottom.BottomLeft.step == null)
                        possibleRoutes.Add(Bottom.BottomLeft);
                    if (Bottom.BottomRight != null && Bottom.BottomRight.step == null)
                        possibleRoutes.Add(Bottom.BottomRight);
                }
                if (Left != null)
                {
                    if (Left.TopLeft != null && Left.TopLeft.step == null)
                        possibleRoutes.Add(Left.TopLeft);
                    if (Left.BottomLeft != null && Left.BottomLeft.step == null)
                        possibleRoutes.Add(Left.BottomLeft);
                }
                if (Right != null)
                {
                    if (Right.TopRight != null && Right.TopRight.step == null)
                        possibleRoutes.Add(Right.TopRight);
                    if (Right.BottomRight != null && Right.BottomRight.step == null)
                        possibleRoutes.Add(Right.BottomRight);
                }

                // Construct random valid routes
                Cell[] routes = possibleRoutes.ToArray();
                for (int i = 0; i < routes.Length; i++)
                {
                    int rand = rnd.Next(0, routes.Length);
                    if (rand != i)
                    {
                        Cell ph = routes[rand];
                        routes[rand] = routes[i];
                        routes[i] = ph;
                    }
                }

                // Try each route
                for (int i = 0; i < routes.Length; i++)
                    if (routes[i].Tour(step + 1))
                        return true;

                // Route from this point fails
                this.step = null;
                return false;
            }
        }

        public static void Main(string[] _)
        {
            // Set puzzle size
            width = Input("Width of board: ");
            height = Input("Height of board: ");
            // Create the board
            board = new Cell[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    board[x, y] = new Cell();
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (y - 1 >= 0)
                        board[x, y].Top = board[x, y - 1];
                    if (y + 1 < height)
                        board[x, y].Bottom = board[x, y + 1];
                    if (x - 1 >= 0)
                        board[x, y].Left = board[x - 1, y];
                    if (x + 1 < width)
                        board[x, y].Right = board[x + 1, y];
                    if (x - 1 >= 0 && y - 1 >= 0)
                        board[x, y].TopLeft = board[x - 1, y - 1];
                    if (x + 1 < width && y - 1 >= 0)
                        board[x, y].TopRight = board[x + 1, y - 1];
                    if (x - 1 >= 0 && y + 1 < height)
                        board[x, y].BottomLeft = board[x - 1, y + 1];
                    if (x + 1 < width && y + 1 < height)
                        board[x, y].BottomRight = board[x + 1, y + 1];
                }
            // Begin the knights tour
            (int posX, int posY) = (Input("Starting X: "), Input("Starting Y: "));
            Console.WriteLine("Processing . . .");
            if (board[posX, posY].Tour(1))
            {
                Console.WriteLine($"Solution Found, starting at {posX}:{posY} with size {width}:{height}");
                Print();
            }
            else
                Console.WriteLine($"Not Possible, starting at {posX}:{posY} with size {width}:{height}");
        }

        private static int Input(string message)
        {
            Console.WriteLine(message);
            return int.Parse(Console.ReadLine());
        }

        private static void Print()
        {
            int newHeight = height - 1;
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < width; x++)
                    if (board[x, y].step == null)
                        Console.Write($"    ");
                    else
                        Console.Write($"  {board[x, y].step.ToString().PadLeft(2, '0')}");
                Console.WriteLine("\n");
            }
            for (int x = 0; x < width; x++)
                if (board[x, newHeight].step == null)
                    Console.Write($"    ");
                else
                    Console.Write($"  {board[x, newHeight].step.ToString().PadLeft(2, '0')}");
            Console.WriteLine("\n" + string.Empty.PadLeft(4 * width, '='));
        }
    }
}
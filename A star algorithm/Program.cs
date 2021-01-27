using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_star_algorithm
{
    public class Node
    {
        public int total_value;//F
        public int distance_to_start;//G
        public int hueristic;//H
        public bool blocked;//Is the node blocked by an obstacle
        public Node(bool Blocked) { blocked = Blocked; }
    }
    public class Position 
    {
        public int X;
        public int Y;
        public Position(int x, int y) { X = x;Y = y;}
    
    } //Position of nodes
    class Program
    {
       static void Main(string[] args)
        {
            Node[,] grid = new Node[10,10]; //Makes a blank grid (2D array of Nodes)
            for(int i = 0; i <= grid.GetLength(0) - 1; i++)
            {
                for (int k = 0; k <= grid.GetLength(1) - 1; k++) { grid[i, k] = new Node(false); }
            }
            Position start = new Position(0, 4);
            Position end = new Position(5, 6);
            List<Position> path = AStar(grid,start,end);

        }

        public static List<Position> FindNeighbours(Position last_position, Node[,] grid)
        {
            List<Position> neighbour_nodes = new List<Position> { };
            int Maximum_X_Position = grid.GetLength(0);
            int Maximum_Y_Position = grid.GetLength(1);
            int x = -1;
            int y = -1;
            while (x < 2) //For all possible X neigbours
            {
                if (last_position.X + x <= Maximum_X_Position & last_position.X + x >= 0)//If X value falls in grid
                {
                    while (y < 2)//For all possible y values
                    {
                        if (last_position.Y + y <= Maximum_X_Position & last_position.Y + y >= 0)//If Y value falls in the grid
                        {
                            if (grid[last_position.X + x, last_position.Y + y].blocked == false) { neighbour_nodes.Add(new Position(last_position.X + x, last_position.Y + y)); }//If it's not blocked
                            y = y + 1;
                        }
                    }
                    y = -1; // reset why for next x value
                }

                x = x + 1;
            }
            return neighbour_nodes;
        }

        public static List<Position> AStar(Node[,] grid, Position StartingPosition, Position EndingPosition)
        {
            List<Position> followed_path = new List<Position> {StartingPosition};// Positiion of nodes along path
            bool finished_search = false;
            while (finished_search == false)
            {
                List<Position> neighbour_nodes = new List<Position> { };
                Position last_position = followed_path[followed_path.Count - 1];

                //Finding Neighbours Below
               neighbour_nodes =  FindNeighbours(last_position,grid);
               neighbour_nodes.Remove(last_position);

            }
            return followed_path;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_star_algorithm
{
    public class Node
    {
        public double total_value = 0;//F
        public double distance_to_start = 0;//G
        public double hueristic = 0;//H
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
                for (int k = 0; k <= grid.GetLength(1) - 1; k++) { grid[i, k] = new Node(false); } //Populare grid
            }
            Position start = new Position(0, 4);
            Position end = new Position(5, 6);
            List<Position> path = AStar(grid,start,end);
            Console.WriteLine(path);
            Console.Read();

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
                            if (grid[last_position.X + x, last_position.Y + y].blocked == false) { neighbour_nodes.Add(new Position(last_position.X + x, last_position.Y + y)); }//If it's not blocked the add
                            y = y + 1; //Iterate Y
                        }
                    }
                    y = -1; // reset why for next x value
                }

                x = x + 1;
            }
            int counter = 0;
            while( counter < neighbour_nodes.Count)
            {
                Position neighbour = neighbour_nodes[counter];
                if (neighbour.X == last_position.X & neighbour.Y == last_position.Y) 
                {
                    neighbour_nodes.RemoveAt(counter);
                    break;
                }
                counter += 1;
            }
            return neighbour_nodes;
        }

        public static List<Position> AStar(Node[,] grid, Position StartingPosition, Position EndingPosition)
        {
            List<Position> followed_path = new List<Position> {StartingPosition};// Positiion of nodes along path. Initialises w/ StartingPosition
            bool finished_search = false;
            while (finished_search == false)
            {
                List<Position> neighbour_nodes = new List<Position> { };
                Position last_position = followed_path[followed_path.Count - 1]; //Gets Last position on path

                //Finding Neighbours Below
                neighbour_nodes =  FindNeighbours(last_position,grid);// Finds all the neigbours of a node
              
               foreach (Position neighbour in neighbour_nodes) { grid[neighbour.X,neighbour.Y].distance_to_start = grid[last_position.X, last_position.Y].distance_to_start + 1;}//Updates position from start
               foreach (Position neighbour in neighbour_nodes) 
                {
                    Node node = grid[neighbour.X, neighbour.Y];
                    node.hueristic = Math.Sqrt(Math.Pow((neighbour.X - EndingPosition.X), 2) + Math.Pow((neighbour.Y - EndingPosition.Y), 2)); //Pythagurus theorem as Hueristic
                    node.total_value = node.distance_to_start + node.hueristic; //Calculates total value
                }
                double minimum_value = grid[neighbour_nodes[0].X, neighbour_nodes[0].Y].total_value; //Records lowest total value. Initialises with 1st value so as to not be null
                Position minimum_value_flag = neighbour_nodes[0];//Is the Position of the lowest value. Initialises at 1st value incase minimum is the 1st value
                foreach (Position neighbour in neighbour_nodes) 
                {
                     if (minimum_value == Math.Min(minimum_value, grid[neighbour.X, neighbour.Y].total_value)){ } //If smallest between minimum value and the grid @ position neighbour then is minum value do nothing
                     else { minimum_value_flag = neighbour;minimum_value = grid[neighbour.X, neighbour.Y].total_value; }//else update minimum_value and the position of lowest value
                }
                followed_path.Add(minimum_value_flag); //Add the lowest to the followed path
                if (minimum_value_flag.X == EndingPosition.X & minimum_value_flag.Y == EndingPosition.Y) 
                { 
                    finished_search = true; } //Breal loop if path has finished
            }
           
            return followed_path;//Returns Path
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
namespace A_star_algorithm
{
    public class Node
    {
        public double total_value = 0;
        public double distance_to_start = 0;
        public double hueristic = 0;
        public double NodeValue;
        public bool blocked;//Is the node blocked by an obstacle
        public Node(bool Blocked, double node_value) { blocked = Blocked;NodeValue = node_value; }
    }
    public class Position //Position of nodes
    {
        public int X;//X coordinate
        public int Y;//Y coordinate
        public Position(int x, int y) { X = x;Y = y;}//Initialiser
    
    } 
    class Program
    {
       static void Main(string[] args)
        {
            Node[,] grid = new Node[10,10]; //Makes a blank grid (2D array of Nodes)
            var GRID = A_star_algorithm.Properties.Resources.Grid; //Open grid from text file
            Position[] InitialPositions = CreateGrid(GRID, grid);//Gets Start & End positions from textfile
            //^Also sets up grid
            Position start = InitialPositions[0];
            Position end = InitialPositions[1];
            List<Position> path = AStar(grid,start,end);//Runs the Astar function to find and return path
            OutputGrid(grid,start,end,path); //Output grid
            //Key is below
            Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("Green is the start position");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("Yellow is a step taken");
            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Red is the end position");
            Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine("Magenta is an obstacle");
            Console.Read();//Pause so user can read before Application closes

        }
        public static Position[] CreateGrid(string GRID, Node[,] grid) //Creates Grid & Return Start & End positions in Array
        {
            Position[] StartEndPositions = new Position[2];//Array to return
            string[] Rows = GRID.Split('\r');//Split by lines
            int row_number = 0;
            int column_number = 0;
            while (row_number < Rows.GetLength(0)) //For every row
            {
                string row = Rows[row_number]; 
                string Row = Regex.Replace(row, @"\n", "");//Remove any new line charecters left (.Replace didn't work)
                String[] Columns = Row.Split(','); //Split by comma for each cell
                while (column_number < Columns.Length) //Every column
                {
  
                    if (int.TryParse(Columns[column_number], out int value) == true) { grid[column_number, row_number] = new Node(false, value); }//Normal node
                    if (Columns[column_number] == "O") { grid[column_number, row_number] = new Node(true,0); }//Obstacle node
                    if (Columns[column_number] == "S") { grid[column_number, row_number] = new Node(false,0);StartEndPositions[0] = new Position(column_number, row_number); }
                    //^Start node
                    if (Columns[column_number] == "E") { grid[column_number, row_number] = new Node(false,0); StartEndPositions[1] = new Position(column_number, row_number); }
                    //^End node
                    column_number += 1;
                }
                row_number += 1;
                column_number = 0;
            }
            return StartEndPositions; //returns positions
        }
        public static void OutputGrid(Node[,] grid,Position start,Position end, List<Position> path) 
        {
            int x_count = 0;
            int y_vount = 0;
            while(y_vount < grid.GetLength(1))//For every row
            {
                while(x_count < grid.GetLength(0))
                {
                    foreach(Position step in path) {if(x_count == step.X & y_vount == step.Y) { Console.ForegroundColor = ConsoleColor.Yellow; } } //Path = yellow
                    if (x_count == start.X & y_vount == start.Y) { Console.ForegroundColor = ConsoleColor.Green;} //Start = green
                    if (x_count == end.X & y_vount == end.Y) { Console.ForegroundColor = ConsoleColor.Red; }//end = yellow
                    if (grid[x_count, y_vount].blocked == true) { Console.ForegroundColor = ConsoleColor.Magenta; }//obstacle = magenta
                    Console.Write(" " + grid[x_count, y_vount].NodeValue.ToString() + " ");//Charechter to insert to make grid
                    x_count += 1;
                    Console.ForegroundColor = ConsoleColor.White; //Normal node is white
                }
                Console.WriteLine();//New line
                x_count = 0;
                y_vount += 1;
            }
        }
        public static List<Position> FindNeighbours(Position last_position, Node[,] grid)
        {
            List<Position> neighbour_nodes = new List<Position> { }; //List to return
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
                    y = -1; // reset y for next x value
                }

                x = x + 1;
            }
            //Code below removes the orignal value from List (.Remove(last_position) didn't work because C# can't compare two custom classes to compare)
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
                if (minimum_value_flag.X == EndingPosition.X & minimum_value_flag.Y == EndingPosition.Y) { finished_search = true; } //Breal loop if path has finished
            }
           
            return followed_path;//Returns Path
        }
    }

}

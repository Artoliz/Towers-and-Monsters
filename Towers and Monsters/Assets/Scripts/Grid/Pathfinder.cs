using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private int COL;
    private int ROW;

    private int[,] grid;

    private Pair<int, int> src;
    private Pair<int, int> dest;

    private struct Cell
    {
        public int parent_i;
        public int parent_j;

        public double f;
        public double g;
        public double h;
    };

    public Pathfinder(int width, int height)
    {
        COL = width;
        ROW = height;
        grid = new int[ROW, COL];
        for (int i = 0; i < COL; i++)
            for (int j = 0; j < ROW; j++)
                grid[j, i] = 1;
    }

    public int[,] GetGrid()
    {
        return grid;
    }

    public void SetGrid(int[,] newGrid)
    {
        grid = newGrid;
    }

    public void SetBlockedPosition(int row, int col)
    {
        if (IsValid(row, col))
            grid[row, col] = 0;
    }

    public void RemoveBlockedPosition(int row, int col)
    {
        if (IsValid(row, col))
            grid[row, col] = 1;
    }

    public void SetSource(int row, int col)
    {
        src = new Pair<int, int>(row, col);
    }

    public void SetDestination(int row, int col)
    {
        dest = new Pair<int, int>(row, col);
    }

    private bool IsValid(int row, int col)
    {
        return (row >= 0) && (row < ROW) &&
          (col >= 0) && (col < COL);
    }

    private bool IsUnBlocked(int[,] grid, int row, int col)
    {
        if (grid[row, col] == 1)
            return (true);
        else
            return (false);
    }

    private bool IsBlocked(int row, int col)
    {
        if ((row >= 0) && (row < ROW) && (col >= 0) && (col < COL))
        {
            if (grid[row, col] == 0)
                return true;
            return false;
        }
        return false;
    }

    private bool IsDestination(int row, int col, Pair<int, int> dest)
    {
        return row == dest.First && col == dest.Second;
    }

    private double CalculateHValue(int row, int col, Pair<int, int> dest)
    {
        return Mathf.Sqrt((row - dest.First) * (row - dest.First) + (col - dest.Second) * (col - dest.Second));
    }

    private Stack<Pair<int, int>> TracePath(Cell[,] cellDetails, Pair<int, int> dest)
    {
        int row = dest.First;
        int col = dest.Second;

        Stack<Pair<int, int>> Path = new Stack<Pair<int, int>>();

        while (!(cellDetails[row, col].parent_i == row
                 && cellDetails[row, col].parent_j == col))
        {
            Path.Push(new Pair<int, int>(row, col));
            int temp_row = cellDetails[row, col].parent_i;
            int temp_col = cellDetails[row, col].parent_j;
            row = temp_row;
            col = temp_col;
        }
        Path.Push(new Pair<int, int>(row, col));

        return  Path;
    }

    public Stack<Pair<int, int>> Search()
    {
        if (IsValid(src.First, src.Second) == false)
        {
            Debug.Log("Source: " + src.First + "; " + src.Second);
            Debug.Log("Source is invalid\n");
            return null;
        }

        if (IsValid(dest.First, dest.Second) == false)
        {
            Debug.Log("Destination is invalid\n");
            return null;
        }

        if (IsUnBlocked(grid, src.First, src.Second) == false ||
                IsUnBlocked(grid, dest.First, dest.Second) == false)
        {
            Debug.Log("Source or the destination is blocked\n");
            return null;
        }

        if (IsDestination(src.First, src.Second, dest) == true)
        {
            Debug.Log("We are already at the destination\n");
            return null;
        }

        bool[,] closedList = new bool[ROW, COL];

        Cell[,] cellDetails = new Cell[ROW, COL];

        int i, j;

        for (i = 0; i < ROW; i++)
        {
            for (j = 0; j < COL; j++)
            {
                cellDetails[i, j].f = double.MaxValue;
                cellDetails[i, j].g = double.MaxValue;
                cellDetails[i, j].h = double.MaxValue;
                cellDetails[i, j].parent_i = -1;
                cellDetails[i, j].parent_j = -1;
            }
        }

        i = src.First;
        j = src.Second;
        cellDetails[i, j].f = 0.0f;
        cellDetails[i, j].g = 0.0f;
        cellDetails[i, j].h = 0.0f;
        cellDetails[i, j].parent_i = i;
        cellDetails[i, j].parent_j = j;

        List<Pair<double, Pair<int, int>>> openList = new List<Pair<double, Pair<int, int>>>();

        openList.Add(new Pair<double, Pair<int, int>>(0.0f, new Pair<int, int>(i, j)));

        while (openList.Count != 0)
        {
            Pair<double, Pair<int, int>> p = openList[0];

            openList.Remove(openList[0]);

            i = p.Second.First;
            j = p.Second.Second;
            closedList[i, j] = true;

            double gNew, hNew, fNew;

            //-----------(North) ------------
            if (IsValid(i - 1, j) == true)
            {
                if (IsDestination(i - 1, j, dest) == true)
                {
                    cellDetails[i - 1, j].parent_i = i;
                    cellDetails[i - 1, j].parent_j = j;
                    return TracePath(cellDetails, dest);
                }
                else if (closedList[i - 1, j] == false && IsUnBlocked(grid, i - 1, j) == true)
                {
                    gNew = cellDetails[i, j].g + 1.0;
                    hNew = CalculateHValue(i - 1, j, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i - 1, j].f == double.MaxValue ||
                            cellDetails[i - 1, j].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew, new Pair<int, int>(i - 1, j)));

                        cellDetails[i - 1, j].f = fNew;
                        cellDetails[i - 1, j].g = gNew;
                        cellDetails[i - 1, j].h = hNew;
                        cellDetails[i - 1, j].parent_i = i;
                        cellDetails[i - 1, j].parent_j = j;
                    }
                }
            }

            //-----------(South) ------------
            if (IsValid(i + 1, j) == true)
            {
                if (IsDestination(i + 1, j, dest) == true)
                {
                    cellDetails[i + 1, j].parent_i = i;
                    cellDetails[i + 1, j].parent_j = j;
                    return TracePath(cellDetails, dest);
                }
                else if (closedList[i + 1, j] == false &&
                         IsUnBlocked(grid, i + 1, j) == true)
                {
                    gNew = cellDetails[i, j].g + 1.0;
                    hNew = CalculateHValue(i + 1, j, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i + 1, j].f == double.MaxValue ||
                            cellDetails[i + 1, j].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew, new Pair<int, int>(i + 1, j)));
                        cellDetails[i + 1, j].f = fNew;
                        cellDetails[i + 1, j].g = gNew;
                        cellDetails[i + 1, j].h = hNew;
                        cellDetails[i + 1, j].parent_i = i;
                        cellDetails[i + 1, j].parent_j = j;
                    }
                }
            }

            //-----------(East) ------------
            if (IsValid(i, j + 1) == true)
            {
                if (IsDestination(i, j + 1, dest) == true)
                {
                    cellDetails[i, j + 1].parent_i = i;
                    cellDetails[i, j + 1].parent_j = j;
                    return TracePath(cellDetails, dest);
                }

                else if (closedList[i, j + 1] == false &&
                         IsUnBlocked(grid, i, j + 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.0;
                    hNew = CalculateHValue(i, j + 1, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i, j + 1].f == double.MaxValue ||
                            cellDetails[i, j + 1].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew,
                                           new Pair<int, int>(i, j + 1)));

                        cellDetails[i, j + 1].f = fNew;
                        cellDetails[i, j + 1].g = gNew;
                        cellDetails[i, j + 1].h = hNew;
                        cellDetails[i, j + 1].parent_i = i;
                        cellDetails[i, j + 1].parent_j = j;
                    }
                }
            }

            //-----------(West) ------------
            if (IsValid(i, j - 1) == true)
            {
                if (IsDestination(i, j - 1, dest) == true)
                {
                    cellDetails[i, j - 1].parent_i = i;
                    cellDetails[i, j - 1].parent_j = j;
                    return TracePath(cellDetails, dest);
                }

                else if (closedList[i, j - 1] == false &&
                         IsUnBlocked(grid, i, j - 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.0;
                    hNew = CalculateHValue(i, j - 1, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i, j - 1].f == double.MaxValue ||
                            cellDetails[i, j - 1].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew, new Pair<int, int>(i, j - 1)));

                        cellDetails[i, j - 1].f = fNew;
                        cellDetails[i, j - 1].g = gNew;
                        cellDetails[i, j - 1].h = hNew;
                        cellDetails[i, j - 1].parent_i = i;
                        cellDetails[i, j - 1].parent_j = j;
                    }
                }
            }

            //-----------(North-East) ------------
            if (IsValid(i - 1, j + 1) == true && !IsBlocked(i - 1, j) && !IsBlocked(i, j + 1))
            {
                if (IsDestination(i - 1, j + 1, dest) == true)
                { 
                    cellDetails[i - 1, j + 1].parent_i = i;
                    cellDetails[i - 1, j + 1].parent_j = j;
                    return TracePath(cellDetails, dest);
                }

                else if (closedList[i - 1, j + 1] == false &&
                         IsUnBlocked(grid, i - 1, j + 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.414;
                    hNew = CalculateHValue(i - 1, j + 1, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i - 1, j + 1].f == double.MaxValue ||
                            cellDetails[i - 1, j + 1].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew,
                                       new Pair<int, int>(i - 1, j + 1)));

                        cellDetails[i - 1, j + 1].f = fNew;
                        cellDetails[i - 1, j + 1].g = gNew;
                        cellDetails[i - 1, j + 1].h = hNew;
                        cellDetails[i - 1, j + 1].parent_i = i;
                        cellDetails[i - 1, j + 1].parent_j = j;
                    }
                }
            }

           //-----------(North-West) ------------
            if (IsValid(i - 1, j - 1) == true && !IsBlocked(i - 1, j) && !IsBlocked(i, j - 1))
            {
                if (IsDestination(i - 1, j - 1, dest) == true)
                {
                    cellDetails[i - 1, j - 1].parent_i = i;
                    cellDetails[i - 1, j - 1].parent_j = j;
                    return TracePath(cellDetails, dest);
                }
                else if (closedList[i - 1, j - 1] == false &&
                         IsUnBlocked(grid, i - 1, j - 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.414;
                    hNew = CalculateHValue(i - 1, j - 1, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i - 1, j - 1].f == double.MaxValue ||
                            cellDetails[i - 1, j - 1].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew, new Pair<int, int>(i - 1, j - 1)));
                        cellDetails[i - 1, j - 1].f = fNew;
                        cellDetails[i - 1, j - 1].g = gNew;
                        cellDetails[i - 1, j - 1].h = hNew;
                        cellDetails[i - 1, j - 1].parent_i = i;
                        cellDetails[i - 1, j - 1].parent_j = j;
                    }
                }
            }

           //-----------(South-East) ------------
            if (IsValid(i + 1, j + 1) == true && !IsBlocked(i + 1, j) && !IsBlocked(i , j + 1))
            {
                if (IsDestination(i + 1, j + 1, dest) == true)
                {
                    cellDetails[i + 1, j + 1].parent_i = i;
                    cellDetails[i + 1, j + 1].parent_j = j;
                    return TracePath(cellDetails, dest);
                }
                else if (closedList[i + 1, j + 1] == false &&
                         IsUnBlocked(grid, i + 1, j + 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.414;
                    hNew = CalculateHValue(i + 1, j + 1, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i + 1, j + 1].f == double.MaxValue ||
                            cellDetails[i + 1, j + 1].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew,
                                            new Pair<int, int>(i + 1, j + 1)));

                        cellDetails[i + 1, j + 1].f = fNew;
                        cellDetails[i + 1, j + 1].g = gNew;
                        cellDetails[i + 1, j + 1].h = hNew;
                        cellDetails[i + 1, j + 1].parent_i = i;
                        cellDetails[i + 1, j + 1].parent_j = j;
                    }
                }
            }

           //-----------(South-West) ------------
            if (IsValid(i + 1, j - 1) == true && !IsBlocked(i + 1, j) && !IsBlocked(i, j - 1))
            {
                if (IsDestination(i + 1, j - 1, dest) == true)
                {
                    cellDetails[i + 1, j - 1].parent_i = i;
                    cellDetails[i + 1, j - 1].parent_j = j;
                    return TracePath(cellDetails, dest);
                }
                else if (closedList[i + 1, j - 1] == false &&
                         IsUnBlocked(grid, i + 1, j - 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.414;
                    hNew = CalculateHValue(i + 1, j - 1, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i + 1, j - 1].f == double.MaxValue ||
                            cellDetails[i + 1, j - 1].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew,
                                            new Pair<int, int>(i + 1, j - 1)));

                        cellDetails[i + 1, j - 1].f = fNew;
                        cellDetails[i + 1, j - 1].g = gNew;
                        cellDetails[i + 1, j - 1].h = hNew;
                        cellDetails[i + 1, j - 1].parent_i = i;
                        cellDetails[i + 1, j - 1].parent_j = j;
                    }
                }
            }
        }
        return null;
    }

    public bool AStarSearch()
    {
        // If the source is out of range 
        if (IsValid(src.First, src.Second) == false)
        {
            Debug.Log("Source is invalid\n");
            return false;
        }

        // If the destination is out of range 
        if (IsValid(dest.First, dest.Second) == false)
        {
            Debug.Log("Destination is invalid\n");
            return false;
        }

        // Either the source or the destination is blocked 
        if (IsUnBlocked(grid, src.First, src.Second) == false ||
                IsUnBlocked(grid, dest.First, dest.Second) == false)
        {
            Debug.Log("Source or the destination is blocked\n");
            return false;
        }

        // If the destination cell is the same as source cell 
        if (IsDestination(src.First, src.Second, dest) == true)
        {
            Debug.Log("We are already at the destination\n");
            return false;
        }

        // Create a closed list and initialise it to false which means 
        // that no cell has been included yet 
        // This closed list is implemented as a boolean 2D array 
        bool [,] closedList = new bool[ROW, COL];

        // Declare a 2D array of structure to hold the details 
        //of that cell 
        Cell[,] cellDetails = new Cell[ROW, COL]; 
  
        int i, j; 
  
        for (i=0; i<ROW; i++) 
        { 
            for (j=0; j<COL; j++) 
            { 
                cellDetails[i, j].f = double.MaxValue; 
                cellDetails[i, j].g = double.MaxValue; 
                cellDetails[i, j].h = double.MaxValue; 
                cellDetails[i, j].parent_i = -1; 
                cellDetails[i, j].parent_j = -1; 
            } 
        }

        // Initialising the parameters of the starting node 
        i = src.First;
        j = src.Second; 
        cellDetails[i, j].f = 0.0f; 
        cellDetails[i, j].g = 0.0f; 
        cellDetails[i, j].h = 0.0f; 
        cellDetails[i, j].parent_i = i; 
        cellDetails[i, j].parent_j = j;

        /* 
         Create an open list having information as- 
         <f, <i, j>> 
         where f = g + h, 
         and i, j are the row and column index of that cell 
         Note that 0 <= i <= ROW-1 & 0 <= j <= COL-1 
         This open list is implenented as a set of pair of pair.*/
        List<Pair<double, Pair<int, int>>> openList = new List<Pair<double, Pair<int, int>>>();

        // Put the starting cell on the open list and set its 
        // 'f' as 0
        openList.Add(new Pair<double, Pair<int, int>>(0.0f, new Pair<int, int>(i, j))); 
  
        while (openList.Count != 0) 
        {
            Pair<double, Pair<int, int>> p = openList[0];

            // Remove this vertex from the open list 
            openList.Remove(openList[0]);
  
            // Add this vertex to the closed list 
            i = p.Second.First; 
            j = p.Second.Second; 
            closedList[i, j] = true; 
       
           /* 
            Generating all the 8 successor of this cell 
  
                N.W   N   N.E 
                  \   |   / 
                   \  |  / 
                W----Cell----E 
                     / | \ 
                   /   |  \ 
                S.W    S   S.E 
  
            Cell-->Popped Cell (i, j) 
            N -->  North       (i-1, j) 
            S -->  South       (i+1, j) 
            E -->  East        (i, j+1) 
            W -->  West           (i, j-1) 
            N.E--> North-East  (i-1, j+1) 
            N.W--> North-West  (i-1, j-1) 
            S.E--> South-East  (i+1, j+1) 
            S.W--> South-West  (i+1, j-1)*/
  
            // To store the 'g', 'h' and 'f' of the 8 successors 
            double gNew, hNew, fNew;

            //-----------(North) ------------
            if (IsValid(i - 1, j) == true)
            {
                if (IsDestination(i - 1, j, dest) == true)
                {
                    cellDetails[i - 1, j].parent_i = i;
                    cellDetails[i - 1, j].parent_j = j;
                    return true;
                }
                else if (closedList[i - 1, j] == false && IsUnBlocked(grid, i - 1, j) == true)
                {
                    gNew = cellDetails[i, j].g + 1.0;
                    hNew = CalculateHValue(i - 1, j, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i - 1, j].f == double.MaxValue ||
                            cellDetails[i - 1, j].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew, new Pair<int, int>(i - 1, j)));

                        cellDetails[i - 1, j].f = fNew;
                        cellDetails[i - 1, j].g = gNew;
                        cellDetails[i - 1, j].h = hNew;
                        cellDetails[i - 1, j].parent_i = i;
                        cellDetails[i - 1, j].parent_j = j;
                    }
                }
            }

            //-----------(South) ------------
            if (IsValid(i + 1, j) == true)
            {
                if (IsDestination(i + 1, j, dest) == true)
                {
                    cellDetails[i + 1, j].parent_i = i;
                    cellDetails[i + 1, j].parent_j = j;
                    return true;
                }
                else if (closedList[i + 1, j] == false &&
                         IsUnBlocked(grid, i + 1, j) == true)
                {
                    gNew = cellDetails[i, j].g + 1.0;
                    hNew = CalculateHValue(i + 1, j, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i + 1, j].f == double.MaxValue ||
                            cellDetails[i + 1, j].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew, new Pair<int, int>(i + 1, j)));
                        cellDetails[i + 1, j].f = fNew;
                        cellDetails[i + 1, j].g = gNew;
                        cellDetails[i + 1, j].h = hNew;
                        cellDetails[i + 1, j].parent_i = i;
                        cellDetails[i + 1, j].parent_j = j;
                    }
                }
            }

            //-----------(East) ------------
            if (IsValid(i, j + 1) == true)
            {
                if (IsDestination(i, j + 1, dest) == true)
                {
                    cellDetails[i, j + 1].parent_i = i;
                    cellDetails[i, j + 1].parent_j = j;
                    return true;
                }

                else if (closedList[i, j + 1] == false &&
                         IsUnBlocked(grid, i, j + 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.0;
                    hNew = CalculateHValue(i, j + 1, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i, j + 1].f == double.MaxValue ||
                            cellDetails[i, j + 1].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew,
                                           new Pair<int, int>(i, j + 1)));

                        cellDetails[i, j + 1].f = fNew;
                        cellDetails[i, j + 1].g = gNew;
                        cellDetails[i, j + 1].h = hNew;
                        cellDetails[i, j + 1].parent_i = i;
                        cellDetails[i, j + 1].parent_j = j;
                    }
                }
            }

            //-----------(West) ------------
            if (IsValid(i, j - 1) == true)
            {
                if (IsDestination(i, j - 1, dest) == true)
                {
                    cellDetails[i, j - 1].parent_i = i;
                    cellDetails[i, j - 1].parent_j = j;
                    return true;
                }

                else if (closedList[i, j - 1] == false &&
                         IsUnBlocked(grid, i, j - 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.0;
                    hNew = CalculateHValue(i, j - 1, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i, j - 1].f == double.MaxValue ||
                            cellDetails[i, j - 1].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew, new Pair<int, int>(i, j - 1)));

                        cellDetails[i, j - 1].f = fNew;
                        cellDetails[i, j - 1].g = gNew;
                        cellDetails[i, j - 1].h = hNew;
                        cellDetails[i, j - 1].parent_i = i;
                        cellDetails[i, j - 1].parent_j = j;
                    }
                }
            }

            //-----------(North-East) ------------
            if (IsValid(i - 1, j + 1) == true && !IsBlocked(i - 1, j) && !IsBlocked(i, j + 1))
            {
                if (IsDestination(i - 1, j + 1, dest) == true)
                {
                    cellDetails[i - 1, j + 1].parent_i = i;
                    cellDetails[i - 1, j + 1].parent_j = j;
                    return true;
                }

                else if (closedList[i - 1, j + 1] == false &&
                         IsUnBlocked(grid, i - 1, j + 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.414;
                    hNew = CalculateHValue(i - 1, j + 1, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i - 1, j + 1].f == double.MaxValue ||
                            cellDetails[i - 1, j + 1].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew,
                                       new Pair<int, int>(i - 1, j + 1)));

                        cellDetails[i - 1, j + 1].f = fNew;
                        cellDetails[i - 1, j + 1].g = gNew;
                        cellDetails[i - 1, j + 1].h = hNew;
                        cellDetails[i - 1, j + 1].parent_i = i;
                        cellDetails[i - 1, j + 1].parent_j = j;
                    }
                }
            }

            //-----------(North-West) ------------
            if (IsValid(i - 1, j - 1) == true && !IsBlocked(i - 1, j) && !IsBlocked(i, j - 1))
            {
                if (IsDestination(i - 1, j - 1, dest) == true)
                {
                    cellDetails[i - 1, j - 1].parent_i = i;
                    cellDetails[i - 1, j - 1].parent_j = j;
                    return true;
                }
                else if (closedList[i - 1, j - 1] == false &&
                         IsUnBlocked(grid, i - 1, j - 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.414;
                    hNew = CalculateHValue(i - 1, j - 1, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i - 1, j - 1].f == double.MaxValue ||
                            cellDetails[i - 1, j - 1].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew, new Pair<int, int>(i - 1, j - 1)));
                        cellDetails[i - 1, j - 1].f = fNew;
                        cellDetails[i - 1, j - 1].g = gNew;
                        cellDetails[i - 1, j - 1].h = hNew;
                        cellDetails[i - 1, j - 1].parent_i = i;
                        cellDetails[i - 1, j - 1].parent_j = j;
                    }
                }
            }

            //-----------(South-East) ------------
            if (IsValid(i + 1, j + 1) == true && !IsBlocked(i + 1, j) && !IsBlocked(i, j + 1))
            {
                if (IsDestination(i + 1, j + 1, dest) == true)
                {
                    cellDetails[i + 1, j + 1].parent_i = i;
                    cellDetails[i + 1, j + 1].parent_j = j;
                    return true;
                }
                else if (closedList[i + 1, j + 1] == false &&
                         IsUnBlocked(grid, i + 1, j + 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.414;
                    hNew = CalculateHValue(i + 1, j + 1, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i + 1, j + 1].f == double.MaxValue ||
                            cellDetails[i + 1, j + 1].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew,
                                            new Pair<int, int>(i + 1, j + 1)));

                        cellDetails[i + 1, j + 1].f = fNew;
                        cellDetails[i + 1, j + 1].g = gNew;
                        cellDetails[i + 1, j + 1].h = hNew;
                        cellDetails[i + 1, j + 1].parent_i = i;
                        cellDetails[i + 1, j + 1].parent_j = j;
                    }
                }
            }

            //-----------(South-West) ------------
            if (IsValid(i + 1, j - 1) == true && !IsBlocked(i + 1, j) && !IsBlocked(i, j - 1))
            {
                if (IsDestination(i + 1, j - 1, dest) == true)
                {
                    cellDetails[i + 1, j - 1].parent_i = i;
                    cellDetails[i + 1, j - 1].parent_j = j;
                    return true;
                }
                else if (closedList[i + 1, j - 1] == false &&
                         IsUnBlocked(grid, i + 1, j - 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.414;
                    hNew = CalculateHValue(i + 1, j - 1, dest);
                    fNew = gNew + hNew;

                    if (cellDetails[i + 1, j - 1].f == double.MaxValue ||
                            cellDetails[i + 1, j - 1].f > fNew)
                    {
                        openList.Add(new Pair<double, Pair<int, int>>(fNew,
                                            new Pair<int, int>(i + 1, j - 1)));

                        cellDetails[i + 1, j - 1].f = fNew;
                        cellDetails[i + 1, j - 1].g = gNew;
                        cellDetails[i + 1, j - 1].h = hNew;
                        cellDetails[i + 1, j - 1].parent_i = i;
                        cellDetails[i + 1, j - 1].parent_j = j;
                    }
                }
            }
        } 
  
        return false; 
    } 
}

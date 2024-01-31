using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AStar
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    public Spot[,] Spots;
    public AStar(Vector3Int[,] grid, int columns, int rows)
    {
        Spots = new Spot[columns, rows];
    }
    private bool IsValidPath(Vector3Int[,] grid, Spot start, Spot end)
    {
        if (end == null)
            return false;
        if (start == null)
            return false;
        if (end.Height >= 1)
            return false;
        return true;
    }
    public List<Spot> CreatePath(Vector3Int[,] grid, Vector2Int _start, Vector2Int target, float _distance, int length)
    {                                       // target is position that need to run away
        Spot spotEnd = null;
        Spot spotStart = null;
        var columns = Spots.GetUpperBound(0) + 1;
        var rows = Spots.GetUpperBound(1) + 1;
        Spots = new Spot[columns, rows];

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Spots[i, j] = new Spot(grid[i, j].x, grid[i, j].y, grid[i, j].z);// grid[i, j].z is height to know has tilemap or not
            }
        }

        for (int i = 0; i < columns; i++)  // add neighboor for each grid on walkable tilemap
        {
            for (int j = 0; j < rows; j++)
            {
                Spots[i, j].AddNeighbors(Spots, i, j);
                if (Spots[i, j].X == _start.x && Spots[i, j].Y == _start.y)
                    spotStart = Spots[i, j];
                
            }
        }
        
        List<Spot> OpenSet = new List<Spot>();//contains nodes that are candidates for exploration
        List<Spot> ClosedSet = new List<Spot>();//contains nodes that have already been explored
        spotStart.G = 0;
        spotStart.H = -CalculateHValue(spotStart, target);
        OpenSet.Add(spotStart);//Put the starting node on the open list

        spotEnd = null;

        
        while (OpenSet.Count > 0)
        {
            //Find the node with the least f on the open list (q)
            int q = 0;
            for (int i = 0; i < OpenSet.Count; i++)
                if (OpenSet[i].F < OpenSet[q].F)
                    q = i;
                else if (OpenSet[i].F == OpenSet[q].F)//tie breaking for faster routing
                    if (OpenSet[i].H < OpenSet[q].H)
                        q = i;

            var current = OpenSet[q];

            // Instead of checking if we've reached the end node, we check if the current node is at least _distance away from the target
            //if (Vector2Int.Distance(new Vector2Int(current.X, current.Y), target) >= _distance)
            if(CalculateDistanceCost(current, target)>_distance)
            {
                // If the condition is met, this node becomes our new end node
                spotEnd = current;
                // Now we can break out of the loop to construct the path
                break;
            }

            OpenSet.Remove(current);//Pop q off the open list
            ClosedSet.Add(current);


            //Finds the next closest step on the grid
            var neighboors = current.Neighbors;
            for (int i = 0; i < neighboors.Count; i++)//look threw our current spots neighboors (current spot is the shortest F distance in openSet
            {
                Spot n = neighboors[i];
                if (!ClosedSet.Contains(n) && n.Height < 1)//Checks to make sure the neighboor of our current tile is not within closed set, and has a height of less than 1
                {
                    var tempG = current.G + CalculateDistanceCost(current, n);//gets a temp comparison integer for seeing if a route is shorter than our current path

                    bool newPath = false;
                    if (OpenSet.Contains(n)) //Checks if the neighboor we are checking is within the openset
                    {
                        if (tempG < n.G)//The distance to the end goal from this neighboor is shorter so we need a new path
                        {
                            n.G = tempG;
                            newPath = true;
                        }
                    }
                    else//if its not in openSet or closed set, then it IS a new path and we should add it too openset
                    {
                        n.G = tempG;
                        newPath = true;
                        OpenSet.Add(n);
                    }
                    if (newPath)//if it is a newPath caclulate the H and F and set current to the neighboors previous
                    {
                        n.H = -CalculateHValue(n, target);
                        n.F = n.G + n.H;
                        n.previous = current;
                    }
                }
            }
        }

        // If we found a spotEnd that meets the requirement, we retrace the path from that spot
        if (spotEnd != null)
        {
            List<Spot> Path = new List<Spot>();
            var temp = spotEnd;
            Path.Add(temp);
            while (temp.previous != null)
            {
                Path.Add(temp.previous);
                temp = temp.previous;
            }
            if (length - (Path.Count - 1) < 0)
            {
                Path.RemoveRange(0, (Path.Count - 1) - length);
            }
            return RetracePath(Path);
        }

        // If no path is found that satisfies the condition, return null
        return null;
    }

    public List<Spot> CreatePath(Vector3Int[,] grid, Vector2Int _start, Vector2Int _end, int length)
    {
        //if (!IsValidPath(grid, start, end))
        //     return null;

        Spot spotEnd = null;
        Spot spotStart = null;
        var columns = Spots.GetUpperBound(0) + 1;
        var rows = Spots.GetUpperBound(1) + 1;
        Spots = new Spot[columns, rows];

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Spots[i, j] = new Spot(grid[i, j].x, grid[i, j].y, grid[i, j].z);// grid[i, j].z is height to know has tilemap or not
            }
        }

        for (int i = 0; i < columns; i++)  // add neighboor for each grid on walkable tilemap
        {
            for (int j = 0; j < rows; j++)
            {
                Spots[i, j].AddNeighbors(Spots, i, j);
                if (Spots[i, j].X == _start.x && Spots[i, j].Y == _start.y)
                    spotStart = Spots[i, j];
                else if (Spots[i, j].X == _end.x && Spots[i, j].Y == _end.y)
                    spotEnd = Spots[i, j];
            }
        }
        if (!IsValidPath(grid, spotStart, spotEnd))
            return null;
        List<Spot> OpenSet = new List<Spot>();//contains nodes that are candidates for exploration
        List<Spot> ClosedSet = new List<Spot>();//contains nodes that have already been explored
        spotStart.G = 0;
        spotStart.H = CalculateDistanceCost(spotStart, spotEnd);
        OpenSet.Add(spotStart);//Put the starting node on the open list

        while (OpenSet.Count > 0)//While the open list is not empty
        {
            //Find the node with the least f on the open list (q)
            int q = 0;
            for (int i = 0; i < OpenSet.Count; i++)
                if (OpenSet[i].F < OpenSet[q].F)
                    q = i;
                else if (OpenSet[i].F == OpenSet[q].F)//tie breaking for faster routing
                    if (OpenSet[i].H < OpenSet[q].H)
                        q = i;

            var current = OpenSet[q];

            //if reach to the end node so found the path-> creates and returns the path
            if (spotEnd != null && OpenSet[q] == spotEnd)
            {
                List<Spot> Path = new List<Spot>();
                var temp = current;
                Path.Add(temp);
                while (temp.previous != null)
                {
                    Path.Add(temp.previous);
                    temp = temp.previous;
                }
                if (length - (Path.Count - 1) < 0)
                {
                    Path.RemoveRange(0, (Path.Count - 1) - length);
                }
                return RetracePath(Path);
            }

            OpenSet.Remove(current);//Pop q off the open list
            ClosedSet.Add(current);


            //Finds the next closest step on the grid
            var neighboors = current.Neighbors;
            for (int i = 0; i < neighboors.Count; i++)//look threw our current spots neighboors (current spot is the shortest F distance in openSet
            {
                Spot n = neighboors[i];
                if (!ClosedSet.Contains(n) && n.Height < 1)//Checks to make sure the neighboor of our current tile is not within closed set, and has a height of less than 1
                {
                    var tempG = current.G + CalculateDistanceCost(current,n);//gets a temp comparison integer for seeing if a route is shorter than our current path

                    bool newPath = false;
                    if (OpenSet.Contains(n)) //Checks if the neighboor we are checking is within the openset
                    {
                        if (tempG < n.G)//The distance to the end goal from this neighboor is shorter so we need a new path
                        {
                            n.G = tempG;
                            newPath = true;
                        }
                    }
                    else//if its not in openSet or closed set, then it IS a new path and we should add it too openset
                    {
                        n.G = tempG;
                        newPath = true;
                        OpenSet.Add(n);
                    }
                    if (newPath)//if it is a newPath caclulate the H and F and set current to the neighboors previous
                    {
                        n.H = CalculateDistanceCost(n, spotEnd);
                        n.F = n.G + n.H;
                        n.previous = current;
                    }
                }
            }

        }
        return null;
    }
    public List<Spot> RetracePath(List<Spot> path)
    {
        List<Spot> waypoints = new List<Spot>();
        for (int i = path.Count - 1; i >= 0; i--)
        {
            waypoints.Add(path[i]);
            //Debug.Log("h:"+path[i].H+"f:"+ path[i].F+ "g:"+path[i].G);
        }
        return waypoints;
    }
    private int CalculateDistanceCost(Spot a, Spot b)
    {
        
        var dx = Math.Abs(a.X - b.X);
        var dy = Math.Abs(a.Y - b.Y);
        int remaining = Math.Abs(dx - dy);
        return MOVE_DIAGONAL_COST * Mathf.Min(dx, dy) + MOVE_STRAIGHT_COST * remaining;

    }
    public int CalculateDistanceCost(Spot a, Vector2Int b)
    {
       
        var dx = Math.Abs(a.X - b.x);
        var dy = Math.Abs(a.Y - b.y);
        int remaining = Math.Abs(dx - dy);
        return MOVE_DIAGONAL_COST * Mathf.Min(dx, dy) + MOVE_STRAIGHT_COST * remaining;

    }
    public double CalculateHValue(Spot a, Vector2Int b)
    {
        var dx = Math.Abs(a.X - b.x);
        var dy = Math.Abs(a.Y - b.y);
        return Math.Sqrt(dy * dy + dx * dx);
    }
}

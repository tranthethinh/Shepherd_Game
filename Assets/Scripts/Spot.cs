using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Spot
{
    public int X;
    public int Y;
    public double F;//cost represents the sum of ‘G’ and ‘H’ costs
    public double G;//cost represents the distance from starting cell node
    public double H;// (Heuristic) cost represents the distance from ending cell node
    public int Height = 0;//if =0 has tilemap if =1 has no tilemap
    public List<Spot> Neighbors;
    public Spot previous = null;
    public Spot(int x, int y, int height)
    {
        X = x;
        Y = y;
        F = 0;
        G = int.MaxValue;
        H = 0;
        Neighbors = new List<Spot>();
        Height = height;
    }
    public void AddNeighbors(Spot[,] grid, int x, int y)
    {
        Neighbors = new List<Spot>(); // Make sure Neighbors is initialized

        int maxX = grid.GetUpperBound(0);
        int maxY = grid.GetUpperBound(1);

        // Check right neighbor
        if (x < maxX)
            Neighbors.Add(grid[x + 1, y]);

        // Check left neighbor
        if (x > 0)
            Neighbors.Add(grid[x - 1, y]);

        // Check top neighbor
        if (y < maxY)
            Neighbors.Add(grid[x, y + 1]);

        // Check bottom neighbor
        if (y > 0)
            Neighbors.Add(grid[x, y - 1]);

        // Check top-right neighbor
        if (x < maxX && y < maxY)
            Neighbors.Add(grid[x + 1, y + 1]);

        // Check top-left neighbor
        if (x > 0 && y < maxY)
            Neighbors.Add(grid[x - 1, y + 1]);

        // Check bottom-right neighbor
        if (x < maxX && y > 0)
            Neighbors.Add(grid[x + 1, y - 1]);

        // Check bottom-left neighbor
        if (x > 0 && y > 0)
            Neighbors.Add(grid[x - 1, y - 1]);
    }



}
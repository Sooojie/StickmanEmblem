
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GridPathfinder : MonoBehaviour
{
    [Header("References")]
    public TerrainCostManager terrainCostManager;

    private readonly Vector3Int[] directions =
    {
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(1, 0, 0)
    };

    // Method: FindPath
    // Calculate paths to a tile 
public List<Vector3Int> FindPath(Vector3Int start, Vector3Int target, UnitController unit)
    {
        List<Vector3Int> openList = new List<Vector3Int>();

        HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();

        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        Dictionary<Vector3Int, int> gCost = new Dictionary<Vector3Int, int>();

        openList.Add(start);
        gCost[start] = 0;

        while (openList.Count > 0)
        {
            Vector3Int current = openList[0];

            // Find the cell with the lowest total cost.
            foreach (Vector3Int cell in openList)
            {
                int currentCost = gCost[cell] + GetDistance(cell, target);
                int bestCost = gCost[current] + GetDistance(current, target);
                if (currentCost < bestCost) current = cell;
            }

            // Destination reached.
            if (current == target)
            {
                return ReconstructPath(cameFrom, current);
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (Vector3Int direction in directions)
            {
                Vector3Int neighbor = current + direction;

                if (closedList.Contains(neighbor)) continue;

                if (!terrainCostManager.IsWalkable(neighbor)) continue;

                if (UnitOccupancyManager.Instance.IsOccupied(neighbor, unit)) continue;

                // Entering a tile now uses its terrain cost.
                int tentativeGCost = gCost[current] + terrainCostManager.GetMovementCost(neighbor);

                if (!gCost.ContainsKey(neighbor) || tentativeGCost < gCost[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gCost[neighbor] = tentativeGCost;

                    if (!openList.Contains(neighbor)) openList.Add(neighbor);
                }
            }
        }

        return null;
    }

    // Method: GetReachableCells
    // Calculate the cheapest cost to reach a tile using Dijkstra
    public List<Vector3Int> GetReachableCells(Vector3Int start, int movementRange, UnitController unit)
    {
        List<Vector3Int> openList = new List<Vector3Int>();

        Dictionary<Vector3Int, int> movementCosts = new Dictionary<Vector3Int, int>();

        openList.Add(start);
        movementCosts[start] = 0;

        while (openList.Count > 0)
        {
            // Find the tile with the lowest accumulated cost.
            Vector3Int current = openList[0];

            foreach (Vector3Int cell in openList)
            {
                if (movementCosts[cell] < movementCosts[current]) current = cell;
            }

            openList.Remove(current);

            foreach (Vector3Int direction in directions)
            {
                Vector3Int neighbor = current + direction;

                if (!terrainCostManager.IsWalkable(neighbor)) continue;

                if (UnitOccupancyManager.Instance.IsOccupied(neighbor, unit)) continue;

                int newCost = movementCosts[current] + terrainCostManager.GetMovementCost(neighbor);

                // Ignore tiles beyond the movement budget.
                if (newCost > movementRange) continue;

                // Only update if this route is cheaper.
                if (!movementCosts.ContainsKey(neighbor) || newCost < movementCosts[neighbor])
                {
                    movementCosts[neighbor] = newCost;

                    if (!openList.Contains(neighbor)) openList.Add(neighbor);
                }
            }
        }

        List<Vector3Int> reachableCells = new List<Vector3Int>(movementCosts.Keys);

        // The starting tile should not be highlighted.
        reachableCells.Remove(start);

        return reachableCells;
    }

    // Method; GetDistance
    // Returns the Manhattan distance between two grid cells.
    private int GetDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    // Method; ReconstructPath
    // Builds the path from the predecessor map and orders it from start to destination.
    private List<Vector3Int> ReconstructPath( Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3Int> path = new List<Vector3Int>();

        path.Add(current);

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();

        return path;
    }
}

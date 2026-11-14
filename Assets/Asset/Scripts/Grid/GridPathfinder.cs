
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GridPathfinder : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap obstacleTilemap;

    private readonly Vector3Int[] directions =
    {
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(1, 0, 0)
    };

    // Method: FindPath
    // Calculate and find a path using Manhattan distance
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

                // Check map boundaries and walkable ground.
                if (!groundTilemap.HasTile(neighbor)) continue;

                // Check obstacles.
                if (obstacleTilemap != null && obstacleTilemap.HasTile(neighbor))continue;

                // Check other units.
                if (UnitOccupancyManager.Instance.IsOccupied(neighbor, unit)) continue;

                int tentativeGCost = gCost[current] + 1;

                if (!gCost.ContainsKey(neighbor) || tentativeGCost < gCost[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gCost[neighbor] = tentativeGCost;

                    if (!openList.Contains(neighbor)) openList.Add(neighbor);
                }
            }
        }

        // No valid path found.
        return null;
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

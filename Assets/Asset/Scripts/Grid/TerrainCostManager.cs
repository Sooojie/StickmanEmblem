using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainCostManager : MonoBehaviour
{
    [Header("Tilemap References")]
    public Tilemap groundTilemap;
    public Tilemap obstacleTilemap;
    public Tilemap forestTilemap;
    public Tilemap mountainTilemap;

    [Header("Movement Costs")]
    [Min(1)] public int normalCost = 1;
    [Min(1)] public int forestCost = 2;
    [Min(1)] public int mountainCost = 3;

    // Method: IsWalkable
    // Determines whether a unit can enter a tile.
    public bool IsWalkable(Vector3Int cell)
    {
        // Must have ground underneath.
        if (!groundTilemap.HasTile(cell))
            return false;

        // Obstacles and water are impassable.
        if (obstacleTilemap != null &&
            obstacleTilemap.HasTile(cell))
            return false;

        return true;
    }

    // Method: GetMovementCost
    // Returns the movement points required to enter a tile.
    public int GetMovementCost(Vector3Int cell)
    {
        // Mountain takes priority 
        if (mountainTilemap != null &&
            mountainTilemap.HasTile(cell))
        {
            return mountainCost;
        }
        // Forest second
        if (forestTilemap != null &&
            forestTilemap.HasTile(cell))
        {
            return forestCost;
        }

        return normalCost;
    }
}

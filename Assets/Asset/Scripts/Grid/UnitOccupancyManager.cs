
using UnityEngine;
using System.Collections.Generic;

public class UnitOccupancyManager : MonoBehaviour
{
    public static UnitOccupancyManager Instance;

    private Dictionary<Vector3Int, UnitController> occupiedCells = new Dictionary<Vector3Int, UnitController>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Method: RegisterUnit
    // Add unit location to the dictionary
    public void RegisterUnit(UnitController unit, Vector3Int cell)
    {
        if (occupiedCells.ContainsKey(cell))
        {
            Debug.LogWarning("Cell is already occupied: " + cell);
            return;
        }

        occupiedCells[cell] = unit;
    }

    // Method: IsOccupied
    // Check cell for occupation
    public bool IsOccupied(Vector3Int cell, UnitController requestingUnit = null)
    {
        if (!occupiedCells.TryGetValue(cell, out UnitController occupant)) return false;

        return occupant != requestingUnit;
    }

    // Method: TryReservePath
    // Reserve every cell in a path
    public bool TryReservePath(UnitController unit, List<Vector3Int> path)
    {
        foreach (Vector3Int cell in path)
        {
            if (IsOccupied(cell, unit)) return false;
        }

        foreach (Vector3Int cell in path)
        {
            occupiedCells[cell] = unit;
        }

        return true;
    }

    // Method: ReleaseCell
    // Remove previous occupied cell
    public void ReleaseCell(UnitController unit, Vector3Int cell)
    {
        if (occupiedCells.TryGetValue(cell, out UnitController occupant)
            && occupant == unit)
        {
            occupiedCells.Remove(cell);
        }
    }

    // Method: UnregisterUnit
    // Remove unit location from dictionaruy
    public void UnregisterUnit(UnitController unit, Vector3Int cell)
    {
        if (occupiedCells.TryGetValue(cell, out UnitController occupant) && occupant == unit)
        {
            occupiedCells.Remove(cell);
        }
    }
}

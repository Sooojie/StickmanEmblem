using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class UnitSelectionManager : MonoBehaviour
{
    [Header("References")]
    public Grid grid;
    public Tilemap groundTilemap;
    public Tilemap highlightTilemap;
    public TileBase movementHighlight;
    public GridPathfinder pathfinder;

    [Header("Obstacles")]
    public Tilemap obstacleTilemap;

    private UnitController selectedUnit;

    private readonly Vector3Int[] directions =
    {
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(1, 0, 0)
    };

    private void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;

        if (selectedUnit != null && selectedUnit.IsMoving) return;

        HandleMouseClick();
    }

    // Method: HandleMouseClick()
    // Read mouse position, click, and unit selection
    private void HandleMouseClick()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 worldPoint = new Vector2(worldPosition.x, worldPosition.y);

        Collider2D hit = Physics2D.OverlapPoint(worldPoint);

        // Detect if object was hit
        if (hit != null)
        {
            UnitController unit = hit.GetComponent<UnitController>();

            // Detect if object had UnitController
            if (unit != null)
            {
                SelectUnit(unit);
                return;
            }
        }

        // Get position of unit
        if (selectedUnit != null)
        {
            Vector3Int clickedCell = grid.WorldToCell(worldPosition);

            TryMoveSelectedUnit(clickedCell);
        }
    }

    // Method: SelectUnit
    // Sets unit if object at mouse position has a unitController
    private void SelectUnit(UnitController unit)
    {
        selectedUnit = unit;

        ShowMovementRange();
    }

    // Method: ShowMovementRange
    // Displays possible tiles to move unit
    private void ShowMovementRange()
    {
        highlightTilemap.ClearAllTiles();

        Queue<Vector3Int> queue = new Queue<Vector3Int>();

        Dictionary<Vector3Int, int> distances = new Dictionary<Vector3Int, int>();

        Vector3Int startCell = selectedUnit.CurrentCell;

        queue.Enqueue(startCell);
        distances[startCell] = 0;

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();

            int currentDistance = distances[current];

            if (currentDistance >= selectedUnit.movementRange) continue;

            foreach (Vector3Int direction in directions)
            {
                Vector3Int neighbor = current + direction;

                if (distances.ContainsKey(neighbor)) continue;

                if (!groundTilemap.HasTile(neighbor)) continue;

                // Don't allow movement through obstacles.
                if (obstacleTilemap != null && obstacleTilemap.HasTile(neighbor)) continue;

                // Don't allow movement through other units.
                if (UnitOccupancyManager.Instance.IsOccupied(neighbor, selectedUnit)) continue;

                int newDistance = currentDistance + 1;

                distances[neighbor] = newDistance;
                queue.Enqueue(neighbor);

                highlightTilemap.SetTile(neighbor, movementHighlight);
            }
        }
    }

    // Method: TryMoveSelectedUnit
    // Moves unit to selected tile, clear selected unit
    private void TryMoveSelectedUnit(Vector3Int targetCell)
    {
        if (!highlightTilemap.HasTile(targetCell)) return;

        // Calculate a path from the unit to the destination.
        List<Vector3Int> path = pathfinder.FindPath(selectedUnit.CurrentCell, targetCell, selectedUnit);

        if (path == null)
        {
            Debug.Log("No valid path found.");
            return;
        }

        // Send the path to the unit.
        selectedUnit.MoveTo(path);

        highlightTilemap.ClearAllTiles();
        selectedUnit = null;
    }
}

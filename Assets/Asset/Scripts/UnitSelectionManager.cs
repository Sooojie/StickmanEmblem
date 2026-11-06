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

    private void HandleMouseClick()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 worldPoint = new Vector2(worldPosition.x, worldPosition.y);

        Collider2D hit = Physics2D.OverlapPoint(worldPoint);

        if (hit != null)
        {
            UnitController unit = hit.GetComponent<UnitController>();

            if (unit != null)
            {
                SelectUnit(unit);
                return;
            }
        }

        if (selectedUnit != null)
        {
            Vector3Int clickedCell = grid.WorldToCell(worldPosition);

            TryMoveSelectedUnit(clickedCell);
        }
    }

    private void SelectUnit(UnitController unit)
    {
        selectedUnit = unit;

        ShowMovementRange();
    }

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

                int newDistance = currentDistance + 1;

                distances[neighbor] = newDistance;
                queue.Enqueue(neighbor);

                highlightTilemap.SetTile(neighbor, movementHighlight);
            }
        }
    }

    private void TryMoveSelectedUnit(Vector3Int targetCell)
    {
        if (!highlightTilemap.HasTile(targetCell))
            return;

        selectedUnit.MoveTo(targetCell);

        highlightTilemap.ClearAllTiles();
        selectedUnit = null;
    }
}

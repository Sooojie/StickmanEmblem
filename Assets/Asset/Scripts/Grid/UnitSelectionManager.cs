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
    public UnitStatsDisplay unitStatsDisplay;

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
    BattleManager battleManager =
        FindAnyObjectByType<BattleManager>();

   //only allow us to click if its our turn
    if (battleManager != null && !battleManager.IsPlayerTurn())
        return;

    if (Mouse.current == null ||
        !Mouse.current.leftButton.wasPressedThisFrame)
        return;

    if (selectedUnit != null && selectedUnit.IsMoving)
        return;

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
            UnitController unitController = hit.GetComponent<UnitController>();
            Unit unit = hit.GetComponent<Unit>();
            
            // Detect if object had UnitController
            if (unitController!= null)
            {
                SelectUnit(unitController);
                unitStatsDisplay.ShowUnit(unit);
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

//i added these two lines to make sure players cannot control the enemy
    if (unit.isEnemy)
        return;

    selectedUnit = unit;
    ShowMovementRange();
}

    // Method: ShowMovementRange
    // Displays possible tiles to move unit
    private void ShowMovementRange()
    {
        highlightTilemap.ClearAllTiles();

        // Calculate every reachable tile using movement costs.
        List<Vector3Int> reachableCells = pathfinder.GetReachableCells(selectedUnit.CurrentCell, selectedUnit.movementRange, selectedUnit);

        // Highlight reachable tiles.
        foreach (Vector3Int cell in reachableCells)
        {
            highlightTilemap.SetTile(cell, movementHighlight);
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
        unitStatsDisplay.ClearDisplay();
    }
}

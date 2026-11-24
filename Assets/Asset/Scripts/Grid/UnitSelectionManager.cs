using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public enum UnitActionMode
{
    None,
    Move,
    Attack
}

public class UnitSelectionManager : MonoBehaviour
{
    [Header("References")]
    public Grid grid;
    public Tilemap groundTilemap;
    public Tilemap highlightTilemap;
    public TileBase movementHighlight;
    public TileBase attackHighlight;

    public GridPathfinder pathfinder;
    public UnitStatsDisplay unitStatsDisplay;
    public BattleManager battleManager;

    private UnitController selectedUnit;
    private UnitActionMode currentMode = UnitActionMode.None;

   private void Update()
    {
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

    public void BeginMoveMode()
    {
        ClearHighlights();
        currentMode = UnitActionMode.Move;

        if (selectedUnit != null) ShowMovementRange();
    }

    public void BeginAttackMode()
    {
        ClearHighlights();
        currentMode = UnitActionMode.Attack;

        if (selectedUnit != null) ShowAttackTargets();
    }

    public void BeginItemMode()
    {
        currentMode = UnitActionMode.None;
        ClearHighlights();

        Debug.Log("Item usage will be implemented later.");
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
                HandleUnitClick(unit);
                return;
            }
        }

        // Get position of unit
        if (selectedUnit == null) return;

        Vector3Int clickedCell = grid.WorldToCell(worldPosition);

        if (currentMode == UnitActionMode.Move)
        {
            TryMoveSelectedUnit(clickedCell);
        }
    }

    private void HandleUnitClick(UnitController clickedUnit)
    {
        // Attack mode: clicking an enemy attempts an attack.
        if (currentMode == UnitActionMode.Attack)
        {
            if (clickedUnit.IsEnemy())
            {
                TryAttackTarget(clickedUnit);
                return;
            }
        }

        if (clickedUnit.IsEnemy()) return;

        selectedUnit = clickedUnit;

        Unit unit = selectedUnit.GetComponent<Unit>();

        if (unit != null && unitStatsDisplay != null) unitStatsDisplay.ShowUnit(unit);

        ClearHighlights();

        if (currentMode == UnitActionMode.Move) ShowMovementRange();

        if (currentMode == UnitActionMode.Attack) ShowAttackTargets();
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

    private void ShowAttackTargets()
    {
        if (selectedUnit == null || !selectedUnit.CanAct()) return;

        UnitController[] units = FindObjectsByType<UnitController>();

        foreach (UnitController target in units)
        {
            if (target == null || !target.IsEnemy()) continue;

            int distance = GetDistance(selectedUnit.CurrentCell, target.CurrentCell);

            if (distance == 1)
            {
                highlightTilemap.SetTile(target.CurrentCell,attackHighlight);
            }
        }
    }

    private void TryAttackTarget(UnitController target)
    {
        if (selectedUnit == null || target == null || !selectedUnit.CanAct()) return;

        int distance = GetDistance(selectedUnit.CurrentCell, target.CurrentCell);

        if (distance != 1)
        {
            Debug.Log("Enemy is not in attack range.");
            return;
        }

        battleManager.Attack(selectedUnit, target);

        ClearSelection();
    }

    public void ClearSelection()
    {
        ClearHighlights();

        selectedUnit = null;
        currentMode = UnitActionMode.None;

        if (unitStatsDisplay != null) unitStatsDisplay.ClearDisplay();
    }

    private void ClearHighlights()
    {
        highlightTilemap.ClearAllTiles();
    }

    private int GetDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitController : MonoBehaviour
{
    [Header("Movement Settings")]
    public int movementRange = 1;
    public float moveSpeed = 4f;
    
    private Grid grid;
    private Vector3Int currentCell;
    private bool isMoving;

    public Vector3Int CurrentCell => currentCell;
    public bool IsMoving => isMoving;

    private void Start()
    {
        grid = FindAnyObjectByType<Grid>(); 

        // Convert the unit's position to grid coordinates
        currentCell = grid.WorldToCell(transform.position);

        SnapToGrid();

        // Register unit location
        UnitOccupancyManager.Instance.RegisterUnit( this, currentCell);
    }

    // Method: SnapToGrid
    // Snap the unit to the center of the grid cell
    private void SnapToGrid()
    {
        Vector3 position = grid.GetCellCenterWorld(currentCell);

        position.z = transform.position.z;

        transform.position = position;
    }

    // Method: MoveTo
    // Move the unit with StartCoroutine
    public void MoveTo(List<Vector3Int> path)
    {
        if (isMoving || path == null || path.Count < 2) return;

        // Reserve the entire path before moving.
        if (!UnitOccupancyManager.Instance.TryReservePath(this, path))
        {
            Debug.Log("Movement path is occupied.");
            return;
        }

        StartCoroutine(MoveRoutine(path));
    }

    // Method MoveRoutine
    // Moves the unit to the target grid cell
    private IEnumerator MoveRoutine(List<Vector3Int> path)
    {
        for (int i = 1; i < path.Count; i++)
        {
            Vector3Int nextCell = path[i];

            Vector3 targetPosition = grid.GetCellCenterWorld(nextCell);

            targetPosition.z = transform.position.z;

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    moveSpeed * Time.deltaTime
                );

                yield return null;
            }

            transform.position = targetPosition;

            UnitOccupancyManager.Instance.ReleaseCell(this, currentCell);

            currentCell = nextCell;
        }

        isMoving = false;
    }

    private void OnDestroy()
    {   
        // Remove registered location
        if (UnitOccupancyManager.Instance != null)
        {
            UnitOccupancyManager.Instance.UnregisterUnit(this, currentCell);
        }
    }
}

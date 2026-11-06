using UnityEngine;
using System.Collections;

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

        currentCell = grid.WorldToCell(transform.position);

        SnapToGrid();
    }

    private void SnapToGrid()
    {
        Vector3 position = grid.GetCellCenterWorld(currentCell);

        position.z = transform.position.z;

        transform.position = position;
    }

    public void MoveTo(Vector3Int targetCell)
    {
        if (!isMoving)
        {
            StartCoroutine(MoveRoutine(targetCell));
        }
    }

    private IEnumerator MoveRoutine(Vector3Int targetCell)
    {
        isMoving = true;

        Vector3 targetPosition = grid.GetCellCenterWorld(targetCell);

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
        currentCell = targetCell;

        isMoving = false;
    }
}

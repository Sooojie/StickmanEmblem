using UnityEngine;
using System;

public enum TurnState
{
    PlayerTurn,
    EnemyTurn,
    BattleOver
}

public class BattleManager : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private int currentRound = 1;
    [SerializeField] private TurnState currentTurn = TurnState.PlayerTurn;

    public int CurrentRound => currentRound;
    public TurnState CurrentTurn => currentTurn;

    public event Action OnTurnChanged;
    public event Action<string> OnBattleLog;

    private void Start()
    {
        currentRound = 1;
        currentTurn = TurnState.PlayerTurn;

        AddLog("Battle started.");
        NotifyTurnChanged();
    }

    public void EndPlayerTurn()
    {
        currentTurn = TurnState.EnemyTurn;

        AddLog("Enemy turn started.");
        NotifyTurnChanged();

        StartEnemyTurn();
    }

   private void StartEnemyTurn()
    {
        StartCoroutine(EnemyTurnRoutine());
    }


    private IEnumerator EnemyTurnRoutine()
    {
        //this is to find the units in the scene
        UnitController[] units =
            FindObjectsByType<UnitController>(FindObjectsSortMode.None);

        foreach (UnitController enemy in units)
        {
            if (enemy == null || !enemy.isEnemy)
                continue;

            //this is going to try to find the closest player (unit)
            UnitController target = FindClosestPlayer(enemy);

            if (target == null)
                continue;

            // check if the enemy can attack
            int distance = GetDistance(
                enemy.CurrentCell,
                target.CurrentCell
            );

            if (distance > 1)
            {
                // Move closer to the player
                MoveEnemy(enemy, target);

                //wait until move is finished
                while (enemy != null && enemy.IsMoving)
                    yield return null;
            }

            // Check attack range again after moving the enemy
            if (enemy != null && target != null)
            {
                distance = GetDistance(
                    enemy.CurrentCell,
                    target.CurrentCell
                );

                if (distance == 1)
                    Attack(enemy, target);
            }

            //small little pause before next enemy makes a move
            yield return new WaitForSeconds(enemyDelay);
        }

        EndEnemyTurn();
    }

    public void EndEnemyTurn()
    {
        currentRound++;
        currentTurn = TurnState.PlayerTurn;
        
        AddLog("Player turn started.");
        NotifyTurnChanged();
    }

    private UnitController FindClosestPlayer(UnitController enemy)
    {
        UnitController[] units = FindObjectsByType<UnitController>(FindObjectsSortMode.None);

        UnitController closest = null;
        int shortestDistance = int.MaxValue;

        foreach (UnitController unit in units)
        {
            if (unit == null || unit.isEnemy)
                continue;

            int distance = GetDistance(
                enemy.CurrentCell,
                unit.CurrentCell
            );

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = unit;
            }
        }

        return closest;
    }

private void MoveEnemy(UnitController enemy, UnitController target)
    {
        //look for all reachable cells for the enemy
        List<Vector3Int> reachableCells =
            pathfinder.GetReachableCells(
                enemy.CurrentCell,
                enemy.movementRange,
                enemy
            );

        Vector3Int bestCell = enemy.CurrentCell;

        int bestDistance = GetDistance(
            bestCell,
            target.CurrentCell
        );

        //choose the cell that is closest to a player unit
        foreach (Vector3Int cell in reachableCells)
        {
            int distance = GetDistance(cell, target.CurrentCell);

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestCell = cell;
            }
        }

     
        if (bestCell == enemy.CurrentCell)
            return;

     
        List<Vector3Int> path =
            pathfinder.FindPath(enemy.CurrentCell, bestCell, enemy);

        if (path != null)
            enemy.MoveTo(path);
    }

    private void Attack(UnitController attacker, UnitController defender)
    {
        Unit attackerUnit = attacker.GetComponent<Unit>();
        Unit defenderUnit = defender.GetComponent<Unit>();

        if (attackerUnit == null || defenderUnit == null)
            return;

        UnitStats attackStats = attackerUnit.unitStats;
        UnitStats defenseStats = defenderUnit.unitStats;

        if (attackStats == null || defenseStats == null)
            return;

        int damage = Mathf.Max(
            0,
            attackStats.attack - defenseStats.defense
        );

        defenseStats.health -= damage;
        defenseStats.health = Mathf.Max(0, defenseStats.health);

        string message = attackerUnit.unitName +
            " attacked " + defenderUnit.unitName +
            " for " + damage + " damage!";

        Debug.Log(message);
        AddLog(message);

        if (defenseStats.health <= 0)
        {
            AddLog(defenderUnit.unitName + " was defeated!");
            Destroy(defender.gameObject);
        }
    }

    private int GetDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) +
               Mathf.Abs(a.y - b.y);
    }


    public void EndBattle()
    {
        AddLog("Battle ended.");
        NotifyTurnChanged();
        currentTurn = TurnState.BattleOver;
    }

    private void NotifyTurnChanged()
    {
        OnTurnChanged?.Invoke();
    }

    public void AddLog(string message)
    {
        OnBattleLog?.Invoke(message);
    }

    public bool IsPlayerTurn()
    {
        return currentTurn == TurnState.PlayerTurn;
    }
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum TurnState
{
    PlayerTurn,
    EnemyTurn,
    BattleOver
}

public class BattleManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridPathfinder pathfinder;
    
    [Header("State")]
    [SerializeField] private int currentRound = 1;
    [SerializeField] private TurnState currentTurn = TurnState.PlayerTurn;

    [Header("Combat")]
    [SerializeField] private UnitSelectionManager selectionManager;
    [SerializeField] private CombatFormula combatFormula;

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
        selectionManager.ClearSelection();
        ResetUnitActions(true);

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
            FindObjectsByType<UnitController>();

        foreach (UnitController enemy in units)
        {
            if (enemy == null || !enemy.IsEnemy())
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
            yield return new WaitForSeconds(0.3f);
        }

        EndEnemyTurn();
    }

    public void EndEnemyTurn()
    {
        currentRound++;
        currentTurn = TurnState.PlayerTurn;
        
        ResetUnitActions(false);
        AddLog("Player turn started.");
        NotifyTurnChanged();
    }

    private UnitController FindClosestPlayer(UnitController enemy)
    {
        UnitController[] units = FindObjectsByType<UnitController>();

        UnitController closest = null;
        int shortestDistance = int.MaxValue;

        foreach (UnitController unit in units)
        {
            if (unit == null || unit.IsEnemy())
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

    public void Attack(UnitController attacker, UnitController defender)
    {
        Unit attackerUnit = attacker.GetComponent<Unit>();
        Unit defenderUnit = defender.GetComponent<Unit>();

        if (attackerUnit == null || defenderUnit == null)
            return;

        UnitStats attackStats = attackerUnit.unitStats;
        UnitStats defenseStats = defenderUnit.unitStats;

        if (attackStats == null || defenseStats == null)
            return;

        combatFormula.InitiateCombat(attackerUnit, defenderUnit, attacker.CurrentCell, defender.CurrentCell);

        attacker.hasActed = true;

        AddLog(attackerUnit.unitName + " attacked " + defenderUnit.unitName);

        // Testing
        //if (defenseStats.health <= 0)
        //{
            AddLog(defenderUnit.unitName + " was defeated!");
            Destroy(defender.gameObject);
        //}

        if (attackStats.health <= 0)
        {
            AddLog(attackerUnit.unitName + " was defeated!");
            Destroy(attacker.gameObject);
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

    public void MoveAction()
    {
        if (!IsPlayerTurn()) return;

        selectionManager.BeginMoveMode();
    }

    public void AttackAction()
    {
        if (!IsPlayerTurn()) return;

        selectionManager.BeginAttackMode();
    }

    public void UseItemAction()
    {
        if (!IsPlayerTurn()) return;

        selectionManager.BeginItemMode();
    }

    public void WaitAction()
    {
        if (!IsPlayerTurn()) return;

        selectionManager.ClearSelection();
        EndPlayerTurn();
    }

    private void ResetUnitActions(bool enemies)
    {
        UnitController[] units = FindObjectsByType<UnitController>();

        foreach (UnitController unit in units)
        {
            if (unit.IsEnemy() == enemies)
            {
                unit.ResetTurnActions();
            }
        }
    }
}

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
        Debug.Log("Enemy start turn");

        EndEnemyTurn();
    }

    public void EndEnemyTurn()
    {
        currentRound++;
        currentTurn = TurnState.PlayerTurn;
        
        AddLog("Player turn started.");
        NotifyTurnChanged();
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

using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MissionUIDisplay : MonoBehaviour
{
    [Header("Mission Information")]
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text turnText;

    [Header("Combat Log")]
    [SerializeField] private TMP_Text logText;

    private List<string> logs = new List<string>();

    [Header("Battle Manager")]
    [SerializeField] private BattleManager battleManager;

    private void OnEnable()
    {
        battleManager.OnTurnChanged += UpdateTurnDisplay;
        battleManager.OnBattleLog += AddLogMessage;
    }
    private void OnDisable()
    {
        battleManager.OnTurnChanged -= UpdateTurnDisplay;
        battleManager.OnBattleLog -= AddLogMessage;
    }

    private void Start()
    {
        UpdateTurnDisplay();
    }

    private void UpdateTurnDisplay()
    {
        roundText.text = "Round " + battleManager.CurrentRound;

        switch (battleManager.CurrentTurn)
        {
            case TurnState.PlayerTurn:
                turnText.text = "Player Turn";
                break;

            case TurnState.EnemyTurn:
                turnText.text = "Enemy Turn";
                break;

            case TurnState.BattleOver:
                turnText.text = "Battle Over";
                break;
        }
    }

    private void AddLogMessage(string message)
    {
        logs.Add(message);

        if (logs.Count > 5)
        {
            logs.RemoveAt(0);
        }

        logText.text = string.Join("\n", logs);
    }
}

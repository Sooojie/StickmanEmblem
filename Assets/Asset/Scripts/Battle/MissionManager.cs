using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text objectiveText;

    [Header("Mission State")]
    private bool objectiveCompleted = false;

    private void Start()
    {
        UpdateObjectiveDisplay();
    }

    private void Update()
    {
        if (objectiveCompleted) return;

        CheckObjectives();
    }

    private void CheckObjectives()
    {
        UnitController[] units = FindObjectsByType<UnitController>();

        bool enemiesRemaining = false;

        foreach (UnitController unit in units)
        {
            if (unit != null && unit.IsEnemy())
            {
                enemiesRemaining = true;
                break;
            }
        }

        if (!enemiesRemaining)
        {
            CompleteObjective();
        }
    }

    private void CompleteObjective()
    {
        objectiveCompleted = true;
        UpdateObjectiveDisplay();

        Debug.Log("Mission objective completed!");
    }

    private void UpdateObjectiveDisplay()
    {
        if (objectiveCompleted)
        {
            objectiveText.text = "Objectives:\n- Complete";
        }
        else
        {
            objectiveText.text = "Objectives:\n- Defeat all enemies";
        }
    }
}

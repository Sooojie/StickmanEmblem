using UnityEngine;
using TMPro;

public class UnitStatsDisplay : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private TMP_Text attributesText;

    public void ShowUnit(Unit unit)
    {
        UnitStats stats = unit.unitStats;

        nameText.text = unit.unitName;
        levelText.text = "Level: " + stats.level;

        healthText.text = "HP: " + stats.health + " / " + stats.maxHealth;

        experienceText.text = "EXP: " + stats.experience + " / 100";

        attributesText.text =
            "ATK: " + stats.attack +
            " DEF: " + stats.defense +
            " RES: " + stats.resistance +
            " SPD: " + stats.speed + "\n" +
            "SKL: " + stats.skill  +
            " LCK: " + stats.luck  +
            " CON: " + stats.constitution;
    }

    public void ClearDisplay()
    {
        nameText.text = "";
        levelText.text = "";
        healthText.text = "";
        experienceText.text = "";
        attributesText.text = "";
    }
}

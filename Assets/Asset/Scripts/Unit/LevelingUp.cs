using UnityEngine;

public class LevelingUp : MonoBehaviour
{
    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void GainExperience(int experience)
    {
        unit.unitStats.experience += experience;

        while (unit.unitStats.experience >= 100)
        {
            unit.unitStats.experience -= 100;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        unit.unitStats.level++;

        IncreaseAttribute(unit.unitStats.healthGrowth, "health");
        IncreaseAttribute(unit.unitStats.attackGrowth, "attack");
        IncreaseAttribute(unit.unitStats.skillGrowth, "skill");
        IncreaseAttribute(unit.unitStats.speedGrowth, "speed");
        IncreaseAttribute(unit.unitStats.luckGrowth, "luck");
        IncreaseAttribute(unit.unitStats.defenseGrowth, "defense");
        IncreaseAttribute(unit.unitStats.resistanceGrowth, "resistance");
    }

    public void IncreaseAttribute(int growthRate, string stat)
    {
        int roll = Random.Range(0, 100);

        if (roll < growthRate)
        {
            switch (stat)
            {
                case "health":
                    unit.unitStats.maxHealth++;
                    unit.unitStats.health++;
                    break;
                case "attack":
                    unit.unitStats.attack++;
                    break;
                case "skill":
                    unit.unitStats.skill++;
                    break;
                case "speed":
                    unit.unitStats.speed++;
                    break;
                case "luck":
                    unit.unitStats.luck++;
                    break;
                case "defense":
                    unit.unitStats.defense++;
                    break;
                case "resistance":
                    unit.unitStats.resistance++;
                    break;
                default:
                    break;
            }
        }
    }
}
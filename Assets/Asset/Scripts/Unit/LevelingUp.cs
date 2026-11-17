using UnitEngine;

public class LevelingUp : MonoBehaviour
{
    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void GainExperience(int experience)
    {
        unit.exp += experience;

        while (unit.exp >= 100)
        {
            unit.exp -= 100;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        unit.level++;

        if(Random.Range(0, 100) < unit.healthGrowth)
        {
            unit.maxHealth++;
        }

        if(Random.Range(0, 100) < unit.attackGrowth)
        {
            unit.attack++;
        }

        if(Random.Range(0, 100) < unit.skillGrowth)
        {
            unit.skill++;
        }

        if(Random.Range(0, 100) < unit.speedGrowth)
        {
            unit.speed++;
        }

        if(Random.Range(0, 100) < unit.luckGrowth)
        {
            unit.luck++;
        }

        if (Random.Range(0, 100) < unit.defenseGrowth)
        {
            unit.defense++;
        }

        if(Random.Range(0, 100) < unit.resistanceGrowth)
        {
            unit.resistance++;
        }
    }
}
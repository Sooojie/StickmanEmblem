//base stats for mage
using UnityEngine;

/**
public class Mage : UnitStats
{
    public Mage()
    {
        level = 1;
        experience = 0;

        maxHealth = 16;
        attack = 1;
        defense = 3;
        resistance = 3;
        speed = 3;
        range = 5;
        skill = 2;
        luck = 0;
        constitution = 6;

        healthGrowth = 45;
        attackGrowth = 75;
        skillGrowth  = 20;
        speedGrowth = 35;
        luckGrowth = 50;
        defenseGrowth = 10;
        resistanceGrowth = 35;
    }
}
**/

[CreateAssetMenu(fileName = "MageStats", menuName = "Units/Stats/Mage")]
public class Mage : UnitStats
{
}
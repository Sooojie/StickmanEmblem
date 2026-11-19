//base stats for tank
using UnityEngine;

/**
public class Tank : UnitStats
{
    public Tank()
    {
        level = 1;
        experience = 0;

        maxHealth = 23;
        attack = 7;
        defense = 8;
        resistance = 3;
        speed = 7;
        range = 8;
        skill = 4;
        luck = 0;
        constitution = 11;

        healthGrowth = 60;
        attackGrowth = 25;
        skillGrowth  = 20;
        speedGrowth = 25;
        luckGrowth = 20;
        defenseGrowth = 15;
        resistanceGrowth = 20;
    }
}
**/

[CreateAssetMenu(fileName = "TankStats", menuName = "Units/Stats/Tank")]
public class Tank : UnitStats
{
}
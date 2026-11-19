//base stats for knight
using UnityEngine;

/**
public class Knight : UnitStats
{
    public Knight()
    {
        level = 1;
        experience = 0;
        
        maxHealth = 18;
        attack = 3;
        defense = 5;
        resistance = 0;
        speed = 4;
        range = 5;
        skill = 3;
        luck = 0;
        constitution = 6;
        
        healthGrowth = 80;
        attackGrowth = 40;
        skillGrowth  = 50;
        speedGrowth = 40;
        luckGrowth = 60;
        defenseGrowth = 25;
        resistanceGrowth = 30;
    }
}
**/

[CreateAssetMenu(fileName = "KnightStats", menuName = "Units/Stats/Knight")]
public class Knight : UnitStats
{
}
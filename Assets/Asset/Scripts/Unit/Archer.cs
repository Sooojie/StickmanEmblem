
//base stats for archer
using UnityEngine;
public class Archer : UnitType, ScriptableObject
{
    public Archer()
    {
        maxHealth = 18;
        attack = 4;
        defense = 3;
        resistance = 0;
        speed = 3;
        range = 5;
        skill = 3;
        luck = 0;
        constitution = 7;

        healthGrowth = 80;
        attackGrowth = 40;
        skillGrowth  = 50;
        speedGrowth = 40;
        luckGrowth = 40;
        defenseGrowth = 20;
        resistanceGrowth = 10;

    }

}
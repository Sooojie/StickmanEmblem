
//base stats for giant
using UnityEngine;

public class Giant : UnitType, ScriptableObject
{
    public Giant()
    {
        maxHealth = 20;
        attack = 5;
        defense = 2;
        resistance = 0;
        speed = 4;
        range = 5;
        skill = 2;
        luck = 0;
        constitution = 11;

        healthGrowth = 80;
        attackGrowth = 30;
        skillGrowth  = 30;
        speedGrowth = 35;
        luckGrowth = 30;
        defenseGrowth = 40;
        resistanceGrowth = 15;

    }

}
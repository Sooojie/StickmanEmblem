using UnityEngine;

public class Unit : ScriptableObject
{
    public string name { get; private set; }
    public int level { get; set; }

    public int exp {get; set;}
    public int health { get; set; }
    public int maxHealth { get; set; }
    public int attack { get; set; }
    public int defense { get; set; }
    public int resistance { get; set; }
    public int speed { get; set; }
    public int range { get; set; }
    public int skill { get; set; }
    public int luck { get; set; }
    public int constitution { get; set; }

    public int healthGrowth { get; set; }
    public int attackGrowth { get; set; }
    public int skillGrowth { get; set; }
    public int speedGrowth { get; set; }
    public int luckGrowth { get; set; }
    public int defenseGrowth { get; set; }
    public int resistanceGrowth { get; set; }

    public void Initialize (string name, UnitType unitType)
    {
        this.name = name;
        level = 1;
        exp = 0;
        health = unitType.maxHealth;
        maxHealth = unitType.maxHealth;
        attack = unitType.attack;
        defense = unitType.defense;
        resistance = unitType.resistance;
        speed = unitType.speed;
        range = unitType.range;
        skill = unitType.skill;
        luck = unitType.luck;
        constitution = unitType.constitution;
        healthGrowth = unitType.healthGrowth;
        attackGrowth = unitType.attackGrowth;
        skillGrowth = unitType.skillGrowth;
        speedGrowth = unitType.speedGrowth;
        luckGrowth = unitType.luckGrowth;
        defenseGrowth = unitType.defenseGrowth;
        resistanceGrowth = unitType.resistanceGrowth;
    }
}
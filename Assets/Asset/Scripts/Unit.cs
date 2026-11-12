using System.Collections.Generic;

public class Unit
{
    public string name { get; private set; }
    public int level { get; set; }

    public int exp {get; set;}
    private int health { get; set; }
    public Weapon weapon { get; set; }
    private int maxHealth { get; set; }
    public int attack { get; set; }
    public int defense { get; set; }
    public int resistance { get; set; }
    public int speed { get; set; }
    public int range { get; set; }
    public int skill { get; set; }
    public int luck { get; set; }
    public int constitution { get; set; }

    public Unit (string name, Weapon weapon, UnitType unitType)
    {
        this.name = name;
        level = 1;
        exp = 0;
        health = unitType.maxHealth;
        this.weapon = weapon;
        maxHealth = unitType.maxHealth;
        attack = unitType.attack;
        defense = unitType.defense;
        resistance = unitType.resistance;
        speed = unitType.speed;
        range = unitType.range;
        skill = unitType.skill;
        luck = unitType.luck;
        constitution = unitType.constitution;
    }
}
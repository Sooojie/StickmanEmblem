using UnityEngine;

public class UnitStats : ScriptableObject
{
    [Header("Level")]
    public int level = 1;
    public int experience = 0;

    [Header("Stats")]
    public int health;
    public int maxHealth;
    public int attack;
    public int defense;
    public int resistance;
    public int speed;
    public int range;    
    public int skill;
    public int luck;
    public int constitution;

    [Header("Stats Growth Rate")]
    public int healthGrowth;
    public int attackGrowth;
    public int skillGrowth;
    public int speedGrowth;
    public int luckGrowth;
    public int defenseGrowth;
    public int resistanceGrowth;

    public void Initialize()
    {
        experience = 0;
        health = maxHealth;
    }
}
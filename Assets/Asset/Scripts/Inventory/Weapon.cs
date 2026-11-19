using UnityEngine;

public class Weapon : Item
{
    [Header("Weapon Attributes")]
    public int might;
    public int hit;
    public int crit;
    public int range;
    public int weight;

    private void awake()
    {
        itemType = ItemType.Weapon;
    }
}
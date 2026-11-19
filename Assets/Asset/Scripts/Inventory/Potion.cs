using UnityEngine;

/**
public class Potion : Item
{
    public int healthRestore { get; set; }

    public Potion()
    {
        healthRestore = 10;
        itemType = ItemType.Potion;
        uses = 1;
        cost = 300;
    }
}
**/

[CreateAssetMenu(fileName = "Potion", menuName = "Inventory/Potion")]
public class Potion : Item
{
    public int healthRestore;

    private void awake()
    {
        itemType = ItemType.Potion;
    }
}
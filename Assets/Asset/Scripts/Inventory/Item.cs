using UnityEngine;

public enum ItemType
{
    Weapon,
    Potion
}

public class Item : ScriptableObject
{
    [Header("Item Information")]
    public ItemType itemType;
    public int uses;
    public int price;
    
}
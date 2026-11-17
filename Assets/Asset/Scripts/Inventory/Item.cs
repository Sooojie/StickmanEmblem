
public enum ItemType
{
    Weapon,
    Potion
}

public class Item
{
    public int uses { get; set; }

    public int price { get; set; }
    public ItemType itemType { get; set; }
}
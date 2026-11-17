public class Potion : Item
{
    public int healthRestore { get; set; }

    public Potion()
    {
        healthRestore = 10;
        itemType = Potion;
        uses = 1;
        cost = 300;

    }
}
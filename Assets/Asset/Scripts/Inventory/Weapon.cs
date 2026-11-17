public class Weapon : Item
{
    public weaponType type { get; set; }
    public int might { get; private set; }
    public int hit { get; private set; }
    public int crit { get; private set; }
    public int range { get; private set; }
    public int weight { get; private set; }

    public enum weaponType
    {
        Sword,
        Spear,
        Mace,
        Bow,
        Staff
    }
}
public class Weapon
{
    public weaponType type { get; protected set; }
    public int level { get; private set; }
    public int might { get; private set; }
    public int hit { get; private set; }
    public int crit { get; private set; }
    public int range { get; private set; }
    public int weight { get; private set; }
    public int uses { get; set; }

    public enum weaponType
    {
        Sword,
        Spear,
        Mace,
        Bow,
        Staff
    }
}
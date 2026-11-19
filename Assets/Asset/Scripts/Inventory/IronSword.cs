using UnityEngine;

/**
public class IronSword : Weapon
{
    public IronSword()
    {
        type = weaponType.Sword;
        might = 5;
        hit = 85;
        crit = 0;  
        range = 1;
        weight = 5;
        uses = 46;
        cost = 460;
    }
}
**/

[CreateAssetMenu(fileName = "IronSword", menuName = "Inventory/Weapon/IronSword")]
public class IronSword : Weapon
{
}
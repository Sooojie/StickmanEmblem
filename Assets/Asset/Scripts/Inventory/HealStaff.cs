using UnityEngine;

/**
public class HealStaff : Weapon
{
    public HealStaff()
    {
        type = weaponType.Staff;
        might = 10;
        hit = 0;
        crit = 0;  
        range = 1;
        weight = 2;
        uses = 30;
        cost = 600;
    }
}
**/

[CreateAssetMenu(fileName = "HealStaff", menuName = "Inventory/Weapon/HealStaff")]
public class HealStaff : Weapon
{
}
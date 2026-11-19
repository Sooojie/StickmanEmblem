using UnityEngine;

/**
public class IronSpear : Weapon
{
    public IronSpear()
    {
        type = weaponType.Spear;
        might = 7;
        hit = 70;
        crit = 0;  
        range = 1;
        weight = 8;
        uses = 45;
        cost = 360;
    }
}
**/

[CreateAssetMenu(fileName = "IronSpear", menuName = "Inventory/Weapon/IronSpear")]
public class IronSpear : Weapon
{
}
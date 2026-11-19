using UnityEngine;

/**
public class IronBow : Weapon
{
    public IronBow()
    {
        type = weaponType.Bow;
        might = 6;
        hit = 80;
        crit = 0;  
        range = 2;
        weight = 5;
        uses = 45;
        cost = 540;
    }
}
**/

[CreateAssetMenu(fileName = "IronBow", menuName = "Inventory/Weapon/IronBow")]
public class IronBow : Weapon
{
}
using UnityEngine;

/**
public class IronMace : Weapon
{
    public IronMace()
    {
        type = weaponType.Mace;
        might = 8;
        hit = 65;
        crit = 0;  
        range = 1;
        weight = 10;
        uses = 45;
        cost = 270;
    }
}
**/

[CreateAssetMenu(fileName = "IronMace", menuName = "Inventory/Weapon/IronMace")]
public class IronMace : Weapon
{
}
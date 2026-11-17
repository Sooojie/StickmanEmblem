using UnityEngine;

public class IronMace : Weapon, ScriptableObject
{
    public IronMace()
    {
        type = Mace;
        might = 8;
        hit = 65;
        crit = 0;  
        range = 1;
        weight = 10;
        uses = 45;
        cost = 270;
    }
}
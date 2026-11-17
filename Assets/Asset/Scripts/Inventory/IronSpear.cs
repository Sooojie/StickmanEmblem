using UnityEngine;

public class IronSpear : Weapon, ScriptableObject
{
    public IronSpear()
    {
        type = Spear;
        might = 7;
        hit = 70;
        crit = 0;  
        range = 1;
        weight = 8;
        uses = 45;
        cost = 360;
    }
}
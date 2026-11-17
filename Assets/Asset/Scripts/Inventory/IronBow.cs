using UnityEngine;

public class IronBow : Weapon, ScriptableObject
{
    public IronBow()
    {
        type = Bow;
        might = 6;
        hit = 80;
        crit = 0;  
        range = 2;
        weight = 5;
        uses = 45;
        cost = 540;
    }
}
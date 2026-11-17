using UnityEngine;

public class IronSword : Weapon, ScriptableObject
{
    public IronSword()
    {
        type = Sword;
        might = 5;
        hit = 85;
        crit = 0;  
        range = 1;
        weight = 5;
        uses = 46;
        cost = 460;
    }
}
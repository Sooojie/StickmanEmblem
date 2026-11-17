using UnityEngine;

public class HealStaff : Weapon, ScriptableObject
{
    public HealStaff()
    {
        type = Staff;
        might = 10;
        hit = 0;
        crit = 0;  
        range = 1;
        weight = 2;
        uses = 30;
        cost = 600;
    }
}
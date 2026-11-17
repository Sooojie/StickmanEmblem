using UnityEngine;

public class Unit : UnitBehaviour
{
    public Inventory inventory;
    public UnitStats unitStats;
    public int gold;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        unitStats = GetComponent<UnitStats>();
        gold = 0;
    }

    public void GetUnitStats(string name, UnitType unitType)
    {
        unitStats.Initialize(name, unitType);
    }

    public void GetItem(Item item)
    {
        inventory.AddItem(item);
    }

    public void addGold(int amount)
    {
        gold += amount;
    }
}
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit Information")]
    public string unitName;
    public int gold;

    public UnitStats unitStats;

    [Header("Inventory")]
    public Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();

        unitStats = Instantiate(unitStats);
        unitStats.Initialize();
        gold = 0;
    }

    public void GetItem(Item item)
    {
        inventory.AddItem(item);
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }
}
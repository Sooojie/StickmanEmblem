using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int maxSlots = 5;

    public bool AddItem(Item item)
    {
        if (items.Count >= maxSlots)
        {
            Debug.Log("Inventory is Full");
            return false;
        }

        items.Add(item);
        return true;
    }

    public bool RemoveItem(int index)
    {
        if (index >= 0 && index < items.Count)
        {
            items.RemoveAt(index);
        }
    }
    //get number of potions
    public int GetPotions()
    {
        int count = 0;
        foreach (Item item in items)
        {
            if (item.itemType == ItemType.Potion)
            {
                count++;
            }
        }

        return count;
    }

}
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemData> inventory;

    public int selectedIndex = 0;
    public int maxSlots = 6;
    public bool itemAdded = false;
    public PlayerControls playerControls;

    private void Awake()
    {
        inventory = new List<ItemData>(maxSlots);

        for (int i = 0; i < maxSlots; i++)
        {
            inventory.Add(null);
        }
    }

    public ItemData GetSelectedItem()
    {
        if (inventory.Count == 0) return null;

        if (selectedIndex < 0 || selectedIndex >= inventory.Count) return null;

        return inventory[selectedIndex];
    }

    public void RemoveSelectedItem()
    {
        if (selectedIndex < 0 || selectedIndex >= inventory.Count) return;

        if (inventory[selectedIndex] != null)
        {
            Debug.Log($"Removed: {inventory[selectedIndex].name}");
            inventory[selectedIndex] = null;
        }

        playerControls.ClearHeldItem();
    }

    public bool CheckInventory()
    {
        foreach (var item in inventory)
        {
            if (item == null) return true;
        }

        return false;
    }

    public void AddItem(ItemData item)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                Debug.Log($"Item Added: {item.name}");
                itemAdded = true;
                return;
            }
        }

        Debug.Log("COuld not add. Inventory Full.");
    }

    public void RemoveLastItem()
    {
        if (inventory.Count > 0)
        {
            ItemData removedItem = inventory[inventory.Count - 1];
            inventory.RemoveAt(inventory.Count - 1);
            Debug.Log($"Removed Last Item: {removedItem.name}");
        }
        else
        {
            Debug.Log("Cannot remove item: Inventory is empty.");
        }
    }


    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == item)
            {
                Debug.Log($"Item Removed: {item.name}");
                inventory[i] = null;  
                return;
            }
        }
    }
}

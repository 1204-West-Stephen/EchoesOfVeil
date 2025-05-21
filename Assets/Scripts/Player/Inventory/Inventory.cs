using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ScriptableObject> inventory;

    private void Start()
    {

    }

    public bool CheckInventory()
    {
        if (inventory.Count >= 6)
        {
            Debug.Log("Cannot add item: Inventory full.");
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddItem(ScriptableObject item)
    {
        Debug.Log($"Item Added: {item.name}");
        inventory.Add(item);
    }

    public void RemoveLastItem()
    {
        if (inventory.Count > 0)
        {
            ScriptableObject removedItem = inventory[inventory.Count - 1];
            inventory.RemoveAt(inventory.Count - 1);
            Debug.Log($"Removed Last Item: {removedItem.name}");
        }
        else
        {
            Debug.Log("Cannot remove item: Inventory is empty.");
        }
    }


    public void RemoveItem(ScriptableObject item)
    {
        Debug.Log($"Item Removed: {item.name}");
        inventory.Remove(item);
    }
}

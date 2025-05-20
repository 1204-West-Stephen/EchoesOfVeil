using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ScriptableObject> inventory;

    private void Start()
    {

    }

    public void TryAddItem(ScriptableObject item)
    {
        if (inventory.Count >= 6)
        {
            Debug.Log("Cannot add item: Inventory full.");
        }
        else
        {
            Destroy(gameObject); // remove item from world after pickup
            AddItem(item);
        }
    }

    private void AddItem(ScriptableObject item)
    {
        Debug.Log($"Item Added: {item.name}");
        inventory.Add(item);
    }

    public void RemoveItem(ScriptableObject item)
    {
        Debug.Log($"Item Removed: {item.name}");
        inventory.Remove(item);
    }
}

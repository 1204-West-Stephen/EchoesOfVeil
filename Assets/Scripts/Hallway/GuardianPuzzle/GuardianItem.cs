using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianItem : MonoBehaviour, i_Interactable
{
    public ItemData item;
    public bool itemPickedUp = false;

    private Inventory inventory;

    public GameObject placedObject;
    [HideInInspector] public ItemData placedObjectData;

    [Header("Child Transform for Placement")]
    public Transform itemPos; // assign in prefab hierarchy

    private void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();

        if (itemPos == null)
        {
            Debug.LogError($"{name} has no ItemPos assigned!");
        }
    }

    public void Interact()
    {
        // === PICK UP ORIGINAL WORLD ITEM ===
        if (item != null && !itemPickedUp)
        {
            if (inventory != null && inventory.CheckInventory())
            {
                inventory.AddItem(item);
                itemPickedUp = true;

                // Detach itemPos so it survives Destroy
                if (itemPos != null)
                    itemPos.parent = null;

                Destroy(gameObject); // destroy the prefab root

                item = null;
            }

            return;
        }

        // === PLACE ITEM FROM INVENTORY ===
        if (placedObject == null)
        {
            PlaceObject();
        }
        else
        {
            PickUpObject();
        }
    }

    private bool PlaceObject()
    {
        if (inventory == null) return false;

        ItemData selectedItem = inventory.GetSelectedItem();
        if (selectedItem == null) return false;
        if (selectedItem.typeInput != InputType.GuardianItem) return false;

        placedObjectData = selectedItem;
        inventory.RemoveSelectedItem();

        if (itemPos != null)
        {
            placedObject = Instantiate(
                selectedItem.prefab,
                itemPos.position,
                itemPos.rotation
            );
            placedObject.SetActive(true);
        }
        else
        {
            Debug.LogError($"{name} has no ItemPos assigned for placement!");
        }

        Debug.Log("Object placed.");
        return true;
    }

    private void PickUpObject()
    {
        if (placedObject == null || inventory == null || !inventory.CheckInventory()) return;

        inventory.AddItem(placedObjectData);

        Destroy(placedObject);
        placedObject = null;
        placedObjectData = null;

        Debug.Log("Placed object picked up.");
    }
}
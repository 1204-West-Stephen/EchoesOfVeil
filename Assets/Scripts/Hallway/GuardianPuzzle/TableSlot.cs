using UnityEngine;

public class TableSlot : MonoBehaviour, i_Interactable
{
    public Transform itemAnchor;   // Where the item will be placed
    public GameObject currentItem;

    private Inventory inventory;

    private void Awake()
    {
        inventory = FindFirstObjectByType<Inventory>();
    }

    public void Interact()
    {
        ItemData selectedItem = inventory.GetSelectedItem();

        // SLOT HAS ITEM  PICK IT UP
        if (currentItem != null)
        {
            WorldItem worldItem = currentItem.GetComponent<WorldItem>();

            // Return held item to inventory first
            if (selectedItem != null)
            {
                inventory.AddItem(selectedItem);
                inventory.RemoveSelectedItem();
            }

            if (worldItem != null)
                inventory.AddItem(worldItem.item);

            Destroy(currentItem);
            currentItem = null;
            return;
        }

        // SLOT EMPTY  PLACE ITEM
        if (selectedItem == null) return;

        currentItem = Instantiate(selectedItem.prefab, itemAnchor);

        currentItem.transform.localPosition = Vector3.zero;

        WorldItem newWorldItem = currentItem.GetComponent<WorldItem>();

        if (newWorldItem != null)
        {
            currentItem.transform.localRotation = Quaternion.Euler(newWorldItem.placedRotation);
            currentItem.transform.localScale = newWorldItem.placedScale;
        }
        else
        {
            currentItem.transform.localRotation = Quaternion.identity;
            currentItem.transform.localScale = Vector3.one;
        }

        inventory.RemoveSelectedItem();
    }
}
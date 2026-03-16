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

        currentItem = Instantiate(
             selectedItem.prefab,
             itemAnchor.position,
             itemAnchor.rotation
         );

        WorldItem newWorldItem = currentItem.GetComponent<WorldItem>();

        if (newWorldItem != null)
        {
            ItemData data = newWorldItem.item;

            currentItem.transform.rotation = Quaternion.Euler(data.rotation);
            currentItem.transform.localScale = data.scale;
        }

        inventory.RemoveSelectedItem();
    }
}
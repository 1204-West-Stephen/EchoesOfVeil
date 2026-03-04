using UnityEngine;

public class GuardianItem : MonoBehaviour, i_Interactable
{
    public ItemData item;
    private bool itemPickedUp = false;

    public Transform itemPos;
    private Transform currentItemPos;
    private GameObject placedObject;

    private Inventory inventory;
    private Collider col;
    private Renderer[] renderers;

    private void Start()
    {
        currentItemPos = itemPos;     
    }

    private void Awake()
    {
        inventory = FindFirstObjectByType<Inventory>();
        col = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();

        if (itemPos == null)
        {
            Debug.LogError($"{name} has no ItemPos assigned!");
            itemPos = currentItemPos;
        }
    }

    public void Interact()
    {
        if (inventory == null) return;

        if (!itemPickedUp && item != null)
        {
            if (!inventory.CheckInventory()) return;

            inventory.AddItem(item);
            itemPickedUp = true;
            item = null;

            if (col != null) col.enabled = false;
            foreach (var r in renderers) r.enabled = false;
        }
        else
        {
            PlaceItem();
        }
    }

    private void PlaceItem()
    {
        ItemData selectedItem = inventory.GetSelectedItem();
        if (selectedItem == null) return;
        if (selectedItem.typeInput != InputType.GuardianItem) return;

        if (placedObject != null) Destroy(placedObject);

        placedObject = Instantiate(
            selectedItem.prefab,
            itemPos.position,
            itemPos.rotation,
            itemPos
        );

        inventory.RemoveSelectedItem();
        itemPickedUp = false;
    }
}
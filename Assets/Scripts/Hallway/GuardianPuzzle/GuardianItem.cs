using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemData item;

    [Header("Placement Settings")]
    public Transform placementPivot;   // Pivot that defines correct upright placement
    public AudioClip place;

    private Inventory inventory;

    private void Awake()
    {
        inventory = FindFirstObjectByType<Inventory>();
    }

    public void Interact()
    {
        if (!inventory.CheckInventory()) return;

        if (place != null)
        {
            AudioSource.PlayClipAtPoint(place, transform.position, 0.2f);
        }

        inventory.AddItem(item);
        Destroy(gameObject);
    }
}
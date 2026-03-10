using UnityEngine;

public class WorldItem : MonoBehaviour, i_Interactable
{
    public ItemData item;

    [Header("Placement Settings")]
    public Vector3 placedRotation;
    public Vector3 placedScale = Vector3.one;
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
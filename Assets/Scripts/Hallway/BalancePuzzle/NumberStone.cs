using UnityEngine;

public class NumberStone : MonoBehaviour, i_Interactable
{
    public ItemData item;
    private GameObject player;
    private Inventory inventory;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();
    }

    public void Interact()
    {
        if (inventory != null && inventory.CheckInventory())
        {
            inventory.AddItem(item);
            gameObject.SetActive(false);
            Debug.Log($"Picked up stone with value {item.stoneValue}");
        }
        else
        {
            Debug.LogWarning("Inventory full, cannot pick up stone!");
        }
    }

    public void Respawn(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }
}

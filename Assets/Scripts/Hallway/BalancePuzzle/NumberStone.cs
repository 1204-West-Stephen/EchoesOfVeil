using UnityEngine;

public class NumberStone : MonoBehaviour, i_Interactable
{
    public ItemData item;
    private GameObject player;
    private Inventory inventory;
    private AudioSource source;
    public AudioClip clip;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();
    }

    public void Interact()
    {
        if (inventory != null && inventory.CheckInventory())
        {
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, transform.position, 0.15f);
            }
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

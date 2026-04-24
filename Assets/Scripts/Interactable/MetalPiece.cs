using UnityEngine;
using UnityEngine.Audio;

public class MetalPiece : MonoBehaviour, i_Interactable
{
    public ItemData item;

    public bool itemPickedUp = false;
    private PlayerControls playerManager;
    private AudioSource source;
    public AudioClip pickup;

    public Material newLeft;
    public Material newRight;

    private void Awake()
    {
        playerManager = FindFirstObjectByType<PlayerControls>();
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                if (inventory.CheckInventory())
                {
                    if (pickup != null)
                    {
                        AudioSource.PlayClipAtPoint(pickup, transform.position, 0.05f);
                    }

                    inventory.AddItem(item);
                    gameObject.SetActive(false);
                    itemPickedUp = true;
                }

                if (itemPickedUp)
                {
                    playerManager.StartDialogue("I should be able to pry something with this...");
                    JournalStateManager.Instance.UpdateMaterial(newLeft, newRight);
                }
            }
            else
            {
                Debug.LogWarning("Player has no Inventory component.");
            }
        }
    }
}


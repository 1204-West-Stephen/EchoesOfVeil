using UnityEngine;

public class MetalPiece : MonoBehaviour, i_Interactable
{
    public ItemData item;

    public bool itemPickedUp = false;
    private PlayerControls playerManager;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerControls>();
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
                    inventory.AddItem(item);
                    gameObject.SetActive(false);
                    itemPickedUp = true;
                }

                if (itemPickedUp)
                {
                    playerManager.StartDialogue("I should be able to pry something with this...");
                }
            }
            else
            {
                Debug.LogWarning("Player has no Inventory component.");
            }
        }
    }

    public void DetectPlayer() { }
    public void ShowUI() { }
    public void HideUI() { }
}


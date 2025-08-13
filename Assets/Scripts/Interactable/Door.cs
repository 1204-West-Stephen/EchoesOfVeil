using UnityEngine;

public class Door : MonoBehaviour, i_Interactable
{
    private Animator animator;
    private bool isOpen;
    public int doorID;

    private PlayerControls playerManager;
    private Inventory inventory;
    private Hotbar hotbar;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerControls>();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            inventory = player.GetComponent<Inventory>();
            hotbar = player.GetComponent<Hotbar>();
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        isOpen = false;
    }

    public void Interact()
    {
        if (inventory == null || hotbar == null)
        {
            Debug.LogWarning("Inventory or Hotbar not found on player.");
            return;
        }

        if (UseSelectedKey())
        {
            Debug.Log("Correct key used. Door unlocked.");
            PlayAnimation();
        }
        else
        {
            playerManager?.StartDialogue("This door needs a key...");
        }
    }

    private bool UseSelectedKey()
    {
        int selectedIndex = hotbar.selectedIndex;

        if (selectedIndex < 0 || selectedIndex >= inventory.inventory.Count)
            return false;

        ItemData selectedItem = inventory.inventory[selectedIndex];

        if (selectedItem != null && selectedItem.typeInput == InputType.Key && selectedItem.keyID == doorID)
        {
            inventory.RemoveItem(selectedItem);
            Debug.Log($"Key with ID {doorID} consumed and removed from inventory.");
            return true;
        }

        return false;
    }

    private void PlayAnimation()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            animator.SetTrigger("Open");
        }
        else
        {
            animator.SetTrigger("Close");
        }
    }

    public InputType GetRequiredInputType()
    {
        return InputType.Key;
    }
}


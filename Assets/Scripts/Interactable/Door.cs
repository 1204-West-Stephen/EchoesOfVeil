using UnityEngine;

public class Door : MonoBehaviour, i_Interactable
{
    private Animator animator;
    private bool isOpen;
    public int doorID;

    private PlayerControls playerManager;

    private void Awake()
    {
        playerManager = FindFirstObjectByType<PlayerControls>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        isOpen = false;
    }

    public void Interact()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                if (isOpen)
                {
                    return;
                }

                if (UseKey(inventory))
                {
                    Debug.Log("Correct key used. Door unlocked.");
                    PlayAnimation();
                }
                else
                {
                    playerManager.StartDialogue("This door needs a key...");
                }
            }
        }
    }
    private bool UseKey(Inventory inventory)
    {
        ItemData selectedItem = inventory.GetSelectedItem();

        if (selectedItem == null) return false;

        if (selectedItem.typeInput == GetRequiredInputType() && selectedItem.keyID == doorID)
        {
            inventory.RemoveSelectedItem();
            Debug.Log("Key consumed.");
            return true;
        }

        return false;
    }

    private void PlayAnimation()
    {
        isOpen = true;
        animator.SetTrigger("Open");
    }

    public InputType GetRequiredInputType()
    {
        return InputType.Key;
    }
}


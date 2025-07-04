using UnityEngine;

public class Door : MonoBehaviour, i_Interactable
{
    private Animator animator;
    private bool isOpen;
    public int doorID;

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
                if (UseKey(inventory))
                {
                    Debug.Log("Correct key used. Door unlocked.");
                    PlayAnimation();
                }
                else
                {
                    Debug.Log("You need the correct key to open this door.");
                }
            }
        }
    }

    private bool UseKey(Inventory inventory)
    {
        foreach (ItemData item in inventory.inventory)
        {
            if (item.typeInput == InputType.Key && item.keyID == doorID)
            {
                inventory.RemoveItem(item);
                Debug.Log($"Key with ID {doorID} consumed and removed from inventory.");
                return true;
            }
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

    public void DetectPlayer() { }
    public void ShowUI() { }
    public void HideUI() { }
}

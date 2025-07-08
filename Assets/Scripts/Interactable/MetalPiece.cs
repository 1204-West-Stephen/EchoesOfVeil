using UnityEngine;

public class MetalPiece : MonoBehaviour, i_Interactable
{
    public ScriptableObject item;

    public bool itemPickedUp = false;

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
                    Destroy(gameObject);
                    itemPickedUp = true;
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


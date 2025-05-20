using UnityEngine;

public class Item : MonoBehaviour, i_Interactable
{
    public ScriptableObject item; // the data for the item

    public void Interact()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.TryAddItem(item);
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

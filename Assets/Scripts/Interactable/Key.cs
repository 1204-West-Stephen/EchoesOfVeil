using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, i_Interactable
{
    public ItemData item;
    public int keyID;

    private void Start()
    {
        keyID = item.keyID;
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
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogWarning("Player has no Inventory component.");
            }
        }
    }
    public InputType GetRequiredInputType()
    {
        return InputType.None; ;
    }
}

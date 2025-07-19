using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class TorchPuzzle : MonoBehaviour, i_Interactable
{
    public ItemData item;
    public bool itemPickedUp;

    private void Start()
    {
        itemPickedUp = false;
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
                    itemPickedUp = true;
                    gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Player has no Inventory component.");
            }
        }
    }
    public void DetectPlayer()
    {
        
    }
    public void ShowUI()
    {

    }
    public void HideUI() 
    { 
    
    }
}

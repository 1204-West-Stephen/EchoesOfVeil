using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchHilt : MonoBehaviour, i_Interactable 
{
    public GameObject fire;
    private TorchPuzzle torch;
    public void Interact()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                if (torch.itemPickedUp && UsePiece(inventory))
                {
                    fire.SetActive(true);
                    transform.rotation = Quaternion.Euler(0, 180, 0);

                }
                else
                {
                    Debug.Log("Player is unable");
                }
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
    private bool UsePiece(Inventory inventory)
    {
        foreach (ItemData item in inventory.inventory)
        {
            if (item.typeInput == InputType.None && item.itemName == "Lit Torch")
            {
                inventory.RemoveItem(item);
                Debug.Log($"Torch consumed and removed from inventory.");
                return true;
            }
        }
        return false;
    }
}

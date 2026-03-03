using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchHilt : MonoBehaviour, i_Interactable 
{
    public GameObject fire;
    public TorchPuzzle torch;

    public bool puzzleComplete;

    private void Start()
    {
        fire.SetActive(false);

        puzzleComplete = false;
    }

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
                    transform.rotation = Quaternion.Euler(-90, 180, -90);
                    puzzleComplete = true;

                }
                else
                {
                    Debug.Log("Player is unable");
                }
            }
        }
    }
    private bool UsePiece(Inventory inventory)
    {
        ItemData selectedItem = inventory.GetSelectedItem();

        if (selectedItem == null) return false;

        if (selectedItem.typeInput == GetRequiredInputType() && selectedItem.itemName == "Torch")
        {
            inventory.RemoveSelectedItem();
            Debug.Log("Torch consumed.");
            return true;
        }

        return false;
    }
    public InputType GetRequiredInputType()
    {
        return InputType.TorchHilt; 
    }
}

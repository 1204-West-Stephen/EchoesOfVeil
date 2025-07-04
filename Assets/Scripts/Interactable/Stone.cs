using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Stone : MonoBehaviour, i_Interactable
{
    private Animator animator;
    public MetalPiece piece;
    public bool stoneFell = false;

    private MeshCollider MeshCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        MeshCollider = GetComponent<MeshCollider>();
    }
    public void Interact()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                if (piece.itemPickedUp && UsePiece(inventory))
                {
                    animator.SetTrigger("Interacted");
                    stoneFell = true;
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
            if (item.typeInput == InputType.None && item.itemName == "Sharp Metal Piece")
            {
                inventory.RemoveItem(item);
                Debug.Log($"Metal Piece consumed and removed from inventory.");
                return true;
            }
        }
            return false;
    }
}



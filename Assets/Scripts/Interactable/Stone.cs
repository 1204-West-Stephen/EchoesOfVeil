using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, i_Interactable
{
    private Animator animator;
    public MetalPiece piece;
    public bool stoneFell = false;

    private MeshCollider MeshCollider;
    private PlayerControls controls;

    private void Awake()
    {
        controls = FindFirstObjectByType<PlayerControls>();
    }

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
                if (stoneFell)
                {
                    return;
                }

                if (piece.itemPickedUp && UsePiece(inventory))
                {
                    animator.SetTrigger("Interacted");
                    stoneFell = true;
                }
                else {
                    controls.StartDialogue("This stone is a little loose. There may be something behind it");
                }
            }
        }
    }
    private bool UsePiece(Inventory inventory)
    {
        ItemData selectedItem = inventory.GetSelectedItem();

        if (selectedItem == null) return false;

        if (selectedItem.typeInput == GetRequiredInputType() && selectedItem.itemName == "Sharp Metal Piece")
        {
            inventory.RemoveSelectedItem();
            Debug.Log("Torch consumed.");
            return true;
        }

        return false;
    }

    public InputType GetRequiredInputType()
    {
        return InputType.MetalPiece;
    }
}



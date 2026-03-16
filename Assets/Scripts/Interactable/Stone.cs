using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, i_Interactable
{
    public MetalPiece piece;
    public bool stoneFell = false;

    private PlayerControls controls;
    private Rigidbody rb;

    private void Awake()
    {
        controls = FindFirstObjectByType<PlayerControls>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;   // disables physics
        rb.useGravity = false;
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
                    rb.isKinematic = false;   // turn physics ON
                    rb.useGravity = true;

                    rb.AddForce(Vector3.down * 0.005f, ForceMode.Impulse);
                    rb.WakeUp();

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



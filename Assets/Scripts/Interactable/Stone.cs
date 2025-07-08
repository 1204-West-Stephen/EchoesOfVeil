using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, i_Interactable
{
    private Animator animator;
    public MetalPiece piece;
    public bool stoneFell = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Interact()
    {
        if (piece.itemPickedUp)
        {
            animator.SetTrigger("Interacted");
            stoneFell = true;
        }
        else
        {
            Debug.Log("Player is unable");
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



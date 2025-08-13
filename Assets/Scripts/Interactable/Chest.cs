using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, i_Interactable
{
    private Animator animator;

    private bool isOpen;

    private void Start()
    {
        animator = GetComponent<Animator>();

        isOpen = false;
    }

    public void Interact()
    {
        if (isOpen)
        {
            animator.SetTrigger("Close");
            isOpen = false;
        }
        else if (!isOpen)
        {
            animator.SetTrigger("Open");
            isOpen = true;
        }
    }

    public InputType GetRequiredInputType()
    {
        return InputType.None;
    }

}

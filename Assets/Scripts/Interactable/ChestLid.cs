using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChestLid: MonoBehaviour
{
    private Animator animator;
    bool isOpen;
    private void Start()
    {
        isOpen = false;
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        if (isOpen)
        {
            animator.SetTrigger("Close");
            isOpen = false;
        }
        else
        {
            animator.SetTrigger("Open");
            isOpen = true;
        }
    }
}

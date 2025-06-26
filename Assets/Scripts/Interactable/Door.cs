using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour, i_Interactable
{
    private Animator animator;

    private bool isOpen;

    public int doorID;

    private void Start()
    {
        animator = GetComponent<Animator>();

        isOpen = false;
    }

    public void Interact()
    {
        Debug.Log("Interacted with Door");
        PlayAnimation();
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

    private void PlayAnimation()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            animator.SetTrigger("Open");
        }
        else if (!isOpen)
        {
            animator.SetTrigger("Close");
        }
    }
}

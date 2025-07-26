using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonDoorExit : MonoBehaviour
{
    public TorchHilt hilt;

    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hilt.puzzleComplete)
            OpenDoor();
    }

    private void OpenDoor()
    {
        animator.SetTrigger("Start");
    }
}

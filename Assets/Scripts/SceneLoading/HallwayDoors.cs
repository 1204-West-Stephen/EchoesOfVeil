using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayDoors : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void CloseDoor()
    {
        animator.SetTrigger("Close");
    }
}

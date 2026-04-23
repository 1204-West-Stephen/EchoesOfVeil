using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayDoors : MonoBehaviour
{
    private AudioSource source;
    private Animator animator;
    public AudioClip clip;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void CloseDoor()
    {
        animator.SetTrigger("Close");

        if (source != null && clip != null)
        {
            source.PlayOneShot(clip);
        }
    }
}

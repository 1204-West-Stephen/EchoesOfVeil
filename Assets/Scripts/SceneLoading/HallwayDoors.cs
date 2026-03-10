using System.Collections;
using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;
using static Unity.VisualScripting.Member;

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

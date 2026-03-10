using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PrisonDoorExit : MonoBehaviour
{
    private Animator animator;
    private AudioSource source;
    public AudioClip clip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }
    public IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(1.2f);

        animator.SetTrigger("Start");

        if (source != null && clip != null)
        {
            source.PlayOneShot(clip);
        }

        yield return null;
    }
}

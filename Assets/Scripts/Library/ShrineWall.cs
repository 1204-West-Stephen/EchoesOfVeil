using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineWall : MonoBehaviour
{
    public PillarPuzzleManager ppm;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void CheckPuzzle()
    {
        if (ppm.puzzleSolved)
        {
            animator.SetTrigger("Lower");
            Debug.Log("Element puzzle solved");
        }
    }
}

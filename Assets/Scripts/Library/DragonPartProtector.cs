using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DragonPartProtector : MonoBehaviour
{
    public SeasonShrineManager ssm;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void CheckShrinePuzzle()
    {
        if (ssm.puzzleSolved) { 
            animator.SetTrigger("Start");
            Debug.Log("Season puzzle solved");
        }
    }

}

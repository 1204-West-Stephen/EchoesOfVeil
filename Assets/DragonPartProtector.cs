using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DragonPartProtector : MonoBehaviour
{
    public SeasonShrineManager ssm;

    public bool[] puzzleCompleted;

    private Animator animator;

    private void Start()
    {
        puzzleCompleted = new bool[4];

        for (int i = 0; i < puzzleCompleted.Length; i++)
        {
            puzzleCompleted[i] = false;
        }

        animator = GetComponent<Animator>();
    }

    public void CheckShrinePuzzle()
    {
        if (ssm.puzzleSolved && !puzzleCompleted[0])
        {
            puzzleCompleted[0] = true;
        }

        if (puzzleCompleted.All(p => p))
        {
            //unlock dragon part
        }
    }

}

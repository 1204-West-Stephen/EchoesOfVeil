using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemManager : MonoBehaviour
{
    public LetterTotem[] totems;
    public bool puzzleSolved;
    private HallwayGameManager hgm;
    public Camera solvedCamera;

    private void Start()
    {
        hgm = FindFirstObjectByType<HallwayGameManager>();
    }

    public void CheckIfSolved()
    {
        foreach (LetterTotem totem in totems)
        {
            if (totem.answerSprite != totem.currentSprite.sprite)
            {
                puzzleSolved = false;
                return;
            }
        }

        puzzleSolved = true;
        foreach (LetterTotem totem in totems)
        {
            totem.canInteract = false;
        }
        FindFirstObjectByType<HallwayGameManager>()?.CheckHallwayPuzzles();
        StartCoroutine(hgm.ShowLight(solvedCamera));
        Debug.Log("All totems are correct! Puzzle solved!");
    }
}

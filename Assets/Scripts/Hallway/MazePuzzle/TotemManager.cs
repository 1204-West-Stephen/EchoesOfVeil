using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemManager : MonoBehaviour
{
    public LetterTotem[] totems;
    public bool puzzleSolved;

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
        FindFirstObjectByType<HallwayGameManager>()?.CheckHallwayPuzzles();
        Debug.Log("All totems are correct! Puzzle solved!");
    }
}

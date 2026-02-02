using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonShrineManager : MonoBehaviour
{
    public SeasonShrine[] shrines;
    public bool puzzleSolved;

    public void CheckIfSolved()
    {
        foreach (SeasonShrine shrine in shrines)
        {
            // Fail if nothing is placed OR the wrong item is placed
            if (shrine.placedObjectData == null || shrine.placedObjectData != shrine.correctAnswerData)
            {
                puzzleSolved = false;
                return;
            }
        }

        puzzleSolved = true;

        FindFirstObjectByType<DragonCamera>()?.StartCamera();
        FindFirstObjectByType<DragonPartProtector>()?.CheckShrinePuzzle();
        Debug.Log("All towers are correct! Puzzle solved!");
    }
}

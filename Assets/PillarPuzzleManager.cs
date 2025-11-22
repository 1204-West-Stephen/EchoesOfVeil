using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarPuzzleManager : MonoBehaviour
{
    public ElementalPillar[] pillars;
    public bool puzzleSolved;

    public void CheckIfSolved()
    {
        foreach (ElementalPillar pillar in pillars)
        {
            // Fail if nothing is placed OR the wrong item is placed
            if (pillar.placedObjectData == null || pillar.placedObjectData != pillar.correctAnswerData)
            {
                puzzleSolved = false;
                return;
            }
        }

        puzzleSolved = true;

        FindObjectOfType<ShrineWall>()?.CheckPuzzle();
        Debug.Log("All towers are correct! Puzzle solved!");
    }
}

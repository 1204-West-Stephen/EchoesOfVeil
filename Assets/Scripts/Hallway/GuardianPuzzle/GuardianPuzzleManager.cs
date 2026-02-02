using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianPuzzleManager : MonoBehaviour
{
    public GuardianMatchTower[] towers;
    public GuardianRiddleStone[] stones;
    public bool puzzleSolved;
    public bool first = true;

    public void CheckIfSolved()
    {
        foreach (GuardianMatchTower tower in towers)
        {
            // Fail if nothing is placed OR the wrong item is placed
            if (tower.placedObjectData == null || tower.placedObjectData != tower.correctAnswerData)
            {
                puzzleSolved = false;
                return;
            }
        }

        puzzleSolved = true;

        FindFirstObjectByType<HallwayGameManager>()?.CheckHallwayPuzzles();
        Debug.Log("All towers are correct! Puzzle solved!");
    }

    public bool CheckInteractions()
    {
        foreach (GuardianRiddleStone stone in stones)
        {
            if (stone.interacting)
            {
                return false;
            }

            if (!stone.firstInteract)
            {
                first = false;
            }
        }

        return true;

    }
}

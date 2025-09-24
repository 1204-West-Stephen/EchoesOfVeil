using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianPuzzleManager : MonoBehaviour
{
    public GuardianMatchTower[] towers;
    public bool puzzleSolved;

    public void CheckIfSolved()
    {
        foreach (GuardianMatchTower tower in towers)
        {
            if (tower.placedObject != tower.answerObject)
            {
                puzzleSolved = false;
                return;
            }
        }

        puzzleSolved = true;

        FindObjectOfType<HallwayGameManager>()?.CheckHallwayPuzzles();
        Debug.Log("All towers are correct! Puzzle solved!");
    }
}

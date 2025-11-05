using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtPuzzleManager : MonoBehaviour
{

    public bool puzzleSolved;

    public Image activeSprite;

    public void CheckIfSolved()
    {
        puzzleSolved = true;

        FindObjectOfType<HallwayGameManager>()?.CheckHallwayPuzzles();
        Debug.Log("All towers are correct! Puzzle solved!");
    }
}

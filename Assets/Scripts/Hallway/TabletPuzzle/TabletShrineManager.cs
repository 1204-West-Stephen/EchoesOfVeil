using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletShrineManager : MonoBehaviour
{
    public TabletShrine[] shrines;
    public bool puzzleSolved;

    public void CheckIfSolved()
    {
        foreach (TabletShrine shrine in shrines)
        {
            if (shrine.currentTabletNum != shrine.tabletShrineNum)
            {
                puzzleSolved = false;
                return;
            }
        }

        puzzleSolved = true;
        FindFirstObjectByType<HallwayGameManager>()?.CheckHallwayPuzzles();
        Debug.Log("All shrines matched! Puzzle solved!");
    }
}


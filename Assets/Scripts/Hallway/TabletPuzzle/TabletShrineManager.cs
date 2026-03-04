using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletShrineManager : MonoBehaviour
{
    public TabletShrine[] shrines;
    public bool puzzleSolved;
    public Camera solvedCamera;
    private Camera playerCamera;
    private HallwayGameManager hgm;

    private void Start()
    {
        solvedCamera.gameObject.SetActive(false);
        hgm = FindFirstObjectByType<HallwayGameManager>();
    }

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
        StartCoroutine(hgm.ShowLight(solvedCamera));
        FindFirstObjectByType<HallwayGameManager>()?.CheckHallwayPuzzles();
        Debug.Log("All shrines matched! Puzzle solved!");
    }
}


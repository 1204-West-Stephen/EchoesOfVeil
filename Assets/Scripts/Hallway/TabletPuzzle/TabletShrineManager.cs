using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletShrineManager : MonoBehaviour
{
    public TabletShrine[] shrines;
    public bool puzzleSolved;
    public Camera solvedCamera;
    private Camera playerCamera;
    private PlayerMovement movement;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        movement = FindFirstObjectByType<PlayerMovement>();
        playerCamera = playerObj.GetComponentInChildren<Camera>();
        solvedCamera.gameObject.SetActive(false);
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
        StartCoroutine(ShowLight());
        FindFirstObjectByType<HallwayGameManager>()?.CheckHallwayPuzzles();
        Debug.Log("All shrines matched! Puzzle solved!");
    }

    private IEnumerator ShowLight()
    {
        movement.controlLock();
        yield return new WaitForSeconds(1.5f);
        solvedCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        yield return new WaitForSeconds(3.5f);

        playerCamera.gameObject.SetActive(true);
        solvedCamera.gameObject.SetActive(false);
        movement.controlUnlock();

        yield return null;
    }
}


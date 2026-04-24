using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarPuzzleManager : MonoBehaviour
{
    public ElementalPillar[] pillars;
    public bool puzzleSolved;
    public Camera solvedCamera;

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

        StartCoroutine(ShowLight(solvedCamera));
        Debug.Log("All towers are correct! Puzzle solved!");
    }

    public IEnumerator ShowLight(Camera solvedCamera)
    {
        PlayerMovement movement = null;
        Camera playerCamera = null;

        // Wait for player to spawn
        while (movement == null)
        {
            movement = FindFirstObjectByType<PlayerMovement>();
            yield return null;
        }

        // Wait for camera to exist
        while (playerCamera == null)
        {
            playerCamera = movement.GetComponentInChildren<Camera>();
            yield return null;
        }

        if (solvedCamera == null)
        {
            Debug.LogError("Solved camera not assigned!");
            yield break;
        }

        movement.controlLock();
        FindFirstObjectByType<ShrineWall>()?.CheckPuzzle();

        yield return new WaitForSeconds(1.5f);

        solvedCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        FindFirstObjectByType<ShrineWall>()?.CheckPuzzle();

        yield return new WaitForSeconds(4.5f);

        playerCamera.gameObject.SetActive(true);
        solvedCamera.gameObject.SetActive(false);

        movement.controlUnlock();
    }
}

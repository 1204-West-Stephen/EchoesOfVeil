using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonShrineManager : MonoBehaviour
{
    public SeasonShrine[] shrines;
    public bool puzzleSolved;
    public Camera solvedCamera;
    public void CheckIfSolved()
    {
        foreach (SeasonShrine shrine in shrines)
        {
            if (shrine.placedObjectData == null || shrine.placedObjectData != shrine.correctAnswerData)
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

        yield return new WaitForSeconds(1.5f);

        solvedCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        FindFirstObjectByType<DragonPartProtector>()?.CheckShrinePuzzle();

        yield return new WaitForSeconds(4.5f);

        playerCamera.gameObject.SetActive(true);
        solvedCamera.gameObject.SetActive(false);

        movement.controlUnlock();
    }
}

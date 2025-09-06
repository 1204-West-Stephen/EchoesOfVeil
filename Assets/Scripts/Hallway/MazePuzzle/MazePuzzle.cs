using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePuzzle : MonoBehaviour, i_Interactable
{
    public Camera puzzleCamera;

    private PlayerControls player;
    private PlayerMovement playerMovement;
    private bool inPuzzleView;

    private void Start()
    {
        puzzleCamera.gameObject.SetActive(false);
        inPuzzleView = false;
    }

    public void Interact()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerControls>();
            playerMovement = FindObjectOfType<PlayerMovement>();
        }

        if (player != null && playerMovement != null)
        {
            EnterPuzzleView();
        }
    }

    private void Update()
    {
        if (inPuzzleView && player.interacted)
        {
            ExitPuzzleView();
        }
    }

    private void EnterPuzzleView()
    {
        puzzleCamera.gameObject.SetActive(true);
        player.DisableControls();
        inPuzzleView = true;
        player.hotbar.gameObject.SetActive(false);
        player.crosshair.gameObject.SetActive(false);
        player.interactionCanvas.gameObject.SetActive(false);
        playerMovement.staminaBar.gameObject.SetActive(false);
        playerMovement.staminaBarImage.gameObject.SetActive(false);
        playerMovement.staminaBarBackground.gameObject.SetActive(false);
        player.canInteract = false;
    }

    private void ExitPuzzleView()
    {
        puzzleCamera.gameObject.SetActive(false);
        player.EnableControls();
        inPuzzleView = false;
        player.hotbar.gameObject.SetActive(true);
        player.crosshair.gameObject.SetActive(true);
        player.interactionCanvas.gameObject.SetActive(true);
        playerMovement.staminaBar.gameObject.SetActive(true);
        playerMovement.staminaBarImage.gameObject.SetActive(true);
        playerMovement.staminaBarBackground.gameObject.SetActive(true);
        player.canInteract = true;
    }
}

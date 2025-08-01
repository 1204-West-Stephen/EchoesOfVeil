using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour, i_Interactable
{
    public bool journalAcquired = false;

    // Reference to the player
    private PlayerControls playerControls;

    private void Awake()
    {
        // Find the player in the scene
        playerControls = FindObjectOfType<PlayerControls>();
    }

    public void Interact()
    {
        // Add the Journal component to the player
        Journal playerJournal = playerControls.gameObject.AddComponent<Journal>();
        playerJournal.journalAcquired = true;

        // Notify PlayerControls that the journal is now available
        playerControls.OnJournalAcquired(playerJournal);

        // Destroy the pickup object in the world
        Destroy(gameObject);
    }
}

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
        playerControls = FindObjectOfType<PlayerControls>();
    }

    public void Interact()
    {
        Journal playerJournal = playerControls.gameObject.AddComponent<Journal>();
        playerJournal.journalAcquired = true;
        playerControls.OnJournalAcquired(playerJournal);
        Destroy(gameObject);
    }
    public InputType GetRequiredInputType()
    {
        return InputType.None;
    }
}

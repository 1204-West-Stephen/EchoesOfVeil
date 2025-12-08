using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GuardianRiddleStone : MonoBehaviour, i_Interactable
{
    public string TotemInformation;
    public GuardianPuzzleManager manager;
    private PlayerControls playerControls;

    private bool firstInteract = true;
    public bool interacting = false;
    public void Interact()
    {
        if (manager.CheckInteractions()) { 

            if (playerControls == null)
                playerControls = FindObjectOfType<PlayerControls>();

            StartCoroutine(Dialogue(TotemInformation));
        }
    }

    private IEnumerator Dialogue(string text)
    {
        interacting = true;

        playerControls.StartDialogue(text);

        yield return new WaitForSeconds(4f);

        if (firstInteract)
        {
            playerControls.StartDialogue("The tablets are so worn, I can only decipher\nsome of the words");
            firstInteract = false;
        }

        interacting = false;
    }
}

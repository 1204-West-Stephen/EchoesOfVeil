using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GuardianRiddleStone : MonoBehaviour, i_Interactable
{
    public string TotemInformation;
    private PlayerControls playerControls;

    private bool firstInteract = true;
    public void Interact()
    {
        if (playerControls == null)
            playerControls = FindObjectOfType<PlayerControls>();

        StartCoroutine(Dialogue(TotemInformation));
    }

    private IEnumerator Dialogue(string text)
    {
        playerControls.StartDialogue(text);

        yield return new WaitForSeconds(4f);

        if (firstInteract)
        {
            playerControls.StartDialogue("The tablets are so worn, I can only decipher\nsome of the words");
            firstInteract = false;
        }
    }
}

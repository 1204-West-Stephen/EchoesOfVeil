using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class ArtPieceTwo : MonoBehaviour, i_Interactable
{
    public bool interacted = false;
    public ArtPieceOne piece1;
    public ArtPieceThree piece3;
    private PlayerControls controls;

    private void Awake()
    {
        controls = FindFirstObjectByType<PlayerControls>();
    }

    public void Interact()
    {
        if (!interacted && !piece1.interacted && !piece3.interacted)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                StartCoroutine(StartDialogue());
            }
        }
    }

    private IEnumerator StartDialogue()
    {
        interacted = true;

        yield return new WaitForSeconds(0.5f);

        controls.StartDialogue("This mural says \" The chaos of the war will cause an awakening within the temple.");

        yield return new WaitForSeconds(3.5f);

        controls.StartDialogue("The Pulse of the Dragon will flow stronger than before, and its power unpredicatable.\"");

        yield return new WaitForSeconds(3.5f);

        controls.StartDialogue("I certainly don't want to be in here if that happens. I need to get out of here. Fast.");

        yield return new WaitForSeconds(3.5f);

        interacted = false;
        yield return new WaitForSeconds(0.5f);

    }
}

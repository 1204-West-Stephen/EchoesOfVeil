using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class ArtPieceThree : MonoBehaviour, i_Interactable
{
    public bool interacted = false;
    private PlayerControls controls;
    public ArtPieceOne piece1;
    public ArtPieceTwo piece2;

    private void Awake()
    {
        controls = FindFirstObjectByType<PlayerControls>();
    }

    public void Interact()
    {
        if (!interacted && !piece2.interacted && !piece1.interacted)
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

        controls.StartDialogue("This mural says \"The Dragon's power will link body and soul");

        yield return new WaitForSeconds(3.5f);

        controls.StartDialogue("and the soul's energy will be absorbed by thee.\"");

        yield return new WaitForSeconds(3.5f);

        controls.StartDialogue("That's the ancient symbol for a perfect being.");

        yield return new WaitForSeconds(3.5f);

        controls.StartDialogue("One who is all powerful, one who possesses more power than the Dragon's Pulse...");
        
        yield return new WaitForSeconds(3.5f);

        interacted = false;
        yield return new WaitForSeconds(0.5f);

    }
}

using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class ArtPieceOne : MonoBehaviour, i_Interactable
{
    public bool interacted = false;
    public ArtPieceTwo piece2;
    public ArtPieceThree piece3;
    private PlayerControls controls;

    private void Awake()
    {
        controls = FindObjectOfType<PlayerControls>();
    }

    public void Interact()
    {
        if (!interacted && !piece2.interacted && !piece3.interacted)
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

        controls.StartDialogue("These murals tell of an old story my grandfather use to tell me.");

        yield return new WaitForSeconds(3.5f);

        controls.StartDialogue("The story is in three parts: each a prophecy about the end of the world");

        yield return new WaitForSeconds(3.5f);

        controls.StartDialogue("This mural fortells of a huge war that will break out.");

        yield return new WaitForSeconds(3.5f);

        controls.StartDialogue("\"There will be a war between magic and rage, wand and mace, fire and steel.");

        yield return new WaitForSeconds(3.5f);

        controls.StartDialogue("This war will be brought upon by a deciever, an evil doer of dark arts,");

        yield return new WaitForSeconds(3.5f);

        controls.StartDialogue("who will put the nations against each other for his own gain.\"");

        yield return new WaitForSeconds(3.5f);

        controls.StartDialogue("It seems to me that this may no longer be a prophecy.");

        yield return new WaitForSeconds(3.5f);

        interacted = false;
        yield return new WaitForSeconds(0.5f);

    }
}

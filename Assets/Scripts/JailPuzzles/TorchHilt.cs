using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

public class TorchHilt : MonoBehaviour, i_Interactable 
{
    public GameObject fire;
    public TorchPuzzle torch;
    private AudioSource source;
    public AudioClip clip;

    public List<PrisonDoorExit> doors;

    public bool puzzleComplete;

    private void Start()
    {
        fire.SetActive(false);
        source = GetComponent<AudioSource>();

        puzzleComplete = false;
    }

    public void Interact()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                if (torch.itemPickedUp && UsePiece(inventory))
                {
                    fire.SetActive(true);
                    transform.rotation = Quaternion.Euler(-90, 180, -90);
                    puzzleComplete = true;

                    if (source != null && clip != null)
                    {
                        source.PlayOneShot(clip);
                    }

                    StartCoroutine(doors[0].OpenDoor());
                    StartCoroutine(doors[1].OpenDoor());

                }
                else
                {
                    Debug.Log("Player is unable");
                }
            }
        }
    }
    private bool UsePiece(Inventory inventory)
    {
        ItemData selectedItem = inventory.GetSelectedItem();

        if (selectedItem == null) return false;

        if (selectedItem.typeInput == GetRequiredInputType() && selectedItem.itemName == "Torch")
        {
            inventory.RemoveSelectedItem();
            Debug.Log("Torch consumed.");
            return true;
        }

        return false;
    }
    public InputType GetRequiredInputType()
    {
        return InputType.TorchHilt; 
    }
}

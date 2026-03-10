using System.Collections;
using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.Audio;

public class TorchPuzzle : MonoBehaviour, i_Interactable
{
    public ItemData item;
    public bool itemPickedUp;
    private AudioSource source;
    public AudioClip clip;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        itemPickedUp = false;
    }
    public void Interact()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                if (inventory.CheckInventory())
                {
                    if (clip != null)
                    {
                        AudioSource.PlayClipAtPoint(clip, transform.position, 0.05f);
                    }

                    inventory.AddItem(item);
                    itemPickedUp = true;
                    gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Player has no Inventory component.");
            }
        }
    }
    public InputType GetRequiredInputType()
    {
        return InputType.None;
    }
}

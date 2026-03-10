using System.Collections;
using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.Audio;

public class Key : MonoBehaviour, i_Interactable
{
    public ItemData item;
    public int keyID;
    private AudioSource source;
    public AudioClip keySound;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        keyID = item.keyID;
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
                    if (keySound != null)
                    {
                        AudioSource.PlayClipAtPoint(keySound, transform.position, 0.05f);
                    }

                    inventory.AddItem(item);
                    Destroy(gameObject);
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
        return InputType.None; ;
    }
}

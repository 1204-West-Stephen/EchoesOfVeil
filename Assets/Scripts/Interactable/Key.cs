using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Key : MonoBehaviour, i_Interactable
{
    public ScriptableObject item;
    private bool itemPickedUp;
    private void Start()
    {

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
                    inventory.AddItem(item);
                    Destroy(gameObject);
                    itemPickedUp = true;
                }
            }
            else
            {
                Debug.LogWarning("Player has no Inventory component.");
            }
        }
    }

    public void DetectPlayer()
    {

    }
    public void ShowUI()
    {

    }
    public void HideUI()
    {

    }
}

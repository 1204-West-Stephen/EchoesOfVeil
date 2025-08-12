using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class TabletShrine : MonoBehaviour, i_Interactable
{
    public ItemData item;
    public GameObject tablet;

    public Transform stonePos;
    public int tabletShrineNum;
    public bool canPlace;
    public bool itemPickedUp;

    private GameObject player;
    private Inventory inventory;

    private void Start()
    {
        canPlace = true;

        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();
    }
    public void Interact()
    {
        if (canPlace)
        {
            if (player != null)
            {
                if (player != null)
                {
                    if (inventory != null)
                    {
                        if (PlaceTablet(inventory))
                        {
                            Debug.Log("Tablet placed");
                        }
                        else
                        {
                            Debug.Log("Tablet not placed");
                        }
                    }
                }
            }

            canPlace = false;
            Debug.Log(canPlace);
        }
        else if (!canPlace)
        {
            if (player != null)
            {
                if (inventory != null)
                {
                    if (inventory.CheckInventory())
                    {
                        inventory.AddItem(item);
                        tablet.gameObject.SetActive(false);
                        itemPickedUp = true;
                    }
                }
                else
                {
                    Debug.LogWarning("Player has no Inventory component.");
                }
            }

            canPlace = true;
            Debug.Log(canPlace);
        }
    }

    private bool PlaceTablet(Inventory inventory)
    {
        foreach (ItemData item in inventory.inventory)
        {
            if (item.typeInput == InputType.Tablet)
            {
                inventory.RemoveItem(item);
                Debug.Log($"Stone with ID {tabletShrineNum} consumed and removed from inventory.");
                return true;
            }
        }
        return false;
    }
}

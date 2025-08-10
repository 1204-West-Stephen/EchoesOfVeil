using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class TabletShrine : MonoBehaviour, i_Interactable
{
    public ItemData item;

    public Transform stonePos;
    public int tabletShrineNum;
    public bool canPlace;
    public bool itemPickedUp;

    private void Start()
    {
        canPlace = true;
    }
    public void Interact()
    {
        if (canPlace)
        {
            //check if tablets are in inventory

            //if tablet in inventory, place iteminhand at stonePos
            canPlace = false;
            Debug.Log(canPlace);
        }
        else if (!canPlace)
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
                        gameObject.SetActive(false);
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
            if (item.typeInput == InputType.Tablet)// && item.tabletNumber == tabletShrineNum)
            {
                inventory.RemoveItem(item);
                //stonepos set
                Debug.Log($"Stone with ID {tabletShrineNum} consumed and removed from inventory.");
                return true;
            }
        }
        return false;
    }
}

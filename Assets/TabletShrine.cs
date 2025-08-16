using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletShrine : MonoBehaviour, i_Interactable
{
    public ItemData item;
    public GameObject tablet;

    public Transform stonePos;
    public int tabletShrineNum;
    public int currentTabletNum = -1;

    public bool canPlace;
    public bool itemPickedUp;

    private GameObject player;
    private Inventory inventory;

    private void Start()
    {
        canPlace = false;

        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();

        tablet.transform.position = stonePos.position;
    }

    public void Interact()
    {
        if (canPlace)
        {
            if (PlaceTablet())
            {
                tablet.gameObject.SetActive(true);
                tablet.transform.position = stonePos.position;
                Debug.Log("Tablet placed");
            }
            else
            {
                Debug.Log("Tablet not placed - selected item invalid or none");
            }

            canPlace = false;
            Debug.Log($"canPlace = {canPlace}");
        }
        else
        {
            if (inventory.CheckInventory())
            { 
                inventory.AddItem(item);
                tablet.gameObject.SetActive(false);
                itemPickedUp = true;
                currentTabletNum = -1; 
            }
            else
            {
                Debug.LogWarning("Player or Inventory component missing.");
            }

            canPlace = true;
            Debug.Log($"canPlace = {canPlace}");
        }

        FindObjectOfType<TabletShrineManager>()?.CheckIfSolved();
    }

    private bool PlaceTablet()
    {
        if (item != null && item.typeInput == InputType.Tablet)
        {
            currentTabletNum = item.tabletNumber;
            inventory.RemoveItem(item);
            Debug.Log($"Tablet {currentTabletNum} placed in shrine {tabletShrineNum}");
            return true;
        }

        return false;
    }
}

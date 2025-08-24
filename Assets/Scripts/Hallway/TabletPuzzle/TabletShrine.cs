using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletShrine : MonoBehaviour, i_Interactable
{
    public Transform stonePos;
    public int tabletShrineNum;
    public int currentTabletNum = -1;

    [Header("Starting State")]
    public ItemData startingTablet;

    public bool canPlace;
    public bool itemPickedUp;

    private GameObject player;
    private Inventory inventory;

    private ItemData placedTablet;
    private GameObject placedTabletObject;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();

        if (startingTablet != null && startingTablet.prefab != null)
        {
            placedTablet = startingTablet;
            MoveTabletToShrine(startingTablet.prefab);
            currentTabletNum = startingTablet.tabletNumber;
        }

        canPlace = false;
    }

    public void Interact()
    {
        if (canPlace)
        {
            if (PlaceTablet())
            {
                Debug.Log("Tablet placed");
                canPlace = false;
            }
            else
            {
                Debug.Log("Tablet not placed - no valid tablet in inventory");
            }
        }
        else
        {
            PickUpTablet();
        }

        FindObjectOfType<TabletShrineManager>()?.CheckIfSolved();
    }

    private bool PlaceTablet()
    {
        if (inventory == null || inventory.inventory.Count == 0)
            return false;

        ItemData tabletToPlace = null;
        foreach (var item in inventory.inventory)
        {
            if (item != null && item.typeInput == InputType.Tablet)
            {
                tabletToPlace = item;
                break;
            }
        }

        if (tabletToPlace != null)
        {
            placedTablet = tabletToPlace;
            currentTabletNum = tabletToPlace.tabletNumber;

            inventory.RemoveItem(tabletToPlace);

            MoveTabletToShrine(tabletToPlace.prefab);

            Debug.Log($"Tablet {currentTabletNum} placed in shrine {tabletShrineNum}");
            return true;
        }

        return false;
    }

    private void PickUpTablet()
    {
        if (placedTablet != null && inventory != null && inventory.CheckInventory())
        {
            inventory.AddItem(placedTablet);

            if (placedTabletObject != null)
            {
                Destroy(placedTabletObject); 
                placedTabletObject = null;
            }

            placedTablet = null;
            currentTabletNum = -1;
            itemPickedUp = true;
            canPlace = true;

            Debug.Log("Tablet picked up from shrine " + tabletShrineNum);
        }
        else
        {
            Debug.LogWarning("Cannot pick up tablet: inventory full or missing tablet.");
        }
    }

    private void MoveTabletToShrine(GameObject tabletPrefab)
    {
        if (tabletPrefab != null)
        {
            if (placedTabletObject != null)
            {
                Destroy(placedTabletObject);
            }

            placedTabletObject = Instantiate(
                tabletPrefab,
                stonePos.position,
                Quaternion.Euler(0, 90, 0)
            );

            placedTabletObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Tablet prefab is null, cannot move to shrine!");
        }
    }
}

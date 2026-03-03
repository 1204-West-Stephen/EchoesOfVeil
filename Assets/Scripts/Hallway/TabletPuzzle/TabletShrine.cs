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
    }

    public void Interact()
    {
        if (placedTablet == null)
        {
            if (!PlaceTablet())
                Debug.Log("No valid tablet selected.");
        }
        else
        {
            PickUpTablet();
        }

        FindFirstObjectByType<TabletShrineManager>()?.CheckIfSolved();
    }

    private bool PlaceTablet()
    {
        if (inventory == null) return false;

        ItemData selectedItem = inventory.GetSelectedItem();
        if (selectedItem == null) return false;

        if (selectedItem.typeInput != InputType.Tablet)
            return false;

        placedTablet = selectedItem; // THIS WAS MISSING
        currentTabletNum = selectedItem.tabletNumber;

        inventory.RemoveSelectedItem();

        MoveTabletToShrine(selectedItem.prefab);

        Debug.Log($"Tablet {currentTabletNum} placed in shrine {tabletShrineNum}");

        return true;
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

    private InputType GetRequiredInputType()
    {
        return InputType.Tablet;
    }
}

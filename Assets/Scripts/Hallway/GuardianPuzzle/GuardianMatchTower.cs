using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianMatchTower : MonoBehaviour, i_Interactable
{
    public string towerName;

    public GameObject placedObject;
    public GameObject answerObject;

    public Transform floatPos;

    private GameObject player;
    private Inventory inventory;

    private ItemData placedObjectData;

    public bool canPlace;
    public bool itemPickedUp;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();
    }
    private void Update()
    {
        if (placedObject == null)
        {
            canPlace = true;
        }
        else
        {
            canPlace = false;
        }
    }

    public void Interact()
    {
        if (canPlace)
        {
            if (PlaceObject())
            {
                canPlace = false;
            }
            else
            {
                Debug.Log("Object not placed - no valid object in inventory");
            }
        }
        else
        {
            PickUpObject();
        }

        FindObjectOfType<GuardianPuzzleManager>()?.CheckIfSolved();
    }

    private bool PlaceObject()
    {
        if (inventory == null || inventory.inventory.Count == 0)
            return false;

        ItemData objectToPlace = null;
        foreach (var item in inventory.inventory)
        {
            if (item != null && item.typeInput == InputType.GuardianItem)
            {
                objectToPlace = item;
                break;
            }
        }

        if (objectToPlace != null)
        {
            placedObjectData = objectToPlace;

            inventory.RemoveItem(objectToPlace);

            MoveObjectToShrine(objectToPlace.prefab);

            Debug.Log($"Object {placedObjectData.name} placed in tower {towerName}");
            return true;
        }

        return false;
    }

    private void PickUpObject()
    {
        if (placedObject != null && inventory != null && inventory.CheckInventory())
        {
            inventory.AddItem(placedObjectData);

            if (placedObject != null)
            {
                Destroy(placedObject);
                placedObject = null;
            }

            placedObject = null;
            itemPickedUp = true;
            canPlace = true;

            Debug.Log("Tablet picked up from tower " + towerName);
        }
        else
        {
            Debug.LogWarning("Cannot pick up object: inventory full or missing object.");
        }
    }

    private void MoveObjectToShrine(GameObject objectPrefab)
    {
        if (objectPrefab != null)
        {
            if (placedObject != null)
            {
                Destroy(placedObject);
            }

            placedObject = Instantiate(
                objectPrefab,
                floatPos.position,
                Quaternion.Euler(0, 0, 0)
            );

            placedObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Tablet prefab is null, cannot move to shrine!");
        }
    }
}

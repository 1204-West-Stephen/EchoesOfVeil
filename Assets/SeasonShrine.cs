using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonShrine : MonoBehaviour, i_Interactable
{
    public string towerName;

    public GameObject placedObject;
    public Transform floatPos;

    private GameObject player;
    private Inventory inventory;

    [Header("Answer Setup")]
    public ItemData correctAnswerData;   // Assign the correct item in inspector
    [HideInInspector] public ItemData placedObjectData; // What the player placed

    public bool canPlace;
    public bool itemPickedUp;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();
    }

    private void Update()
    {
        canPlace = placedObject == null;
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

        FindObjectOfType<SeasonShrineManager>()?.CheckIfSolved();
    }

    private bool PlaceObject()
    {
        if (inventory == null || inventory.inventory.Count == 0)
            return false;

        ItemData objectToPlace = null;
        foreach (var item in inventory.inventory)
        {
            if (item != null && item.typeInput == InputType.SeasonItem)
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

            Destroy(placedObject);
            placedObject = null;

            placedObjectData = null; // reset when picked back up
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

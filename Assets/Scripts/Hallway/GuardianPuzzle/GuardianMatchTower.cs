using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianMatchTower : MonoBehaviour, i_Interactable
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

        FindFirstObjectByType<GuardianPuzzleManager>()?.CheckIfSolved();
    }

    private bool PlaceObject()
    {
        ItemData objectToPlace = null;
        ItemData selectedItem = inventory.GetSelectedItem();

        objectToPlace = selectedItem;

        if (selectedItem == null) return false;

        if (selectedItem.typeInput == GetRequiredInputType())
        {
            inventory.RemoveSelectedItem();
            MoveObjectToShrine(objectToPlace.prefab);
            Debug.Log("Key consumed.");
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

            placedObjectData = null;
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

    private InputType GetRequiredInputType()
    {
        return InputType.GuardianItem;
    }
}

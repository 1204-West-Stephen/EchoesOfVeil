using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class SeasonShrine : MonoBehaviour, i_Interactable
{
    public string towerName;

    public GameObject placedObject;
    public Transform floatPos;

    private GameObject player;
    private Inventory inventory;

    [Header("Answer Setup")]
    public ItemData correctAnswerData;
    [HideInInspector] public ItemData placedObjectData;

    public Vector3 rotationOverride;
    private AudioSource source;
    public AudioClip pickup;
    public AudioClip place;

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

        FindFirstObjectByType<SeasonShrineManager>()?.CheckIfSolved();
    }

    private bool PlaceObject()
    {
        ItemData selectedItem = inventory.GetSelectedItem();

        if (selectedItem == null) return false;

        if (selectedItem.typeInput == GetRequiredInputType())
        {
            inventory.RemoveSelectedItem();
            MoveObjectToShrine(selectedItem);

            if (source != null && place != null)
                source.PlayOneShot(place);

            return true;
        }

        return false;
    }

    private void PickUpObject()
    {
        if (placedObject != null && inventory != null && inventory.CheckInventory())
        {
            inventory.AddItem(placedObjectData);

            if (pickup != null)
                AudioSource.PlayClipAtPoint(pickup, transform.position, 0.15f);

            Destroy(placedObject);
            placedObject = null;
            placedObjectData = null;

            itemPickedUp = true;
            canPlace = true;
        }
        else
        {
            Debug.LogWarning("Cannot pick up object: inventory full or missing object.");
        }
    }

    private void MoveObjectToShrine(ItemData itemData)
    {
        if (itemData == null || itemData.prefab == null)
        {
            Debug.LogWarning("Item prefab is null, cannot move to shrine!");
            return;
        }

        if (placedObject != null)
            Destroy(placedObject);

        placedObject = Instantiate(
            itemData.prefab,
            floatPos.position,
            Quaternion.identity
        );

        placedObject.transform.rotation =
            Quaternion.Euler(itemData.rotation + rotationOverride);

        placedObject.transform.localScale = itemData.scale;

        placedObjectData = itemData;
    }

    private InputType GetRequiredInputType()
    {
        return InputType.SeasonItem;
    }
}

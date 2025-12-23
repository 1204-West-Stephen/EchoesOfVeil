using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelf : MonoBehaviour, i_Interactable
{
    public Transform pos;

    [Header("Starting State")]
    public ItemData startingBook;

    public bool canPlace;
    public bool itemPickedUp;

    private GameObject player;
    private Inventory inventory;

    private ItemData placedBook;
    private GameObject placedBookObject;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();

        if (startingBook != null && startingBook.prefab != null)
        {
            placedBook = startingBook;
            MoveBookToShelf(startingBook.prefab);
        }

        canPlace = false;
    }

    public void Interact()
    {
        if (canPlace)
        {
            if (PlaceBook())
            {
                Debug.Log("Book placed");
                canPlace = false;
            }
            else
            {
                Debug.Log("Book not placed - no valid book in inventory");
            }
        }
        else
        {
            PickUpBook();
        }
    }

    private bool PlaceBook()
    {
        if (inventory == null || inventory.inventory.Count == 0)
            return false;

        ItemData bookToPlace = null;
        foreach (var item in inventory.inventory)
        {
            if (item != null && item.typeInput == InputType.Book)
            {
                bookToPlace = item;
                break;
            }
        }

        if (bookToPlace != null)
        {
            placedBook = bookToPlace;

            inventory.RemoveItem(bookToPlace);

            MoveBookToShelf(bookToPlace.prefab);

            return true;
        }

        return false;
    }

    private void PickUpBook()
    {
        if (placedBook != null && inventory != null && inventory.CheckInventory())
        {
            inventory.AddItem(placedBook);

            if (placedBookObject != null)
            {
                Destroy(placedBookObject);
                placedBookObject = null;
            }

            placedBook = null;
            itemPickedUp = true;
            canPlace = true;
        }
        else
        {
            Debug.LogWarning("Cannot pick up book: inventory full or missing Book.");
        }
    }

    private void MoveBookToShelf(GameObject bookPrefab)
    {
        if (bookPrefab != null)
        {
            if (placedBookObject != null)
            {
                Destroy(placedBookObject);
            }

            placedBookObject = Instantiate(
                bookPrefab,
                pos.position,
                Quaternion.Euler(0, 90, 0)
            );

            placedBookObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Tablet prefab is null, cannot move to shrine!");
        }
    }
}

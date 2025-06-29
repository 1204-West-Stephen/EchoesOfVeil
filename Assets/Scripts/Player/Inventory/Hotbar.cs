using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
public class Hotbar : MonoBehaviour
{
    public List<Cell> cells;
    public Inventory inventory;

    public int selectedIndex = 0;

    PlayerInput controls;

    [Header("Hotbar UI Fade")]
    public float fadeSpeed = 2f;

    bool onePressed;
    bool twoPressed;
    bool threePressed;
    bool fourPressed;
    bool fivePressed;
    bool sixPressed;

    Vector2 scrollDelta;

    bool changeDetected;
    bool fadeOutAllowed;

    private float lastInputTime;

    private Coroutine fadeOutDelayCoroutine;

    public Image itemInHand; 
    public PlayerControls playerControls;

    private void Awake()
    {
        controls = new PlayerInput();

        controls.Menus.Press1.performed += _ => onePressed = true;
        controls.Menus.Press1.canceled += _ => onePressed = false;

        controls.Menus.Press2.performed += _ => twoPressed = true;
        controls.Menus.Press2.canceled += _ => twoPressed = false;

        controls.Menus.Press3.performed += _ => threePressed = true;
        controls.Menus.Press3.canceled += _ => threePressed = false;

        controls.Menus.Press4.performed += _ => fourPressed = true;
        controls.Menus.Press4.canceled += _ => fourPressed = false;

        controls.Menus.Press5.performed += _ => fivePressed = true;
        controls.Menus.Press5.canceled += _ => fivePressed = false;

        controls.Menus.Press6.performed += _ => sixPressed = true;
        controls.Menus.Press6.canceled += _ => sixPressed = false;

        controls.Menus.Scroll.performed += ctx =>
        {
            scrollDelta = ctx.ReadValue<Vector2>();
        };

    }

    private void Update()
    {
        HandleInput();

        int itemCount = inventory.inventory.Count;

        bool validKeyPress =
            (onePressed) ||
            (twoPressed && itemCount > 1) ||
            (threePressed && itemCount > 2) ||
            (fourPressed && itemCount > 3) ||
            (fivePressed && itemCount > 4) ||
            (sixPressed && itemCount > 5);

        bool validScroll = scrollDelta != Vector2.zero && itemCount > 0;

        changeDetected = inventory.itemAdded || validKeyPress || validScroll;

        if (changeDetected)
        {
            lastInputTime = Time.time;
            playerControls.ItemInHand.gameObject.SetActive(true);

            if (inventory.itemAdded)
            {
                inventory.itemAdded = false;
            }
        }

        DisplayCells();
        FadeHotbar();

        scrollDelta = Vector2.zero;
    }



    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void HandleInput()
    {
        int itemCount = inventory.inventory.Count;

        // Prevent all input logic if inventory is empty
        if (itemCount == 0)
        {
            selectedIndex = 0; // Or -1 if you prefer to mark it as invalid
            return;
        }

        // Scroll input
        if (scrollDelta.y < 0f)
        {
            do
            {
                selectedIndex = (selectedIndex + 1) % itemCount;
            } while (inventory.inventory[selectedIndex] == null);
        }
        else if (scrollDelta.y > 0f)
        {
            do
            {
                selectedIndex = (selectedIndex - 1 + itemCount) % itemCount;
            } while (inventory.inventory[selectedIndex] == null);
        }

        // Number key input
        if (onePressed)
        {
            selectedIndex = 0;
        }
        else if (twoPressed && itemCount > 1)
        {
            selectedIndex = 1;
        }
        else if (threePressed && itemCount > 2)
        {
            selectedIndex = 2;
        }
        else if (fourPressed && itemCount > 3)
        {
            selectedIndex = 3;
        }
        else if (fivePressed && itemCount > 4)
        {
            selectedIndex = 4;
        }
        else if (sixPressed && itemCount > 5)
        {
            selectedIndex = 5;
        }
    }



    private void DisplayCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            ItemData item = (i < inventory.inventory.Count) ? inventory.inventory[i] as ItemData : null;
            cells[i].SetItem(item);

            cells[i].SetSelected(i == selectedIndex); // Highlight selected cell
        }

        if (inventory.inventory.Count > 0 && selectedIndex < inventory.inventory.Count)
        {
            SetItemInHand(inventory.inventory[selectedIndex] as ItemData);
        }
        else
        {
            SetItemInHand(null); // Hide item in hand if nothing is there
        }
    }

    public void SetItemInHand(ItemData item)
    {
        if (item == null || item.sprite == null)
        {
            itemInHand.gameObject.SetActive(false);
        }
        else
        {
            itemInHand.sprite = item.sprite;
            itemInHand.gameObject.SetActive(true);
        }
    }


    private void FadeHotbar()
    {
        float currentAlpha = cells[0].GetComponentInChildren<TextMeshProUGUI>()?.color.a ?? 0f;
        float targetAlpha = 0f;

        // If input was recently detected, fade in
        if (Time.time - lastInputTime < 3f)
        {
            targetAlpha = 1f;
        }
        else
        {
            targetAlpha = 0f;
        }

        foreach (var cell in cells)
        {
            TextMeshProUGUI text = cell.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                Color color = text.color;
                color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime * fadeSpeed);
                text.color = color;
            }
        }
    }
}

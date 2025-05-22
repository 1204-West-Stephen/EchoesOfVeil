using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

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
    private bool isFadingIn = false;
    private bool isFadingOut = false;

    private float lastInputTime;

    private Coroutine fadeOutDelayCoroutine;

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

        changeDetected = inventory.itemAdded || onePressed || twoPressed || threePressed || fourPressed || fivePressed || sixPressed || scrollDelta != Vector2.zero;

        if (changeDetected)
        {
            lastInputTime = Time.time;

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
        if (scrollDelta.y < 0f)
        {
            selectedIndex = (selectedIndex + 1) % cells.Count;
        }
        else if (scrollDelta.y > 0f)
        {
            selectedIndex = (selectedIndex - 1 + cells.Count) % cells.Count;
        }

        if (onePressed)
        {
            selectedIndex = 0;
        }
        else if (twoPressed)
        {
            selectedIndex = 1;
        }
        else if (threePressed)
        {
            selectedIndex = 2;
        }
        else if (fourPressed)
        {
            selectedIndex = 3;
        }
        else if (fivePressed)
        {
            selectedIndex = 4;
        }
        else if (sixPressed)
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

        // You can also handle showing the item in hand here, like:
        // SetItemInHand(inventory.inventory[selectedIndex] as ItemData);
    }

    // Optional: Add this to control what's in hand (e.g. a 3D model)
    // public void SetItemInHand(ItemData item) { ... }

    private void FadeHotbar()
    {
        float currentAlpha = cells[0].GetComponentInChildren<TextMeshProUGUI>()?.color.a ?? 0f;
        float targetAlpha = 0f;

        // If input was recently detected, fade in
        if (Time.time - lastInputTime < 3f)
        {
            targetAlpha = 1f;
            isFadingIn = true;
            isFadingOut = false;
        }
        else
        {
            targetAlpha = 0f;
            isFadingIn = false;
            isFadingOut = true;
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

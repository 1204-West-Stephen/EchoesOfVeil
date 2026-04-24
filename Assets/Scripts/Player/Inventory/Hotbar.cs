using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
public class Hotbar : MonoBehaviour
{
    public List<Cell> cells;
    public Inventory inventory;
    private ItemData lastItem = null;

    public int selectedIndex = 0;
    private int lastIndex = -1;

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

        bool hasAnyItem = false;
        foreach (var item in inventory.inventory)
        {
            if (item != null)
            {
                hasAnyItem = true;
                break;
            }
        }

        if (!hasAnyItem)
        {
            inventory.selectedIndex = 0;
            selectedIndex = 0;
            return;
        }

        if (scrollDelta.y != 0f)
        {
            bool hasItem = false;
            foreach (var item in inventory.inventory)
            {
                if (item != null)
                {
                    hasItem = true;
                    break;
                }
            }

            if (!hasItem) return; 

            int startIndex = selectedIndex;

            do
            {
                if (scrollDelta.y < 0f)
                {
                    selectedIndex = (selectedIndex + 1) % inventory.inventory.Count;
                }
                else
                {
                    selectedIndex = (selectedIndex - 1 + inventory.inventory.Count) % inventory.inventory.Count;
                }

                if (selectedIndex == startIndex) break;

            } while (inventory.inventory[selectedIndex] == null);
        }

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

        inventory.selectedIndex = selectedIndex;
    }
    private void DisplayCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            ItemData item = (i < inventory.inventory.Count) ? inventory.inventory[i] as ItemData : null;
            cells[i].SetItem(item);

            cells[i].SetSelected(i == selectedIndex); 
        }

        ItemData currentItem = inventory.inventory[selectedIndex];

        if (selectedIndex != lastIndex || currentItem != lastItem)
        {
            lastIndex = selectedIndex;
            lastItem = currentItem;

            if (currentItem != null)
                playerControls.SpawnSelectedHotbarItem(currentItem);
            else
                playerControls.ClearHeldItem();
        }
    }
    private void FadeHotbar()
    {
        float currentAlpha = cells[0].GetComponentInChildren<TextMeshProUGUI>()?.color.a ?? 0f;
        float targetAlpha = 0f;

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
    public ItemData GetCurrentItemInHand()
    {
        if (inventory == null || inventory.inventory.Count == 0)
            return null;

        // Clamp selectedIndex to valid range
        selectedIndex = Mathf.Clamp(selectedIndex, 0, inventory.inventory.Count - 1);

        ItemData current = inventory.inventory[selectedIndex] as ItemData;
        Debug.Log($"Current item: {current}");
        return current;
    }

}

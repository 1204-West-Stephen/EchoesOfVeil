using UnityEngine;
using System.Collections.Generic;

public class Hotbar : MonoBehaviour
{
    public List<Cell> cells;
    public Inventory inventory;

    public int selectedIndex = 0;

    PlayerInput controls;

    private void Start()
    {
        
    }

    private void Update()
    {
        HandleInput();
        DisplayCells();
    }

    private void HandleInput()
    {
        // Scroll Wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            selectedIndex = (selectedIndex + 1) % cells.Count;
        }
        else if (scroll < 0f)
        {
            selectedIndex = (selectedIndex - 1 + cells.Count) % cells.Count;
        }

        // Number keys 1–6 (KeyCode.Alpha1 is "1", etc.)
        for (int i = 0; i < cells.Count; i++)
        {
            
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
}

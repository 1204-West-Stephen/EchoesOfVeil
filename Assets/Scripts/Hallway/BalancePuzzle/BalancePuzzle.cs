using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalancePuzzle : MonoBehaviour, i_Interactable
{
    private int weightBalance;
    private NumberButton selectedNumber;
    public TextMeshProUGUI weightNum;

    public Camera gameCamera;
    private PlayerCamera playerCamera;
    public Canvas gameCanvas;

    [SerializeField] private List<NumberButton> numberButtons;

    private PlayerControls controls;

    private void Start()
    {
        controls = FindObjectOfType<PlayerControls>();

        if (gameCamera != null) gameCamera.gameObject.SetActive(false);
        if (gameCanvas != null) gameCanvas.gameObject.SetActive(false);

        // Auto-populate numberButtons if empty
        if (numberButtons == null || numberButtons.Count == 0)
        {
            numberButtons = new List<NumberButton>(gameCanvas.GetComponentsInChildren<NumberButton>());
        }

        weightBalance = 5;
        UpdateWeightUI();
    }

    public void Interact()
    {
        if (gameCanvas == null) return;

        gameCanvas.gameObject.SetActive(true);
        controls?.DisableControls();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Puzzle interaction started.");
    }

    public void SelectNumber(NumberButton numberBtn)
    {
        if (numberBtn == null) return;

        // Deselect if clicking the same button
        if (selectedNumber == numberBtn)
        {
            selectedNumber.Highlight(false);
            selectedNumber = null;
            return;
        }

        // Deselect previously selected
        if (selectedNumber != null)
            selectedNumber.Highlight(false);

        selectedNumber = numberBtn;
        selectedNumber.Highlight(true);
    }

    public void IncreaseBalance()
    {
        if (selectedNumber == null) return;

        if (weightBalance + selectedNumber.value <= 5)
        {
            weightBalance += selectedNumber.value;
            ConsumeSelectedNumber();
        }
        else
        {
            ResetPuzzle();
        }

        UpdateWeightUI();
    }

    public void DecreaseBalance()
    {
        if (selectedNumber == null) return;

        if (weightBalance - selectedNumber.value >= -5)
        {
            weightBalance -= selectedNumber.value;
            ConsumeSelectedNumber();
        }
        else
        {
            ResetPuzzle();
        }

        UpdateWeightUI();
    }

    private void ConsumeSelectedNumber()
    {
        if (selectedNumber == null) return;

        selectedNumber.DisableButton();
        selectedNumber = null;

        CheckPuzzleCompletion();
    }

    private void CheckPuzzleCompletion()
    {
        bool allUsed = true;

        foreach (var btn in numberButtons)
        {
            if (btn != null && btn.IsInteractable())
            {
                allUsed = false;
                break;
            }
        }

        if (allUsed)
        {
            if (weightBalance != 0)
            {
                Debug.Log("Puzzle reset because all numbers were used and balance is not 0");
                ResetPuzzle();
            }
            else
            {
                Debug.Log("Puzzle completed successfully!");
                // Optional: trigger success event here
            }
        }
    }

    public void ResetPuzzle()
    {
        Debug.Log("Puzzle reset!");

        // 1. Clear selected number reference first
        if (selectedNumber != null)
        {
            selectedNumber.Highlight(false);
            selectedNumber = null;
        }

        // 2. Reset all buttons in the puzzle
        if (numberButtons != null)
        {
            foreach (var btn in numberButtons)
            {
                if (btn != null)
                {
                    // Ensure interactable is true first
                    btn.ResetButton();
                }
            }
        }

        // 3. Reset balance
        weightBalance = 5;
        UpdateWeightUI();
    }


    private void UpdateWeightUI()
    {
        if (weightNum != null)
            weightNum.text = " " + weightBalance;
    }
}

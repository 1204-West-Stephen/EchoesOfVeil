using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalancePuzzle : MonoBehaviour, i_Interactable
{
    private int weightBalance;
    public TextMeshProUGUI weightNum;

    public NumberStone numberStone;

    private void Start()
    {
        weightBalance = 5;
        UpdateWeightUI();
    }

    public void Interact()
    {
        
    }

    public void IncreaseBalance()
    {
        if (weightBalance <= 5)
        {
            
        }
        else
        {
            ResetPuzzle();
        }

        UpdateWeightUI();
    }

    public void DecreaseBalance()
    {
        if (weightBalance >= -5)
        {

        }
        else
        {
            ResetPuzzle();
        }

        UpdateWeightUI();
    }

    private void ConsumeSelectedNumber()
    {
        CheckPuzzleCompletion();
    }

    private void CheckPuzzleCompletion()
    {
        
    }

    public void ResetPuzzle()
    {
        
    }


    private void UpdateWeightUI()
    {
        if (weightNum != null)
            weightNum.text = " " + weightBalance;
    }
}

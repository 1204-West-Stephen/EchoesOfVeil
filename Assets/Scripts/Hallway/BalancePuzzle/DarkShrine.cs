using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkShrine : MonoBehaviour, i_Interactable
{
    public BalancePuzzle puzzle;

    public void Interact()
    {
        puzzle.IncreaseBalance();
    }
}

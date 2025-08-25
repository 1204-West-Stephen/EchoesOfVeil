using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShrine : MonoBehaviour, i_Interactable
{
    public BalancePuzzle puzzle;

    public void Interact()
    {
        puzzle.DecreaseBalance();
    }
}

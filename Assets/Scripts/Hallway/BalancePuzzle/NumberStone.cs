using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberStone : MonoBehaviour, i_Interactable
{
    public BalancePuzzle puzzle;
    public ItemData item;
    private int value;

    private void Start()
    {
        value = item.stoneValue;
    }

    public void Interact()
    {

    }
}

using System.Collections.Generic;
using UnityEngine;

public class BalancePuzzle : MonoBehaviour
{
    private int weightBalance;
    public GameObject weightGem;
    public List<NumberStone> numberStones;  // references to stones in the scene
    public List<Transform> gemLocations;    // gem marker positions (-5..+5)

    private void Start()
    {
        weightBalance = 0;
        UpdateGemPosition();
    }

    public void ApplyStoneIncrease(ItemData item)
    {
        weightBalance += item.stoneValue;

        if (weightBalance > 5)
        {
            ResetPuzzle();
        }
        else
        {
            UpdateGemPosition();
        }
    }
    public void ApplyStoneDecrease(ItemData item)
    {
        weightBalance += item.stoneValue;

        if (weightBalance < -5)
        {
            ResetPuzzle();
        }
        else
        {
            UpdateGemPosition();
        }
    }

    private void UpdateGemPosition()
    {
        int index = weightBalance + 5; 
        if (index >= 0 && index < gemLocations.Count)
        {
            weightGem.transform.position = gemLocations[index].position;
        }
    }

    public void ResetPuzzle()
    {
        Debug.Log("Puzzle failed - resetting!");

        weightBalance = 0;
        UpdateGemPosition();

        // Respawn stones
        for (int i = 0; i < numberStones.Count; i++)
        {
            numberStones[i].Respawn(numberStones[i].transform.position);
        }
    }
}

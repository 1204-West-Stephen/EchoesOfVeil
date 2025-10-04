using System.Collections.Generic;
using UnityEngine;

public class BalancePuzzle : MonoBehaviour
{
    private int weightBalance;
    public GameObject weightGem;
    public List<NumberStone> numberStones;
    public List<Transform> gemLocations;

    public bool puzzleSolved = false;
    private bool isResetting = false;

    private void Start()
    {
        weightBalance = 5;
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
        weightBalance -= item.stoneValue;

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
        foreach (var location in gemLocations)
        {
            ScaleValue sv = location.GetComponent<ScaleValue>();
            if (sv == null)
            {
                Debug.LogWarning(location.name + " is missing a ScaleValue component!");
                continue;
            }

            int scale = sv.returnScaleValue();

            if (scale == weightBalance)
            {
                weightGem.transform.position = location.position;

                if (!isResetting) // only check puzzle state if not resetting
                {
                    if (weightBalance == 0 && AllStonesUsed())
                    {
                        Debug.Log("Puzzle is solved!");
                        OnPuzzleSolved();
                    }
                    else if (weightBalance != 0 && AllStonesUsed())
                    {
                        ResetPuzzle();
                    }
                }

                return;
            }
        }

        Debug.LogWarning("No gem location matches weightBalance: " + weightBalance);
    }

    private bool AllStonesUsed()
    {
        foreach (var stone in numberStones)
        {
            if (stone.gameObject.activeSelf) // still active in scene
                return false;
        }
        return true;
    }

    private void OnPuzzleSolved()
    {
        puzzleSolved = true;
        FindObjectOfType<HallwayGameManager>()?.CheckHallwayPuzzles();
    }


    public void ResetPuzzle()
    {
        Debug.Log("Puzzle failed - resetting!");

        isResetting = true;

        weightBalance = 5;
        UpdateGemPosition();

        for (int i = 0; i < numberStones.Count; i++)
        {
            numberStones[i].Respawn(numberStones[i].transform.position);
        }

        isResetting = false;
    }
}

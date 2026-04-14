using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class BalancePuzzle : MonoBehaviour
{
    private int weightBalance;
    public GameObject weightGem;
    public List<NumberStone> numberStones;
    public List<Transform> gemLocations;
    private Inventory inventory;
    public Camera solvedCamera;
    private HallwayGameManager hgm;
    private PlayerCamera playerCamera;

    public GameObject Enemy;
    public Animator enemyAnimator;
    public AudioClip clip;

    public bool puzzleSolved = false;
    private bool isResetting = false;
    public bool firstTime = true;

    private void Start()
    {
        hgm = FindFirstObjectByType<HallwayGameManager>();
        weightBalance = 5;
        UpdateGemPosition();
        inventory = FindFirstObjectByType<Inventory>();

        Enemy.SetActive(false);
        playerCamera = FindFirstObjectByType<PlayerCamera>();
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
        FindFirstObjectByType<HallwayGameManager>()?.CheckHallwayPuzzles();
        StartCoroutine(hgm.ShowLight(solvedCamera));
    }


    public void ResetPuzzle()
    {
        Debug.Log("Puzzle failed - resetting!");

        isResetting = true;

        weightBalance = 5;
        UpdateGemPosition();

        if (firstTime)
        {
            StartCoroutine(JumpscareLibrary());

        }

        for (int i = 0; i < inventory.inventory.Count; i++)
        {
            if (inventory.inventory[i] != null &&
                inventory.inventory[i].typeInput == InputType.NumberStone)
            {
                inventory.inventory[i] = null;
            }
        }

        for (int i = 0; i < numberStones.Count; i++)
        {
            numberStones[i].Respawn(numberStones[i].transform.position);
        }

        isResetting = false;
    }

    IEnumerator JumpscareLibrary()
    {
        yield return new WaitForSeconds(1.5f);

        Enemy.SetActive(true);
        enemyAnimator.Play("JumpScare", 0, 0f);
        AudioSource.PlayClipAtPoint(clip, playerCamera.transform.position, 2f);

        yield return new WaitForSeconds(0.3f);

        Enemy.SetActive(false);

        firstTime = false;
    }
}

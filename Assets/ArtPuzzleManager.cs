using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtPuzzleManager : MonoBehaviour
{
    public List<ArtPiece> pieces;        // 48 pieces assigned in Inspector
    public Transform[] spawnPositions;   // 48 positions (3 grids of 16)
    public ArtPiece activePiece;         // currently selected piece

    private List<int> correctOrder = new List<int>();
    private List<int> currentOrder = new List<int>();

    private void Start()
    {
        // Track correct order
        for (int i = 0; i < pieces.Count; i++)
            correctOrder.Add(i);

        // Separate movable and anchor pieces
        List<int> movableIndices = new List<int>();
        for (int i = 0; i < pieces.Count; i++)
            if (pieces[i].isMovable)
                movableIndices.Add(i);

        Shuffle(movableIndices);

        currentOrder = new List<int>(correctOrder);

        int movableIndex = 0;
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].isMovable)
            {
                int shuffledIndex = movableIndices[movableIndex];
                pieces[shuffledIndex].transform.position = spawnPositions[i].position;
                currentOrder[i] = shuffledIndex;
                movableIndex++;
            }
            else
            {
                pieces[i].transform.position = spawnPositions[i].position;
                currentOrder[i] = i;
            }
        }
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    // Swap two pieces smoothly
    public IEnumerator SwapPiecesCoroutine(ArtPiece first, ArtPiece second)
    {
        Vector3 firstPos = first.transform.position;
        Vector3 secondPos = second.transform.position;

        float elapsed = 0f;
        float duration = first.moveDuration;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            first.transform.position = Vector3.Lerp(firstPos, secondPos, t);
            second.transform.position = Vector3.Lerp(secondPos, firstPos, t);
            yield return null;
        }

        first.transform.position = secondPos;
        second.transform.position = firstPos;

        // Update internal order tracking
        int firstIndex = pieces.IndexOf(first);
        int secondIndex = pieces.IndexOf(second);
        int temp = currentOrder[firstIndex];
        currentOrder[firstIndex] = currentOrder[secondIndex];
        currentOrder[secondIndex] = temp;
    }

    // Check if puzzle is solved
    public bool CheckSolved()
    {
        for (int i = 0; i < correctOrder.Count; i++)
            if (correctOrder[i] != currentOrder[i])
                return false;
        return true;
    }
}

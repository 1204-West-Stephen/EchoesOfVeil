using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArtPiece : MonoBehaviour, i_Interactable
{
    public GameObject parent;
    public GameObject piece;
    public bool isMovable = true; // false for anchor pieces

    public ArtPuzzleManager puzzleManager;

    public float moveDistance = 0.1f;
    public float moveDuration = 0.1f;

    private bool interacted = false;

    public Image pieceImage; // the UI sprite reference (optional)

    public void Interact()
    {
        if (!isMovable)
            return; // ignore clicks on anchors

        if (puzzleManager.activePiece == null)
        {
            // Select this piece
            puzzleManager.activePiece = this;
            StartCoroutine(MovePiece(transform, moveDistance, moveDuration));
            interacted = true;
        }
        else if (puzzleManager.activePiece == this)
        {
            // Deselect
            StartCoroutine(MovePiece(transform, -moveDistance, moveDuration));
            puzzleManager.activePiece = null;
            interacted = false;
        }
        else
        {
            // Swap with active piece
            StartCoroutine(puzzleManager.SwapPiecesCoroutine(puzzleManager.activePiece, this));
            puzzleManager.activePiece = null;
        }
    }

    public IEnumerator MovePiece(Transform target, float distance, float duration)
    {
        Vector3 start = target.position;
        Vector3 end = start + new Vector3(distance, 0, 0);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            target.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        target.position = end;
    }
}

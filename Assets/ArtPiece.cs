using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArtPiece : MonoBehaviour, i_Interactable
{
    public GameObject parent;
    public GameObject piece;
    private GameObject tempPiece;
    private GameObject tempParent;

    public ArtPuzzleManager puzzleManager;

    public bool interacted = false;
    public Image sprite1; //first selected sprite
    public Image correctImage;
    private Image sprite2; //second selected sprite 
    private Image tempSprite; //temporarily save a sprite so that sprites can be swapped

    public float moveDistance = 0.1f; // how far the piece moves when selected
    public float moveDuration = 0.1f; // time in seconds for smooth move

    public void Interact()
    {
        if (!interacted)
        {
            // Smoothly move piece forward
            StartCoroutine(MovePiece(transform, moveDistance, moveDuration));

            tempPiece = piece;
            tempParent = parent;
            puzzleManager.activeSprite = sprite1;
            interacted = true;
        }
        else
        {
            if (tempPiece == piece && tempParent == parent)
            {
                // Smoothly move piece back
                StartCoroutine(MovePiece(transform, -moveDistance, moveDuration));

                tempPiece = null;
                tempParent = null;
                interacted = false;
            }
            else
            {
                // Swap sprites
                tempSprite = puzzleManager.activeSprite;
                sprite2 = sprite1;
                sprite1 = tempSprite;
                tempSprite = null;
            }
        }
    }

    private IEnumerator MovePiece(Transform target, float distance, float duration)
    {
        Vector3 start = target.position;
        Vector3 end = start + new Vector3(distance, 0, 0); // move along x-axis
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

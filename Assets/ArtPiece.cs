using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
    public void Interact()
    {
        if (!interacted)
        {
            Vector3 pos = transform.position;
            
            for (float i = 0.001f; i < 0.1; i += 0.001f)
            {
                pos.x += i;
                transform.position = pos;
            }

            tempPiece = piece;
            tempParent = parent;
            puzzleManager.activeSprite = sprite1;
            interacted = true;
        }

        else
        {
            if (tempPiece == piece && tempParent == parent)
            {
                Vector3 pos = transform.position;

                for (float i = 0.001f; i < 0.1; i += 0.001f)
                {
                    pos.x -= i;
                    transform.position = pos;
                }

                tempPiece = null;
                tempParent = null;
                interacted = true;
            }

            else
            {
                tempSprite = puzzleManager.activeSprite;
                sprite2 = sprite1;
                sprite1 = tempSprite;
                tempSprite = null;
            }
        }
    }
}

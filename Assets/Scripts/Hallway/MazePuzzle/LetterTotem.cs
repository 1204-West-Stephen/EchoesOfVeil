using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterTotem : MonoBehaviour, i_Interactable
{
    public List<Sprite> letterSprites;
    public Sprite answerSprite;
    public SpriteRenderer currentSprite;
    private int index;
    

    public bool puzzleSovled;
    public bool canInteract = true;


    void Start()
    {
        index = 0;
        currentSprite.sprite = letterSprites[index];
        puzzleSovled = false;
    }

    public void Interact()
    {
        if (canInteract)
        {
            index++;

            if (index > letterSprites.Count)
            {
                index = 0;
            }

            currentSprite.sprite = letterSprites[index];

            FindFirstObjectByType<TotemManager>()?.CheckIfSolved();
        }
    }

}

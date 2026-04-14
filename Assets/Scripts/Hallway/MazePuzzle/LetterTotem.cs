using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LetterTotem : MonoBehaviour, i_Interactable
{
    public List<Sprite> letterSprites;
    public Sprite answerSprite;
    public SpriteRenderer currentSprite;
    private int index;
    private AudioSource source;
    public AudioClip clip;

    public bool puzzleSovled;
    public bool canInteract = true;
    public bool first = true;


    void Start()
    {
        index = 0;
        currentSprite.sprite = letterSprites[index];
        puzzleSovled = false;
        source = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (canInteract)
        {
            index++;

            if (source != null && clip != null && first)
            {
                first = false;
                source.PlayOneShot(clip);
            }

            if (index > letterSprites.Count)
            {
                index = 0;
            }

            currentSprite.sprite = letterSprites[index];

            FindFirstObjectByType<TotemManager>()?.CheckIfSolved();
        }
    }

}

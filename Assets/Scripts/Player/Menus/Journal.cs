using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Journal : MonoBehaviour, i_Interactable
{
    private AudioSource source;
    public AudioClip clip;
    public GameObject journalUI;

    public TutorialManager tutorialManager;
    private CanvasGroup cg;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        PlayerControls player = FindFirstObjectByType<PlayerControls>();
        player.AcquireJournal();
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, 0.15f);
        }
        journalUI.SetActive(false);
        tutorialManager.StartCoroutine(tutorialManager.ShowKeyUI(tutorialManager.jControlUI, tutorialManager.jCG));
        Destroy(gameObject);
    }

    public InputType GetRequiredInputType()
    {
        return InputType.None;
    }
}

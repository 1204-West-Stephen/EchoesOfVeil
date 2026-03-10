using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.Audio;

public class Journal : MonoBehaviour, i_Interactable
{
    private AudioSource source;
    public AudioClip clip;

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
        Destroy(gameObject);
    }

    public InputType GetRequiredInputType()
    {
        return InputType.None;
    }
}

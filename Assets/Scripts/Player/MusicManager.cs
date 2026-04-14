using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clip;

    [Header("Timing")]
    public float minCooldown = 360f; // 6 minutes
    public float checkInterval = 10f;
    [Range(0f, 1f)]
    public float playChance = 0.2f;

    private float lastPlayTime = -999f;
    private bool isPlaying;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(MusicRoutine());
    }

    IEnumerator MusicRoutine()
    {
        while (true)
        {
            if (!isPlaying)
            {
                bool cooldownPassed = Time.time >= lastPlayTime + minCooldown;

                if (cooldownPassed)
                {
                    float value = Random.value;

                    if (value <= playChance)
                    {
                        PlayClip();
                        yield return StartCoroutine(WaitForClipToFinish());
                        lastPlayTime = Time.time;
                    }
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    void PlayClip()
    {
        if (clip == null) return;

        audioSource.clip = clip;
        audioSource.Play();
        isPlaying = true;
    }

    IEnumerator WaitForClipToFinish()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        isPlaying = false;
    }
}
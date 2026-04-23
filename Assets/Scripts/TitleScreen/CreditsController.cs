using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CreditsController : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform creditsPanel;
    public CanvasGroup canvasGroup;
    public ScrollRect scrollRect;

    [Header("Scroll Settings")]
    public float scrollSpeed = 20f;
    public float startDelay = 2f;
    public bool autoScroll = true;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;
    public bool fadeInOnStart = true;
    public bool fadeOutOnEnd = true;

    [Header("End Behavior")]
    public float endDelay = 2f;
    public bool loadSceneAfter = false;
    public string sceneToLoad;

    public PlayerInput playerInput;

    private bool isScrolling = false;
    private bool isEnding = false;

    public AudioClip music;
    private AudioSource musicSource;

    private void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.Menus.Enable();

        playerInput.Menus.Space.performed += ctx => SkipCredits();
    }

    void Start()
    {
            musicSource = GetComponent<AudioSource>();
            musicSource.PlayOneShot(music);
            musicSource.loop = true;

        if (fadeInOnStart)
            StartCoroutine(Fade(0, 1));

        if (autoScroll)
            StartCoroutine(BeginScroll());
    }

    void Update()
    {
        if (isScrolling)
        {
            creditsPanel.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            if (creditsPanel.anchoredPosition.y >= creditsPanel.sizeDelta.y)
            {
                isScrolling = false;
                StartCoroutine(EndCredits());
            }
        }
    }

    IEnumerator BeginScroll()
    {
        yield return new WaitForSeconds(startDelay);
        isScrolling = true;
    }

    IEnumerator EndCredits()
    {
        isEnding = true;

        yield return new WaitForSeconds(endDelay);

        if (fadeOutOnEnd)
            yield return Fade(canvasGroup.alpha, 0);

        if (loadSceneAfter && !string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    IEnumerator Fade(float from, float to)
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    public void SkipCredits()
    {
        if (isEnding) return;

        isScrolling = false;
        StartCoroutine(SkipRoutine());
    }

    IEnumerator SkipRoutine()
    {
        isEnding = true;

        if (fadeOutOnEnd)
            yield return Fade(canvasGroup.alpha, 0);

        if (loadSceneAfter && !string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
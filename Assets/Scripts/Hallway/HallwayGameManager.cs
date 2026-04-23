using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class HallwayGameManager : MonoBehaviour
{
    public BalancePuzzle bpm;
    public GuardianPuzzleManager gpm;
    public TotemManager tm;
    public TabletShrineManager tbm;

    public Camera solvedCamera;

    public bool gameMaster = false;
    private bool isLoaded = false;
    public string LibrarySceneName = "Library";

    public List<Light> lights;
    public float fadeDuration = 5f;
    public bool[] puzzleCompleted;
    public float lightIntensity = 6f;

    private Animator animator;
    public AudioClip clip;
    public AudioClip lightSound;

    private int activeFades = 0;

    private void Start()
    {
        puzzleCompleted = new bool[4];

        for (int i = 0; i < puzzleCompleted.Length; i++)
        {
            puzzleCompleted[i] = false;
        }

        foreach (Light light in lights)
        {
            light.intensity = 0;
        }

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckHallwayPuzzles();
    }

    public void CheckHallwayPuzzles()
    {
        if (bpm.puzzleSolved && !puzzleCompleted[0])
        {
            puzzleCompleted[0] = true;
            activeFades++;
            StartCoroutine(FadeInLight(lights[0]));
        }

        if (gpm.puzzleSolved && !puzzleCompleted[1])
        {
            puzzleCompleted[1] = true;
            activeFades++;
            StartCoroutine(FadeInLight(lights[1]));
        }

        if (tm.puzzleSolved && !puzzleCompleted[2])
        {
            puzzleCompleted[2] = true;
            activeFades++;
            StartCoroutine(FadeInLight(lights[2]));
        }

        if (tbm.puzzleSolved && !puzzleCompleted[3])
        {
            puzzleCompleted[3] = true;
            activeFades++;
            StartCoroutine(FadeInLight(lights[3]));
        }

        // CHANGED: wait for fades before opening
        if ((puzzleCompleted.All(p => p) || gameMaster) && !isLoaded)
        {
            isLoaded = true;
            StartCoroutine(WaitForFadesThenOpen());
            gameMaster = false;
        }
    }

    private IEnumerator WaitForFadesThenOpen()
    {
        // Wait until all fades complete
        while (activeFades > 0)
            yield return null;

        // Now safe to proceed
        StartCoroutine(LoadLibrary());
        StartCoroutine(OpenDoors());
    }

    private IEnumerator FadeInLight(Light light)
    {
        yield return new WaitForSeconds(3f);

        // Prevent duplicate triggers
        if (light.intensity > 0f)
        {
            activeFades--;
            yield break;
        }

        AudioSource.PlayClipAtPoint(lightSound, transform.position, 0.9f);

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / fadeDuration);
            light.intensity = Mathf.Lerp(0f, lightIntensity, lerp);
            yield return null;
        }

        light.intensity = lightIntensity;

        activeFades--; // IMPORTANT
    }

    private IEnumerator OpenDoors()
    {
        yield return new WaitForSeconds(2.0f);

        StartCoroutine(ShowLight(solvedCamera));

        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("Open");
        AudioSource.PlayClipAtPoint(clip, transform.position, 0.55f);
    }

    private IEnumerator LoadLibrary()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LibrarySceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public IEnumerator ShowLight(Camera solvedCamera)
    {
        PlayerMovement movement = null;
        Camera playerCamera = null;

        // Wait for player to spawn
        while (movement == null)
        {
            movement = FindFirstObjectByType<PlayerMovement>();
            yield return null;
        }

        // Wait for camera to exist
        while (playerCamera == null)
        {
            playerCamera = movement.GetComponentInChildren<Camera>();
            yield return null;
        }

        if (solvedCamera == null)
        {
            Debug.LogError("Solved camera not assigned!");
            yield break;
        }

        movement.controlLock();

        yield return new WaitForSeconds(1.5f);

        solvedCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);

        yield return new WaitForSeconds(3.5f);

        playerCamera.gameObject.SetActive(true);
        solvedCamera.gameObject.SetActive(false);

        movement.controlUnlock();
    }
}
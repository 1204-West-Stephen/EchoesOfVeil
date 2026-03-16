using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Unity.AppUI.UI;

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
    private AudioSource source;
    public AudioClip clip;
    public AudioClip lightSound;

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
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckHallwayPuzzles();
    }

    public void CheckHallwayPuzzles()
    {
        if (bpm.puzzleSolved && !puzzleCompleted[0])
        {
            StartCoroutine(FadeInLight(lights[0]));
            puzzleCompleted[0] = true;
        }

        if (gpm.puzzleSolved)
        {
            StartCoroutine(FadeInLight(lights[1]));
            puzzleCompleted[1] = true;
        }

        if (tm.puzzleSolved)
        {
            StartCoroutine(FadeInLight(lights[2]));
            puzzleCompleted[2] = true;
        }

        if (tbm.puzzleSolved)
        {
            StartCoroutine(FadeInLight(lights[3]));
            puzzleCompleted[3] = true;
        }

        if (puzzleCompleted.All(p => p) || gameMaster)
        {
            if (!isLoaded)
            {
                isLoaded = true; // lock it immediately
                StartCoroutine(LoadLibrary());
                StartCoroutine(OpenDoors());
                gameMaster = false;
            }
        }
    }

    private IEnumerator FadeInLight(Light light)
    {
        yield return new WaitForSeconds(3f);

        AudioSource.PlayClipAtPoint(lightSound, transform.position, 0.55f);

        float t = 0f;
        while (t < fadeDuration && light.intensity <= lightIntensity)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / fadeDuration);
            light.intensity += lerp; 
            yield return null;
        }
    }

    private IEnumerator OpenDoors()
    {
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(ShowLight(solvedCamera));
        yield return new WaitForSeconds(1.5f);
        animator.SetTrigger("Open");
        AudioSource.PlayClipAtPoint(clip, transform.position, 0.55f);

        yield return new WaitForSeconds(1.5f);
    }

    private IEnumerator LoadLibrary()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LibrarySceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Wait until loading is complete
        while (!asyncLoad.isDone)
        {
            // Unity considers scene ready when progress hits 0.9
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        isLoaded = true;
    }

    public IEnumerator ShowLight(Camera solvedCamera)
    {
        PlayerMovement movement = FindFirstObjectByType<PlayerMovement>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        Camera playerCamera = playerObj.GetComponentInChildren<Camera>();
        playerCamera = playerObj.GetComponentInChildren<Camera>();

        movement.controlLock();
        yield return new WaitForSeconds(1.5f);
        solvedCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        yield return new WaitForSeconds(3.5f);

        playerCamera.gameObject.SetActive(true);
        solvedCamera.gameObject.SetActive(false);
        movement.controlUnlock();

        yield return null;
    }
}

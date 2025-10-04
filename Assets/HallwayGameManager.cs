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

    private bool isLoaded = false;
    public string LibrarySceneName = "Library";

    public List<Light> lights;
    public float fadeDuration = 5f;
    public bool[] puzzleCompleted;
    public float lightIntensity = 6f;

    private Animator animator;

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

        if (puzzleCompleted.All(p => p))
        {
            StartCoroutine(LoadLibrary());
            OpenDoors();
        }
    }

    private IEnumerator FadeInLight(Light light)
    {
        yield return new WaitForSeconds(3f);

        float t = 0f;
        while (t < fadeDuration && light.intensity <= lightIntensity)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / fadeDuration);
            light.intensity += lerp; 
            yield return null;
        }
    }

    private void OpenDoors()
    {
        animator.SetTrigger("Open");
    }

    private IEnumerator  LoadLibrary()
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
}

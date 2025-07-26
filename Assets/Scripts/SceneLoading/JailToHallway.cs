using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JailToHallway : MonoBehaviour
{
    public string HallwaySceneName = "Hallway";
    private bool isLoaded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isLoaded && other.CompareTag("Player"))
        {
            StartCoroutine(LoadHallway());
        }
    }

    IEnumerator LoadHallway()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(HallwaySceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9)
        {
            yield return null;
        }

        asyncLoad.allowSceneActivation = true;
        isLoaded = true;
    }

}

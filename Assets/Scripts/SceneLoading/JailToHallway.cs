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
        Debug.Log("Entered trigger: " + other.name);

        if (!isLoaded && other.CompareTag("Player"))
        {
            StartCoroutine(LoadHallway());
        }
    }

    IEnumerator LoadHallway()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(HallwaySceneName, LoadSceneMode.Additive);
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

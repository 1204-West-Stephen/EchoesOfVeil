using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragomHeadPiece : MonoBehaviour, i_Interactable
{
    public string SceneName;

    public void Interact()
    {
        StartCoroutine(LoadCredits());
    }

    private IEnumerator LoadCredits()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        SceneManager.UnloadSceneAsync("Library");
        SceneManager.UnloadSceneAsync("GameManager");
    }
}

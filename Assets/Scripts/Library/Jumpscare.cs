using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jumpscare : MonoBehaviour
{
    private Animator animator;
    public string librarySceneName = "Library";
    public Camera mainCamera;
    private GameObject player;
    private GameObject spawnPos;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator StartJumpscare()
    {
        SceneManager.UnloadSceneAsync(librarySceneName);

        animator.SetTrigger("Start");

        yield return new WaitForSeconds(2.3f);

        //Make Camera go black

        yield return new WaitForSeconds(2f);

        SceneManager.LoadSceneAsync(librarySceneName);

        player = GameObject.FindWithTag("Player");
        spawnPos = GameObject.FindWithTag("RespawnPoint");

        player.transform.position = spawnPos.transform.position;

        yield return null;
    }
}

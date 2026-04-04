using System.Collections;
using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HallwayTrigger : MonoBehaviour
{
    public List<GameObject> doors;
    private PlayerControls controls;

    public AudioClip scream;

    private void Start()
    {
        foreach (GameObject door in doors)
        {
            door.gameObject.SetActive(false);
        }

        controls = FindFirstObjectByType<PlayerControls>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.UnloadSceneAsync("Hallway");
            
            foreach (GameObject door in doors)
            {
                door.gameObject.SetActive(true);
                StartCoroutine(LibraryEnemyScream());
            }

            gameObject.SetActive(false);
        }
    }

    private IEnumerator LibraryEnemyScream()
    {
        controls.DisableControls();

        yield return new WaitForSeconds(0.3f);
        AudioSource.PlayClipAtPoint(scream, transform.position, 0.7f);

        controls.StartDialogue("What in the gods was that?!");
        controls.StartDialogue("I need to locate the Dragon's Pulse");
        controls.StartDialogue("before whatever... that was... finds me...");

        yield return new WaitForSeconds(0.3f);
        controls.EnableControls();
    }

}


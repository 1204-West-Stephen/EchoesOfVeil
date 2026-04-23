using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HallwayTrigger : MonoBehaviour
{
    public List<GameObject> doors;
    private PlayerControls controls;

    public AudioClip scream;
    public Transform screamLocation;

    public Material hallwayLeft;
    public Material hallwayRight;

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
            }

            StartCoroutine(LibraryEnemyScream());

            JournalStateManager.Instance.UpdateMaterial(hallwayLeft, hallwayRight);
        }
    }

    private IEnumerator LibraryEnemyScream()
    {
        controls.DisableControls();

        yield return new WaitForSeconds(0.3f);
        AudioSource.PlayClipAtPoint(scream, screamLocation.position, 0.8f);

        controls.StartDialogue("What in the gods was that?!");
        controls.StartDialogue("I need to locate the Dragon's Pulse");
        controls.StartDialogue("before whatever... that was... finds me...");

        yield return new WaitForSeconds(0.3f);
        controls.EnableControls();
        gameObject.SetActive(false);
    }

}


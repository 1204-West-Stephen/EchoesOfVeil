using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HallwayTrigger : MonoBehaviour
{
    public List<GameObject> doors;

    private void Start()
    {
        foreach (GameObject door in doors)
        {
            door.gameObject.SetActive(false);
        }
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

            gameObject.SetActive(false);
        }
    }

}


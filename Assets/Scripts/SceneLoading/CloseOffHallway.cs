using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloseOffHallway : MonoBehaviour
{
    public HallwayDoors door1;
    public HallwayDoors door2;

    public AudioClip closeDoors;
    public Material closeLeft;
    public Material closeRight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.UnloadSceneAsync("Jail Cell");
            door1.CloseDoor();
            door2.CloseDoor();

            AudioSource.PlayClipAtPoint(closeDoors, other.transform.position, 1f);

            gameObject.SetActive(false);

            JournalStateManager.Instance.UpdateMaterial(closeLeft, closeRight);
        }

    }

}

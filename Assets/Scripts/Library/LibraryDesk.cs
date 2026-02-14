using System.Collections;
using UnityEngine;

public class LibraryDesk : MonoBehaviour, i_Interactable
{
    public GameObject bookPrefab;
    public float spawnDistance = 0.25f;
    private PlayerControls player;
    private Camera cam;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerControls>();
        cam = Camera.main;
    }

    public void Interact()
    {
        if (bookPrefab == null || player == null || cam == null)
        {
            Debug.LogWarning("Missing prefab or player/cam");
            return;
        }

        GameObject bookGO = Instantiate(bookPrefab);
        Debug.Log("Book prefab instantiated: " + bookGO.name);

        BookViewer viewer = bookGO.GetComponent<BookViewer>();
        if (viewer != null)
            viewer.Init(player, cam);
        else
            Debug.LogWarning("BookViewer component missing on prefab");
    }

}

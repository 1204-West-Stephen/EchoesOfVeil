using UnityEngine;

public class LibraryDesk : MonoBehaviour, i_Interactable
{
    public GameObject bookPrefab;

    private PlayerControls player;
    private Camera cam;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerControls>();
        cam = Camera.main;
    }

    public void Interact()
    {
        if (bookPrefab == null || player == null || cam == null) return;

        // Spawn book viewer
        GameObject bookObj = Instantiate(bookPrefab);

        // Get script and initialize it
        BookViewer viewer = bookObj.GetComponent<BookViewer>();
        if (viewer != null)
        {
            viewer.Init(player, cam);
        }
        else
        {
            Debug.LogError("Book prefab is missing BookViewer component!");
        }
    }
}
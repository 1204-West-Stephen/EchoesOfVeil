using UnityEngine;
using UnityEngine.UI;

public class LibraryDesk : MonoBehaviour, i_Interactable
{
    public GameObject bookPrefab;
    public GameObject bookUI;

    [Header("UI Buttons")]
    public Button nextButton;
    public Button prevButton;
    public Button closeButton;

    private PlayerControls player;
    private Camera cam;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerControls>();
        cam = Camera.main;

        if (bookUI != null)
            bookUI.SetActive(false);
    }

    public void Interact()
    {
        if (bookPrefab == null || player == null || cam == null)
        {
            Debug.LogWarning("Missing prefab or player/cam");
            return;
        }

        player.DisableControls();

        if (bookUI != null)
            bookUI.SetActive(true);

        GameObject bookGO = Instantiate(bookPrefab);

        BookViewer viewer = bookGO.GetComponent<BookViewer>();

        if (viewer != null)
        {
            viewer.Init(player, cam, this);
        }
        else
        {
            Debug.LogWarning("BookViewer missing on prefab");
        }
    }
}
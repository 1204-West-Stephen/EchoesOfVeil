using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BookViewer : MonoBehaviour
{
    [Header("Transform Settings")]
    public Vector3 hiddenOffset = new Vector3(0f, -0.35f, 0f);
    public float slideDuration = 0.35f;
    public Vector3 spawnRotationEuler = Vector3.zero;
    public Vector3 spawnScale = Vector3.one;

    private LibraryDesk desk;
    private Animator animator;
    private PlayerControls player;

    private Vector3 shownLocalPos;
    private bool isOpen;
    private bool isClosing;

    // Called immediately after instantiation
    public void Init(PlayerControls owner, Camera cam, LibraryDesk sourceDesk)
    {
        player = owner;
        desk = sourceDesk;
        animator = GetComponent<Animator>();

        // Position in front of camera
        Vector3 spawnPos = cam.transform.position + cam.transform.forward * 0.25f;
        transform.position = spawnPos;
        transform.SetParent(cam.transform, true);

        transform.localEulerAngles = spawnRotationEuler;
        transform.localScale = spawnScale;

        shownLocalPos = transform.localPosition;
        transform.localPosition += hiddenOffset;

        player.DisableControls();

        // Hook up UI buttons to THIS instance
        SetupUI();

        StartCoroutine(SlideIn());
    }

    private void SetupUI()
    {
        if (desk == null || desk.bookUI == null)
        {
            Debug.LogWarning("BookViewer: Desk or UI missing");
            return;
        }

        Button nextButton = desk.nextButton;
        Button prevButton = desk.prevButton;
        Button closeButton = desk.closeButton;

        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(NextPage);
        }

        if (prevButton != null)
        {
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(PreviousPage);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(Close);
        }
    }

    private IEnumerator SlideIn()
    {
        Vector3 start = transform.localPosition;
        Vector3 end = shownLocalPos;
        float t = 0f;

        while (t < slideDuration)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(start, end, t / slideDuration);
            yield return null;
        }

        transform.localPosition = end;

        if (animator != null)
            animator.SetTrigger("go_ahead");

        isOpen = true;
    }

    public void Close()
    {
        if (!isOpen || isClosing) return;

        isClosing = true;

        if (desk != null && desk.bookUI != null)
            desk.bookUI.SetActive(false);

        StartCoroutine(CloseRoutine());
    }

    private IEnumerator CloseRoutine()
    {
        if (animator != null)
            animator.SetTrigger("go_back");

        yield return new WaitForSeconds(1.1f);

        Vector3 start = transform.localPosition;
        Vector3 end = shownLocalPos + hiddenOffset;
        float t = 0f;

        while (t < slideDuration)
        {
            t += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(start, end, t / slideDuration);
            yield return null;
        }

        transform.localPosition = end;

        if (player != null)
            player.EnableControls();

        Destroy(gameObject);
    }

    public void PreviousPage()
    {
        if (animator != null)
            animator.SetTrigger("go_back");
    }

    public void NextPage()
    {
        if (animator != null)
            animator.SetTrigger("go_ahead");
    }
}
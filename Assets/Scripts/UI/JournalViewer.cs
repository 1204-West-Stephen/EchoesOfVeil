using System.Collections;
using UnityEngine;

public class JournalViewer : MonoBehaviour
{
    private Animator animator;

    [Header("Slide Settings")]
    public Vector3 hiddenOffset = new Vector3(0f, -0.35f, 0f);
    public float slideDuration = 0.35f;

    [Header("Spawn Settings")]
    public float spawnDistance = 0.25f;
    public Vector3 spawnRotationEuler = Vector3.zero;
    public Vector3 spawnScale = Vector3.one;

    private PlayerControls player;
    private Vector3 shownLocalPos;
    private bool isOpen;
    private bool isClosing;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerControls>();
        animator = GetComponent<Animator>();
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

        player.EnableControls();
        Destroy(gameObject);
    }

    public void Init(PlayerControls owner, Camera cam)
    {
        player = owner;

        PositionInFrontOfCamera(cam); // move journal in front of camera

        transform.localEulerAngles = spawnRotationEuler;
        transform.localScale = spawnScale;

        shownLocalPos = transform.localPosition;
        transform.localPosition += hiddenOffset;

        player.DisableControls();
        StartCoroutine(SlideIn());
    }


    public void PositionInFrontOfCamera(Camera cam)
    {
        // World position exactly spawnDistance in front of camera
        Vector3 spawnPos = cam.transform.position + cam.transform.forward * spawnDistance;
        transform.position = spawnPos;

        // Parent to camera while keeping the exact world position
        transform.SetParent(cam.transform, true); // 'true' keeps world position unchanged
    }

}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class JournalViewer : MonoBehaviour
{
    private Animator animator;
    public List<GameObject> buttons;
    public AudioClip clip;
    public AudioClip closing;

    [Header("Slide Settings")]
    public Vector3 hiddenOffset = new Vector3(0f, -0.35f, 0f);
    public float slideDuration = 0.35f;

    [Header("Spawn Settings")]
    public float spawnDistance = 0.25f;
    public Vector3 spawnRotationEuler = Vector3.zero;
    public Vector3 spawnScale = Vector3.one;

    private PlayerControls player;
    public GameObject journalUI;
    private Vector3 shownLocalPos;
    private bool isOpen;
    private bool isClosing;

    public List<Canvas> canvases;

    public Material leftMaterial;
    public Material rightMaterial;

    public GameObject leftPage;
    public GameObject rightPage;

    private int flipCounter = 0;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerControls>();
        animator = GetComponent<Animator>();
    }

    public void pageLeft()
    {
        if (flipCounter > 0) {
            animator.SetTrigger("go_back");
            PlaySound(clip);
            StartCoroutine(ButtonVanish());

            flipCounter--;
        }
    }

    public void pageRight()
    {
        if (flipCounter < 4) {
            animator.SetTrigger("go_ahead");
            PlaySound(clip);
            StartCoroutine(ButtonVanish());
            flipCounter++;
        }
    }

    private IEnumerator ButtonVanish()
    {
        buttons[0].gameObject.SetActive(false);
        buttons[1].gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        buttons[0].gameObject.SetActive(true);
        buttons[1].gameObject.SetActive(true);
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
        {
            animator.SetTrigger("go_ahead");
            PlaySound(clip);
        }

        isOpen = true;
        yield return new WaitForSeconds(1f);
        journalUI.gameObject.SetActive(isOpen);
    }

    public void Close()
    {
        if (isClosing) return;
        isClosing = true;
        StartCoroutine(CloseRoutine());
    }
    private IEnumerator CloseRoutine()
    {
        if (animator != null) animator.SetTrigger("go_back");
        yield return new WaitForSeconds(0.12f);
        animator.SetTrigger("go_back");
        yield return new WaitForSeconds(0.12f);
        animator.SetTrigger("go_back");
        yield return new WaitForSeconds(0.12f);
        animator.SetTrigger("go_back");
        PlaySound(closing);
        yield return new WaitForSeconds(0.12f);
        animator.SetTrigger("go_back");
        yield return new WaitForSeconds(0.12f);
        animator.SetTrigger("go_back");
        yield return new WaitForSeconds(0.12f);
        animator.SetTrigger("go_back");

        journalUI.gameObject.SetActive(false);

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
        player.NotifyJournalClosed();
        Destroy(gameObject);
    }

    public void Init(PlayerControls owner, Camera cam)
    {
        player = owner;
        journalUI.SetActive(false);
        UpdateMaterials();
        PositionInFrontOfCamera(cam);

        transform.localEulerAngles = spawnRotationEuler;
        transform.localScale = spawnScale;

        shownLocalPos = transform.localPosition;
        transform.localPosition += hiddenOffset;

        player.DisableControls();
        StartCoroutine(SlideIn());
    }

    private void UpdateMaterials() 
    {
        leftMaterial = JournalStateManager.Instance.leftMaterial;
        rightMaterial = JournalStateManager.Instance.rightMaterial;

        Debug.Log(leftMaterial.name);
        Debug.Log(rightMaterial.name);

        SkinnedMeshRenderer leftSMR = leftPage.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer rightSMR = rightPage.GetComponent<SkinnedMeshRenderer>();

        Material[] leftMat = leftSMR.materials;
        Material[] rightMat = rightSMR.materials;

        leftMat[1] = leftMaterial;
        rightMat[0] = rightMaterial;

        leftSMR.materials = leftMat;
        rightSMR.materials = rightMat;
    }

    public void PositionInFrontOfCamera(Camera cam)
    {
        Vector3 spawnPos = cam.transform.position + cam.transform.forward * spawnDistance;
        transform.position = spawnPos;
        transform.SetParent(cam.transform, true);
    }
    public void PlaySound(AudioClip sound)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position, 0.2f);
    }


}

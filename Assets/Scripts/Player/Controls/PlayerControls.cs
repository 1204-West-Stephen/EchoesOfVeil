using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    [Header("Interactable")]
    public float interactionRange = 1f;
    public Transform interactionOrigin;
    public bool interacted;
    public bool canInteract;
    private i_Interactable currentInteractable;

    public PlayerInput controls;
    PlayerMovement movement;
    public PlayerCamera playerCamera;

    bool isPaused;
    public Canvas pauseMenu;
    bool pauseToggle;

    [Header("Held Item")]
    bool pressedF;
    private Inventory inventory;
    public Canvas ItemInHand;
    public RectTransform ItemInHandTransform;
    public Hotbar hotbar;
    public float moveDuration = 0.3f;
    public float moveDistance = 100f;

    bool pressedQ;
    private bool inspectionToggle;
    public Canvas inspectionMenu;

    private bool pressedJ;
    private bool journalToggle;
    public Canvas journalMenu;
    public Journal journal;

    [Header("Interaction UI")]
    public Canvas interactionCanvas;
    public Canvas internalDialogueCanvas;
    public TextMeshProUGUI internalDialogue;
    public GameObject crosshair;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        inventory = GetComponent<Inventory>();

        journal = GetComponent<Journal>();

        controls = new PlayerInput();

        controls.Movement.Interact.performed += _ => interacted = true;
        controls.Movement.Interact.canceled += _ => interacted = false;

        controls.Movement.PressF.performed += _ => pressedF = true;
        controls.Movement.PressF.canceled += _ => pressedF = false;

        controls.Movement.PressQ.performed += _ => pressedQ = true;
        controls.Movement.PressQ.canceled += _ => pressedQ = false;

        controls.Menus.Pause.performed += _ => isPaused = true;
        controls.Menus.Pause.canceled += _ => isPaused = false;

        controls.Menus.Journal.performed += _ => pressedJ = true;
        controls.Menus.Journal.canceled += _ => pressedJ = false;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Start()
    {
        pauseMenu.gameObject.SetActive(false);
        pauseToggle = false;

        if (interactionOrigin == null)
        {
            Debug.LogWarning("PlayerControls: interactionOrigin is not assigned. Using transform.position instead.");
            interactionOrigin = transform;
        }

        canInteract = true;
        inspectionToggle = false;
        inspectionMenu.gameObject.SetActive(false);
        journalToggle = false;
        journalMenu.gameObject.SetActive(false);

        if (interactionCanvas != null)
            interactionCanvas.gameObject.SetActive(false);

        if (internalDialogueCanvas != null)
        {
            internalDialogueCanvas.gameObject.SetActive(false);
            
            if (internalDialogue != null)
            {
                internalDialogue.text = " ";
            }
        }
    }

    private void Update()
    {
        if (pressedF)
        {
            StartCoroutine(MoveItemDownAndHide());
            pressedF = false;
        }

        if (interacted && canInteract)
        {
            Interacted();
            interacted = false;
        }

        if (canInteract)
        {
            AutoDetectInteractable();
        }

        if (isPaused)
        {
            PauseMenu();
            isPaused = false;
        }

        if (pressedQ)
        {
            inspectionToggle = !inspectionToggle;
            InspectionMenu();
            pressedQ = false;
        }

        if (pressedJ && journal != null && journal.journalAcquired)
        {
            journalToggle = !journalToggle;
            JournalMenu();
            pressedJ = false;
        }

    }

    private void AutoDetectInteractable()
    {
        LayerMask interactableLayer = LayerMask.GetMask("Interactable");
        Camera cam = Camera.main;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            i_Interactable interactable = hit.collider.GetComponent<i_Interactable>();
            if (interactable != null)
            {
                currentInteractable = interactable;

                if (interactionCanvas != null && !interactionCanvas.gameObject.activeSelf)
                    interactionCanvas.gameObject.SetActive(true);
            }
        }
        else
        {
            currentInteractable = null;

            if (interactionCanvas != null && interactionCanvas.gameObject.activeSelf)
                interactionCanvas.gameObject.SetActive(false);
        }
    }

    private void Interacted()
    {
        if (currentInteractable != null)
        { 
            currentInteractable.Interact();
        }
    }

    private IEnumerator MoveItemDownAndHide()
    {
        Vector3 startPos = ItemInHandTransform.anchoredPosition;
        Vector3 endPos = startPos - new Vector3(0, moveDistance, 0);
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            ItemInHandTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ItemInHandTransform.anchoredPosition = endPos;
        ItemInHand.gameObject.SetActive(false);

        ItemInHandTransform.anchoredPosition = startPos;
    }

    public void PauseMenu()
    {
        pauseToggle = !pauseToggle;
        pauseMenu.gameObject.SetActive(pauseToggle);

        if (pauseToggle)
        {
            Time.timeScale = 0f;
            DisableControls();
        }
        else
        {
            Time.timeScale = 1f;
            EnableControls();
        }
    }

    private void InspectionMenu()
    {
        if (inspectionToggle)
        {
            DisableControls();
            inspectionMenu.gameObject.SetActive(true);

            if (interactionCanvas != null)
                interactionCanvas.gameObject.SetActive(false);
        }
        else
        {
            EnableControls();
            inspectionMenu.gameObject.SetActive(false);
        }
    }

    public void DisableControls()
    {
        movement.controlLock();
        playerCamera.controlLock();
        canInteract = false;
        Cursor.visible = true;
    }

    public void EnableControls()
    {
        movement.controlUnlock();
        playerCamera.controlUnlock();
        canInteract = true;
        Cursor.visible = false;
    }

    private void JournalMenu()
    {
        if (journalToggle)
        {
            DisableControls();
            journalMenu.gameObject.SetActive(true);

            if (interactionCanvas != null)
                interactionCanvas.gameObject.SetActive(false);
        }
        else
        {
            EnableControls();
            journalMenu.gameObject.SetActive(false);
            
        }
    }

    public void StartDialogue(string message)
    {
        StartCoroutine(ShowDialogue(message));
    }

    private IEnumerator ShowDialogue(string message)
    {
        internalDialogue.text = message;

        internalDialogueCanvas.gameObject.SetActive(true);

        CanvasGroup canvasGroup = internalDialogueCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = internalDialogueCanvas.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(3f);

        float fadeDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        internalDialogueCanvas.gameObject.SetActive(false);
    }

    public void OnJournalAcquired(Journal newJournal)
    {
        journal = newJournal;
    }


    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

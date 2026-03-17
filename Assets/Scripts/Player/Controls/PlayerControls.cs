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
    
    [Header("Player Controls")] 
    public PlayerInput controls; 
    public PlayerCamera playerCamera;
    private PlayerMovement movement;

    [Header("Pause Menu")] 
    public Canvas pauseMenu; 
    private bool pauseToggle;
    private bool isPaused;

    [Header("Held Item")] 
    public Canvas ItemInHand; 
    public RectTransform ItemInHandTransform; 
    public Hotbar hotbar;
    private bool pressedF;
    private Inventory inventory;

    [Header("Movement")]
    public float moveDuration = 0.3f; 
    public float moveDistance = 100f; 
    
    [Header("Inspection Menu")] 
    public Canvas inspectionMenu;
    private bool pressedQ;
    private bool inspectionToggle;

    [Header("Interaction UI")] 
    public Canvas interactionCanvas; 
    public Canvas internalDialogueCanvas; 
    public TextMeshProUGUI internalDialogue; 
    public GameObject crosshair;
    private Queue<string> dialogueQueue = new Queue<string>();
    private bool dialogueRunning = false;

    [Header("Journal")] 
    public bool hasJournal; 
    public GameObject journalPrefab; 
    public Journal journal;
    private JournalViewer activeJournal;
    private bool pressedJ;
    public bool IsDialogueRunning => dialogueRunning;

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

        controls.Menus.Submit.performed += _ => SkipDialogue();
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

        if (pressedJ && hasJournal)
        {
            if (activeJournal == null)
                OpenJournal();
            else
                activeJournal.Close();

            pressedJ = false;
        }

    }

    // ========== INTERACTION ========== \\
    private void AutoDetectInteractable()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        LayerMask interactableLayer = LayerMask.GetMask("Interactable");

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            i_Interactable interactable = hit.collider.GetComponent<i_Interactable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                if (interactionCanvas && !interactionCanvas.gameObject.activeSelf)
                    interactionCanvas.gameObject.SetActive(true);
            }
        }
        else
        {
            currentInteractable = null;
            if (interactionCanvas && interactionCanvas.gameObject.activeSelf)
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

    // ========== MENUS ========== \\
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

    // ========== CONTROL LOCKS ========== \\
    public void DisableControls()
    {
        movement.controlLock();
        playerCamera.controlLock();
        canInteract = false;
        Cursor.visible = true;
    }

    public void DisableMovementOnly()
    {
        movement.controlLock();   // stops WASD
        canInteract = false;
    }
    public void EnableControls()
    {
        movement.controlUnlock();
        playerCamera.controlUnlock();
        canInteract = true;
        Cursor.visible = false;
    }

    public void EnableMovementOnly()
    {
        movement.controlUnlock();   // stops WASD
        canInteract = false;
    }

    // ========== JOURNAL ========== \\
    public void AcquireJournal()
    {
        hasJournal = true;
    }
    private void OpenJournal()
    {
        Camera cam = Camera.main;

        Vector3 spawnPos =
            cam.transform.position +
            cam.transform.forward * 0.6f; // distance from camera

        Quaternion spawnRot =
            Quaternion.LookRotation(cam.transform.forward, cam.transform.up);

        GameObject journalGO = Instantiate(journalPrefab, spawnPos, spawnRot);

        // Parent AFTER spawning so position stays correct
        journalGO.transform.SetParent(cam.transform, true);

        activeJournal = journalGO.GetComponent<JournalViewer>();
        activeJournal.Init(this, cam);
    }
    public void OnJournalAcquired(Journal newJournal)
    {
        journal = newJournal;
    }
    public void NotifyJournalClosed()
    {
        activeJournal = null;
    }

    // ========== DIALOGUE ========== \\
    public void StartDialogue(string message)
    {
        dialogueQueue.Enqueue(message);

        if (!dialogueRunning)
            StartCoroutine(ProcessDialogueQueue());
    }
    private IEnumerator ProcessDialogueQueue()
    {
        dialogueRunning = true;

        while (dialogueQueue.Count > 0)
        {
            string nextMessage = dialogueQueue.Dequeue();
            yield return StartCoroutine(ShowDialogue(nextMessage));
        }

        internalDialogueCanvas.gameObject.SetActive(false);
        dialogueRunning = false;
    }
    private IEnumerator ShowDialogue(string message)
    {
        internalDialogue.text = message;
        internalDialogueCanvas.gameObject.SetActive(true);

        CanvasGroup canvasGroup = internalDialogueCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = internalDialogueCanvas.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 1f;

        // Stay on screen
        yield return new WaitForSeconds(3.85f);

        // Fade Out
        float fadeDuration = 1f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }

    public void SkipDialogue()
    {
        if (!dialogueRunning) return;

        StopAllCoroutines(); // stops current dialogue coroutine

        dialogueQueue.Clear();
        dialogueRunning = false;

        if (internalDialogueCanvas != null)
            internalDialogueCanvas.gameObject.SetActive(false);
    }

    // ========== EXIT ========== \\
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

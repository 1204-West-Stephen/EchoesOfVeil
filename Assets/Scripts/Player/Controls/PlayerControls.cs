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
    public bool isPaused;

    [Header("Held Item")] 
    public Hotbar hotbar;
    private bool pressedF;
    private Inventory inventory;
    public float itemSpawnDistance = 0.5f;
    private GameObject spawnedHeldItem;
    [Header("Held Item Anchor")]
    public Transform itemAnchor;

    [Header("Movement")]
    public float moveDuration = 0.3f; 
    public float moveDistance = 100f; 

    [Header("Options Menu")]
    public Canvas optionsCanvas;
    private bool optionsToggle = false;

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

        controls.Menus.Pause.performed += _ => HandleEscape();

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
        if (pressedF && spawnedHeldItem != null)
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


        if (pressedJ && hasJournal)
        {
            if (activeJournal == null)
            {
                OpenJournal();
                isPaused = false;
            }
            else
            {
                activeJournal.Close();
                isPaused = false;
            }

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
            Debug.Log("Hit: " + hit.collider.name + " | Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
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
    public void ClearInventory()
    {
        for (int i = 0; i < inventory.inventory.Count; i++)
        {
            inventory.inventory[i] = null;
        }

        inventory.selectedIndex = 0;
        ClearHeldItem();

        Debug.Log("Inventory Cleared Completely");
    }

    // ========== MENUS ========== \\
    private IEnumerator MoveItemDownAndHide()
    {
        Vector3 startPos = spawnedHeldItem.transform.position;
        Vector3 endPos = startPos - new Vector3(0, moveDistance, 0);
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            spawnedHeldItem.transform.position = Vector3.Lerp(startPos, endPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spawnedHeldItem.transform.position = endPos;
        spawnedHeldItem.gameObject.SetActive(false);

        spawnedHeldItem.transform.position = startPos;
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
    public void OptionsMenu()
    {
        optionsToggle = !optionsToggle;
        optionsCanvas.gameObject.SetActive(optionsToggle);
        pauseMenu.gameObject.SetActive(!optionsToggle);
    }
    public void SpawnSelectedHotbarItem(ItemData item)
    {
        if (item == null || item.prefab == null) return;

        if (spawnedHeldItem != null)
            Destroy(spawnedHeldItem);

        Transform anchor = itemAnchor != null ? itemAnchor : Camera.main.transform;

        spawnedHeldItem = Instantiate(
            item.prefab,
            anchor.position,
            anchor.rotation
        );

        spawnedHeldItem.transform.SetParent(anchor, true);

        spawnedHeldItem.transform.localScale = item.inHandScale;
        spawnedHeldItem.transform.localRotation = Quaternion.Euler(item.inHandRotation);

        Rigidbody rb = spawnedHeldItem.GetComponent<Rigidbody>();
        if (rb != null) Destroy(rb);

        foreach (var c in spawnedHeldItem.GetComponentsInChildren<Collider>())
            c.enabled = false;
    }
    public void ClearHeldItem()
    {
        if (spawnedHeldItem != null)
        {
            Destroy(spawnedHeldItem);
            spawnedHeldItem = null;
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
    private void HandleEscape()
    {
        // 1. Options menu open  close it
        if (optionsCanvas.gameObject.activeSelf)
        {
            optionsToggle = false;
            optionsCanvas.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
            return;
        }

        // 2. Journal open  close it
        if (activeJournal != null)
        {
            activeJournal.Close();
            return;
        }

        // 3. Pause menu  close it
        if (pauseMenu.gameObject.activeSelf)
        {
            PauseMenu(); // this will unpause
            return;
        }

        // 4. Nothing open  open pause menu
        PauseMenu();
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

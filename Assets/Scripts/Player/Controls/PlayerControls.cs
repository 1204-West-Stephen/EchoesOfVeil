using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    [Header("Interactable")]
    public float interactionRange = 1f;
    public Transform interactionOrigin;
    bool interacted;
    private bool canInteract;

    PlayerInput controls;
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
    public float moveDuration = 0.3f;
    public float moveDistance = 100f;

    bool pressedQ;
    private bool inspectionToggle;
    public Canvas inspectionMenu;

    private bool pressedJ;
    private bool journalToggle;
    public Canvas journalMenu;
    public Journal journal;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        inventory = GetComponent<Inventory>();

        if (journal == null)
        {
            Debug.LogError("PlayerControls: Journal component is missing from player.");
        }

        controls = new PlayerInput();

        controls.Movement.Interact.performed += _ => interacted = true;
        controls.Movement.Interact.canceled += _ => interacted = false;

        controls.Menus.Pause.performed += _ => isPaused = true;
        controls.Menus.Pause.canceled += _ => isPaused = false;

        controls.Movement.PressF.performed += _ => pressedF = true;
        controls.Movement.PressF.canceled += _ => pressedF = false;

        controls.Movement.PressQ.performed += _ => pressedQ = true;
        controls.Movement.PressQ.canceled += _ => pressedQ = false;

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

        if (isPaused)
        {
            PauseMenu();
            isPaused = false;
        }

        if (pressedQ)
        {
            Debug.Log("Q pressed");
            inspectionToggle = !inspectionToggle;
            InspectionMenu();
            pressedQ = false;
        }

        if (pressedJ && journal.journalAcquired)
        {
            Debug.Log("J pressed");
            journalToggle = !journalToggle;
            JournalMenu();
            pressedJ = false;
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

    private void Interacted()
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
                interactable.Interact();
            }
        }
    }

    public void PauseMenu()
    {
        pauseToggle = !pauseToggle;
        pauseMenu.gameObject.SetActive(pauseToggle);

        if (pauseToggle)
        {
            Time.timeScale = 0f;
            movement.controlLock();
            playerCamera.controlLock();
        }
        else
        {
            Time.timeScale = 1f; // Resume game time
            movement.controlUnlock();
            playerCamera.controlUnlock();
        }
    }

    private void InspectionMenu()
    {
        if (inspectionToggle)
        {
            movement.controlLock();
            playerCamera.controlLock();
            canInteract = false;
            inspectionMenu.gameObject.SetActive(true);
        }
        else if (!inspectionToggle)
        {
            movement.controlUnlock();
            playerCamera.controlUnlock();
            canInteract = true;
            inspectionMenu.gameObject.SetActive(false);
        }
    }
    private void JournalMenu()
    {
        if (journalToggle)
        {
            movement.controlLock();
            playerCamera.controlLock();
            canInteract = false;
            journalMenu.gameObject.SetActive(true);
        }
        else if (!journalToggle)
        {
            movement.controlUnlock();
            playerCamera.controlUnlock();
            canInteract = true;
            journalMenu.gameObject.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
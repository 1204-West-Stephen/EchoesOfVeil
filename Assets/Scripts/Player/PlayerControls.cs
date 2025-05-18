using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    [Header("Interactable")]
    public float interactionRange = 1f;
    bool interacted;

    PlayerInput controls;
    PlayerMovement movement;
    public PlayerCamera playerCamera;

    bool isPaused;
    public Canvas pauseMenu;
    bool pauseToggle;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();

        controls = new PlayerInput();

        controls.Movement.Interact.performed += _ => interacted = true;
        controls.Movement.Interact.canceled += _ => interacted = false;

        controls.Menus.Pause.performed += _ => isPaused = true;
        controls.Menus.Pause.canceled += _ => isPaused = false;
    }
    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Start()
    {
        pauseMenu.gameObject.SetActive(false);
        pauseToggle = false;
    }
    private void Update()
    {
        if (interacted)
        {
            Interacted();
            interacted = false;
        }

        if (isPaused)
        {
            PauseMenu();
            isPaused = false;
        }
    }

    private void Interacted()
    {
        LayerMask interactableLayer = LayerMask.GetMask("Interactable");

        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);

        foreach (Collider hit in hits)
        {
            i_Interactable interactable = hit.GetComponent<i_Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
                break;
            }
        }
    }

    private void PauseMenu()
    {
        pauseToggle = !pauseToggle;
        pauseMenu.gameObject.SetActive(pauseToggle);
        
        if (pauseToggle)
        {
            movement.controlLock();
            playerCamera.controlLock();
        }
        else
        {
            movement.controlUnlock();
            playerCamera.controlUnlock();
        }
    }


}

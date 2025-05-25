using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2f;
    public float groundDrag = 5f;

    [Header("Stamina")]
    public float staminaDrainRate = 0.4f;
    public float staminaRegenRate = 0.2f;
    public float staminaRegenDelay = 2f;

    private float stamina = 1f;
    private float lastSprintTime;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public float groundCheckDistance = 1.2f;
    private float originalDragDistance;
    private float crouchDragDistance;
    public LayerMask Ground;
    bool grounded;

    [Header("References")]
    public Transform orientation;
    public Transform cameraTransform;
    public RectTransform staminaBar;
    public Image staminaBarImage;
    public Image staminaBarBackground;

    [Header("Stamina UI Fade")]
    public float fadeSpeed = 2f;

    Rigidbody rb;
    PlayerInput controls;

    bool moveForward, moveBackward, moveLeft, moveRight;
    bool moved, crouchPressed;

    bool sprintInput;
    bool isRecharging;
    float currentMoveSpeed;
    private bool fadeOutAllowed = true;
    private Coroutine fadeOutDelayCoroutine;
    private bool wasStaminaFull = true;

    // Crouch handling
    private bool isCrouching = false;
    private float originalHeight;
    private float originalMoveSpeed;
    private float originalCameraY;

    [Header("Crouch Settings")]
    public float crouchCameraYOffset = 0.2f;

    private void Awake()
    {
        controls = new PlayerInput();

        controls.Movement.Forward.performed += _ => moveForward = true;
        controls.Movement.Forward.canceled += _ => moveForward = false;

        controls.Movement.Backward.performed += _ => moveBackward = true;
        controls.Movement.Backward.canceled += _ => moveBackward = false;

        controls.Movement.Left.performed += _ => moveLeft = true;
        controls.Movement.Left.canceled += _ => moveLeft = false;

        controls.Movement.Right.performed += _ => moveRight = true;
        controls.Movement.Right.canceled += _ => moveRight = false;

        controls.Movement.Sprint.performed += _ => sprintInput = true;
        controls.Movement.Sprint.canceled += _ => sprintInput = false;

        controls.Movement.Crouch.performed += _ => crouchPressed = true;
        controls.Movement.Crouch.canceled += _ => crouchPressed = false;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        currentMoveSpeed = moveSpeed;

        originalHeight = playerHeight;
        originalMoveSpeed = moveSpeed;
        originalCameraY = cameraTransform.localPosition.y;

        originalDragDistance = groundCheckDistance;
        crouchDragDistance = groundCheckDistance / 4;

    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, Ground);
        rb.drag = grounded ? (isCrouching ? 1.0f : groundDrag) : 0f;

        HandleStamina();

        if (!isCrouching)
        {
            FadeStaminaBar();
        }

        moved = (moveForward || moveBackward || moveLeft || moveRight || isCrouching);

        if (crouchPressed)
        {
            isCrouching = !isCrouching;
            ToggleCrouch();
            crouchPressed = false;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
        UpdateStamina();
    }

    private void LateUpdate()
    {
        if (transform.position.y < -20f)
        {
            transform.position = new Vector3(0, 5, 0);
            rb.velocity = Vector3.zero;
        }
    }

    private void HandleStamina()
    {
        wasStaminaFull = stamina >= 1f;

        if (sprintInput)
            lastSprintTime = Time.time;

        // Only allow sprinting if NOT crouching
        bool canActuallySprint = moved && sprintInput && stamina > 0f && !isCrouching;

        if (canActuallySprint)
        {
            currentMoveSpeed = sprintSpeed;
            stamina -= staminaDrainRate * Time.deltaTime;
            stamina = Mathf.Clamp01(stamina);
        }
        else
        {
            currentMoveSpeed = isCrouching ? crouchSpeed : moveSpeed;

            if (Time.time - lastSprintTime >= staminaRegenDelay)
            {
                stamina += staminaRegenRate * Time.deltaTime;
                stamina = Mathf.Clamp01(stamina);
            }
        }

        isRecharging = stamina < 1f;

        if (!wasStaminaFull && stamina >= 1f)
        {
            if (fadeOutDelayCoroutine != null)
                StopCoroutine(fadeOutDelayCoroutine);

            fadeOutDelayCoroutine = StartCoroutine(DelayFadeOut());
        }
    }


    private void FadeStaminaBar()
    {
        float timeSinceSprint = Time.time - lastSprintTime;
        Color color = staminaBarImage.color;
        Color color2 = staminaBarBackground.color;

        if (!isRecharging && timeSinceSprint >= 3f && fadeOutAllowed)
        {
            color.a = Mathf.Lerp(color.a, 0f, Time.deltaTime * fadeSpeed);
            color2.a = Mathf.Lerp(color2.a, 0f, Time.deltaTime * fadeSpeed);
        }
        else
        {
            color.a = Mathf.Lerp(color.a, 1f, Time.deltaTime * fadeSpeed);
            color2.a = Mathf.Lerp(color2.a, 1f, Time.deltaTime * fadeSpeed);
        }

        staminaBarImage.color = color;
        staminaBarBackground.color = color2;
    }

    private void MovePlayer()
    {
        float horizontalInput = (moveRight ? 1f : 0f) - (moveLeft ? 1f : 0f);
        float verticalInput = (moveForward ? 1f : 0f) - (moveBackward ? 1f : 0f);

        Vector3 moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        float forceMultiplier = isCrouching ? 20f : 10f; // double the force for crouching
        rb.AddForce(moveDirection.normalized * currentMoveSpeed * forceMultiplier, ForceMode.Force);

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float maxSpeed = currentMoveSpeed;

        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void UpdateStamina()
    {
        Vector3 staminaScale = staminaBar.transform.localScale;
        staminaScale.x = stamina;
        staminaBar.transform.localScale = staminaScale;
    }

    private IEnumerator DelayFadeOut()
    {
        fadeOutAllowed = false;
        yield return new WaitForSeconds(1f);
        fadeOutAllowed = true;
    }

    private void ToggleCrouch()
    {
        if (cameraTransform != null)
        {
            Vector3 camPos = cameraTransform.localPosition;
            camPos.y = isCrouching ? originalCameraY - crouchCameraYOffset : originalCameraY;
            StartCoroutine(SmoothCameraHeight(camPos));
        }

    }

    private IEnumerator SmoothCameraHeight(Vector3 targetPos)
    {
        float elapsed = 0f;
        float duration = 0.2f;
        Vector3 startPos = cameraTransform.localPosition;

        while (elapsed < duration)
        {
            cameraTransform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = targetPos;
    }

    public void controlLock()
    {
        controls.Disable();
        Cursor.lockState = CursorLockMode.None;
    }

    public void controlUnlock()
    {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }
}

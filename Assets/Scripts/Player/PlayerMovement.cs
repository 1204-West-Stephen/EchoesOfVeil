using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float groundDrag = 5f;

    [Header("Stamina")]
    public float staminaDrainRate = 0.4f; // per second
    public float staminaRegenRate = 0.2f; // per second
    public float staminaRegenDelay = 2f;  // seconds after last sprint

    private float stamina = 1f; // normalized (0 to 1)
    private float lastSprintTime;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask Ground;
    bool grounded;

    public Transform orientation;

    Rigidbody rb;
    PlayerInput controls;

    bool moveForward, moveBackward, moveLeft, moveRight;
    bool sprintInput;

    float currentMoveSpeed;

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
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        currentMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);
        rb.drag = grounded ? groundDrag : 0f;

        HandleStamina();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void HandleStamina()
    {
        bool canActuallySprint = sprintInput && stamina > 0;

        if (canActuallySprint)
        {
            currentMoveSpeed = sprintSpeed;
            stamina -= staminaDrainRate * Time.deltaTime;
            stamina = Mathf.Clamp01(stamina);
            lastSprintTime = Time.time;
        }
        else
        {
            currentMoveSpeed = moveSpeed;

            if (Time.time - lastSprintTime >= staminaRegenDelay)
            {
                stamina += staminaRegenRate * Time.deltaTime;
                stamina = Mathf.Clamp01(stamina);
            }
        }
    }

    private void MovePlayer()
    {
        float horizontalInput = (moveRight ? 1f : 0f) - (moveLeft ? 1f : 0f);
        float verticalInput = (moveForward ? 1f : 0f) - (moveBackward ? 1f : 0f);

        Vector3 moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f, ForceMode.Force);
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

}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    bool grounded;

    public Transform orientation;

    Rigidbody rb;

    PlayerInput controls;

    bool moveForward, moveBackward, moveLeft, moveRight;

    private void Awake()
    {
        controls = new PlayerInput();

        // Use the Movement action map
        controls.Movement.Forward.performed += _ => moveForward = true;
        controls.Movement.Forward.canceled += _ => moveForward = false;

        controls.Movement.Backward.performed += _ => moveBackward = true;
        controls.Movement.Backward.canceled += _ => moveBackward = false;

        controls.Movement.Left.performed += _ => moveLeft = true;
        controls.Movement.Left.canceled += _ => moveLeft = false;

        controls.Movement.Right.performed += _ => moveRight = true;
        controls.Movement.Right.canceled += _ => moveRight = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        rb.drag = grounded ? groundDrag : 0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void MovePlayer()
    {
        float horizontalInput = (moveRight ? 1f : 0f) - (moveLeft ? 1f : 0f);
        float verticalInput = (moveForward ? 1f : 0f) - (moveBackward ? 1f : 0f);

        Vector3 moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 playerVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (playerVel.magnitude > moveSpeed)
        {
            Vector3 velocity = playerVel.normalized * moveSpeed;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
    }
}

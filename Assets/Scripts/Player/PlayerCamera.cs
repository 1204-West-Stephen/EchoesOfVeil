using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float xSensitivity = 10f;
    public float ySensitivity = 10f;

    public Transform orientation;

    float rotateX;
    float rotateY;

    PlayerInput controls;
    Vector2 mouseDelta;

    private void Awake()
    {
        controls = new PlayerInput();

        // Hook up the Look input
        controls.Movement.Look.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        controls.Movement.Look.canceled += ctx => mouseDelta = Vector2.zero;
    }
    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 startRotation = transform.rotation.eulerAngles;
        rotateX = startRotation.x;
        rotateY = startRotation.y;
    }

    private void Update()
    {
        float mouseX = mouseDelta.x * Time.deltaTime * xSensitivity;
        float mouseY = mouseDelta.y * Time.deltaTime * ySensitivity;

        rotateX -= mouseY;
        rotateY += mouseX;

        rotateX = Mathf.Clamp(rotateX, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotateX, rotateY, 0);
        orientation.rotation = Quaternion.Euler(0, rotateY, 0);
    }
    public void controlLock()
    {
        controls.Disable();
    }

    public void controlUnlock()
    {
        controls.Enable();
    }
}

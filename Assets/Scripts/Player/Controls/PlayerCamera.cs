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
        
        Camera.main.nearClipPlane = 0.01f; // default is often 0.3

        Vector3 startRotation = transform.rotation.eulerAngles;
        rotateX = startRotation.x;
        rotateY = startRotation.y;
    }

    private void Update()
    {
        // Existing camera control logic
        float mouseX = mouseDelta.x * Time.deltaTime * xSensitivity;
        float mouseY = mouseDelta.y * Time.deltaTime * ySensitivity;

        rotateX -= mouseY;
        rotateY += mouseX;
        rotateX = Mathf.Clamp(rotateX, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotateX, rotateY, 0);
        orientation.rotation = Quaternion.Euler(0, rotateY, 0);

        // Call raycast here
        RaycastFromCamera();
    }

    public void controlLock()
    {
        controls.Disable();
    }

    public void controlUnlock()
    {
        controls.Enable();
    }

    public void RaycastFromCamera()
    {
        Camera cam = Camera.main;
        Vector3 origin = cam.transform.position;
        Vector3 direction = cam.transform.forward;

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f)) // 100 units max distance
        {
            Debug.Log("Hit: " + hit.collider.name);
            // Optional: Draw line in Scene view
            Debug.DrawLine(origin, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(origin, origin + direction * 100f, Color.green);
        }
    }

}

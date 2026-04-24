using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    public float xSensitivity = 100f;
    public float ySensitivity = 100f;

    public Transform orientation;

    float rotateX;
    float rotateY;

    PlayerInput controls;
    [SerializeField] private Slider sensitivitySlider;

    private void Awake()
    {
        controls = new PlayerInput();
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

        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);

            // optional: initialize from current value
            SetSensitivity(sensitivitySlider.value);
        }
    }

    private void Update()
    {
        Vector2 mouseDelta = controls.Movement.Look.ReadValue<Vector2>();

        float mouseX = mouseDelta.x * (xSensitivity * 0.001f);
        float mouseY = mouseDelta.y * (ySensitivity * 0.001f);
        
        rotateX -= mouseY;
        rotateY += mouseX;
        rotateX = Mathf.Clamp(rotateX, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotateX, rotateY, 0);
        orientation.rotation = Quaternion.Euler(0, rotateY, 0);

        RaycastFromCamera();
    }


    public void SetSensitivity(float value)
    {
        xSensitivity = value;
        ySensitivity = value;
    }
    public void RaycastFromCamera()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 origin = cam.transform.position;

        Vector3 direction = cam.transform.forward;
    }

    public void controlLock() { controls.Disable(); }
    public void controlUnlock() { controls.Enable(); }
}

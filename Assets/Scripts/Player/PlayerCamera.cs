using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public float xSensitivity;
    public float ySensitivity;

    public Transform orientation;

    float rotateX;
    float rotateY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySensitivity;

        rotateX -= mouseY;
        rotateY += mouseX;

        rotateX = Mathf.Clamp(rotateX, -90f, 90f);
        
        //rotate camera
        transform.rotation = Quaternion.Euler(rotateX, rotateY, 0);
        orientation.rotation = Quaternion.Euler(0, rotateY, 0);
    }

}

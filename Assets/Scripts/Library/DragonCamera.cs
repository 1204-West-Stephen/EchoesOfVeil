using System.Collections;
using UnityEngine;

public class DragonCamera : MonoBehaviour
{
    private PlayerControls controls;
    private PlayerMovement move;
    public GameObject dragonCam;

    private void Start()
    {
        controls = FindFirstObjectByType<PlayerControls>();
        dragonCam.gameObject.SetActive(false);
    }

    public void StartCamera()
    {
        StartCoroutine(CameraSwap());
    }

    private IEnumerator CameraSwap()
    {
        move.controlLock();
        controls.canInteract = false;
        dragonCam.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        dragonCam.gameObject.SetActive(false);
        controls.canInteract = true;
        move.controlUnlock();

        yield return null;
    }
}

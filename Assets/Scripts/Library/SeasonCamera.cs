using System.Collections;
using UnityEngine;

public class SeasonCamera : MonoBehaviour
{
    private PlayerControls controls;
    private PlayerMovement move;
    public GameObject seasonCam;

    private void Start()
    {
        controls = FindFirstObjectByType<PlayerControls>();
        seasonCam.gameObject.SetActive(false);
    }

    public void StartCamera()
    {
        StartCoroutine(CameraSwap());
    }

    private IEnumerator CameraSwap()
    {
        move.controlLock();
        controls.canInteract = false;
        seasonCam.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        seasonCam.gameObject.SetActive(false);
        controls.canInteract = true;
        move.controlUnlock();

        yield return null;
    }
}

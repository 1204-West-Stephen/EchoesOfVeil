using System.Collections;
using UnityEngine;

public class DragonCamera : MonoBehaviour
{
    private PlayerControls controls;
    private PlayerMovement move;

    private void Start()
    {
        controls = FindFirstObjectByType<PlayerControls>();
        gameObject.SetActive(false);
    }

    public void StartCamera()
    {
        StartCoroutine(CameraSwap());
    }

    private IEnumerator CameraSwap()
    {
        move.controlLock();
        controls.canInteract = false;
        gameObject.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        gameObject.SetActive(false);
        controls.canInteract = true;
        move.controlUnlock();

        yield return null;
    }
}

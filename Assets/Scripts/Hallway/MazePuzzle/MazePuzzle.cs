using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MazePuzzle : MonoBehaviour, i_Interactable
{
    public Transform puzzleViewPosition;   // World-space target (can be child of something)
    public float transitionDuration = 0.5f;

    private PlayerControls player;
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamController; // your camera controller script
    private Camera mainCamera;

    private bool inPuzzleView = false;
    private bool canToggle = true;

    // For restoring
    private Transform originalParent;
    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;
    private Vector3 originalWorldPos;
    private Quaternion originalWorldRot;

    private Coroutine transitionCoroutine;

    private void Update()
    {
        // Allow toggling out with the same Interact action while in puzzle view
        if (inPuzzleView && canToggle && player != null && player.controls.Movement.Interact.triggered)
        {
            ExitPuzzleView();
        }
    }

    public void Interact()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerControls>();
            playerMovement = FindFirstObjectByType<PlayerMovement>();
            mainCamera = Camera.main;
            if (player != null) playerCamController = player.playerCamera; // assumes your PlayerControls has this reference
        }

        if (mainCamera == null || player == null || playerMovement == null || puzzleViewPosition == null)
            return;

        if (!inPuzzleView && canToggle)
        {
            EnterPuzzleView();
        }
    }

    private void EnterPuzzleView()
    {
        canToggle = false;
        inPuzzleView = true;

        // Save original parent + local (to restore exactly), and world (for smooth return)
        originalParent = mainCamera.transform.parent;
        originalLocalPos = mainCamera.transform.localPosition;
        originalLocalRot = mainCamera.transform.localRotation;
        originalWorldPos = mainCamera.transform.position;
        originalWorldRot = mainCamera.transform.rotation;

        // Fully disable camera controller so it stops writing to the transform
        if (playerCamController != null) playerCamController.enabled = false;

        // Detach so player transforms can’t affect it during the puzzle
        mainCamera.transform.SetParent(null, true);

        // Lock player controls & hide HUD
        player.DisableControls();
        SetHUD(false);

        // Smoothly move to the puzzle view (world space)
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(SmoothMove(
            mainCamera.transform,
            puzzleViewPosition.position,
            puzzleViewPosition.rotation,
            transitionDuration,
            onComplete: () => { canToggle = true; }
        ));
    }

    private void ExitPuzzleView()
    {
        canToggle = false;
        inPuzzleView = false;

        // Keep controls OFF during the return move to avoid the controller fighting the transition
        // Smoothly move back to the saved world transform
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(SmoothMove(
            mainCamera.transform,
            originalWorldPos,
            originalWorldRot,
            transitionDuration,
            onComplete: () =>
            {
                // Reparent and restore exact local transform so your camera controller resumes seamlessly
                mainCamera.transform.SetParent(originalParent, worldPositionStays: false);
                mainCamera.transform.localPosition = originalLocalPos;
                mainCamera.transform.localRotation = originalLocalRot;

                // Re-enable camera controller now that transforms are restored
                if (playerCamController != null) playerCamController.enabled = true;

                // Show HUD and re-enable controls
                SetHUD(true);
                player.EnableControls();

                canToggle = true;
            }
        ));
    }

    private void SetHUD(bool on)
    {
        player.hotbar.gameObject.SetActive(on);
        player.crosshair.gameObject.SetActive(on);
        player.interactionCanvas.gameObject.SetActive(on);
        playerMovement.staminaBar.gameObject.SetActive(on);
        playerMovement.staminaBarImage.gameObject.SetActive(on);
        playerMovement.staminaBarBackground.gameObject.SetActive(on);
        player.canInteract = on;
    }

    private IEnumerator SmoothMove(Transform cam, Vector3 targetPos, Quaternion targetRot, float duration, System.Action onComplete)
    {
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // SmoothStep easing
            t = t * t * (3f - 2f * t);

            cam.position = Vector3.Lerp(startPos, targetPos, t);
            cam.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        cam.position = targetPos;
        cam.rotation = targetRot;

        transitionCoroutine = null;
        onComplete?.Invoke();
    }
}

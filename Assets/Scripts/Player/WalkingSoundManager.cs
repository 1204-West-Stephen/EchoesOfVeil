using UnityEngine;

public class WalkingSoundManager : MonoBehaviour
{
    public AudioClip walkDirt;
    public AudioClip walkHallway;
    public AudioClip walkLibrary;

    public AudioClip runDirt;
    public AudioClip runHallway;
    public AudioClip runLibrary;

    public PlayerControls playerControls;
    public AudioSource audioSource;

    private SurfaceType currentSurface = SurfaceType.None;

    private bool isPlaying;

    private void Update()
    {
        if (currentSurface == SurfaceType.None) return;

        bool isSprinting = playerControls != null && /* replace with your sprint check */ false;
        bool isMoving = /* replace with your movement check */ true;

        if (!isMoving) return;

        if (!audioSource.isPlaying && !isPlaying)
        {
            PlayFootstep(isSprinting);
        }
    }

    public void SetSurface(SurfaceType surface)
    {
        currentSurface = surface;
    }

    private void PlayFootstep(bool sprinting)
    {
        AudioClip clip = null;

        switch (currentSurface)
        {
            case SurfaceType.Dirt:
                clip = sprinting ? runDirt : walkDirt;
                break;

            case SurfaceType.Hallway:
                clip = sprinting ? runHallway : walkHallway;
                break;

            case SurfaceType.Library:
                clip = sprinting ? runLibrary : walkLibrary;
                break;
        }

        if (clip == null) return;

        audioSource.PlayOneShot(clip);
        isPlaying = true;

        float delay = sprinting ? 0.3f : 0.5f;
        Invoke(nameof(ResetFootstep), delay);
    }

    private void ResetFootstep()
    {
        isPlaying = false;
    }
}
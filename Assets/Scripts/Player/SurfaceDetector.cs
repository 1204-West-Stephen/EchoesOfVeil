using UnityEngine;

public enum SurfaceType
{
    None,
    Dirt,
    Hallway,
    Library
}

public class SurfaceDetector : MonoBehaviour
{
    public WalkingSoundManager soundManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            soundManager.SetSurface(SurfaceType.Dirt);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Hallway"))
        {
            soundManager.SetSurface(SurfaceType.Hallway);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Library"))
        {
            soundManager.SetSurface(SurfaceType.Library);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        soundManager.SetSurface(SurfaceType.None);
    }
}
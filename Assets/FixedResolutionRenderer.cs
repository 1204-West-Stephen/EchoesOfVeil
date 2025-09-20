using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspect : MonoBehaviour
{
    // Desired aspect ratio (16:9 for 1920x1080)
    public float targetAspect = 16f / 9f;

    void Start()
    {
        ApplyAspect();
    }

    void Update()
    {
        // Re-apply if player resizes window
        ApplyAspect();
    }

    void ApplyAspect()
    {
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera cam = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            // Letterbox (black bars top/bottom)
            Rect rect = cam.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            cam.rect = rect;
        }
        else
        {
            // Pillarbox (black bars left/right)
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = cam.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            cam.rect = rect;
        }
    }
}

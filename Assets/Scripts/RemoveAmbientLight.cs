using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAmbientLight : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = Color.black;
        RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;
        RenderSettings.customReflectionTexture = null;
        RenderSettings.fog = false;
    }

}

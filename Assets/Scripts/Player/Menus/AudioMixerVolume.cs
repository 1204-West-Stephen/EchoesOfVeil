using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;
    [SerializeField] private string parameterName = "MusicVolume";

    void Start()
    {
        slider.onValueChanged.AddListener(SetVolume);
        if (mixer == null)
            Debug.LogError("MIXER NOT ASSIGNED!");

        if (slider == null)
            Debug.LogError("SLIDER NOT ASSIGNED!");
    }

    public void SetVolume(float value)
    {
        Debug.Log("SetVolume CALLED");
        float dB = (value <= 0.0001f)
            ? -80f
            : Mathf.Log10(value) * 20f;

        mixer.SetFloat(parameterName, dB);

        // Read back actual mixer value
        if (mixer.GetFloat(parameterName, out float currentdB))
        {
            Debug.Log($"SET dB: {dB} | MIXER dB: {currentdB}");
        }
        else
        {
            Debug.LogWarning("Failed to read mixer parameter: " + parameterName);
        }
    }
}
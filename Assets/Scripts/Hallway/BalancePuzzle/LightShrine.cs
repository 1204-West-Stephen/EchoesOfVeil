using UnityEngine;
using System.Collections;

public class LightShrine : MonoBehaviour, i_Interactable
{
    public BalancePuzzle puzzle;
    public GameObject lightBeam;
    public Light beam;

    private GameObject player;
    private Inventory inventory;

    [Header("Beam Settings")]
    public float growAmount = 0.37f;
    public float growDuration = 1f;
    public float fadeDuration = 5f;
    public float beamIntensity = 2.5f;

    private Coroutine beamCoroutine;

    private Vector3 baseScale;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();

        if (lightBeam != null)
        {
            baseScale = lightBeam.transform.localScale;
            baseScale.y = 0f;
            lightBeam.transform.localScale = baseScale;
            lightBeam.SetActive(false);
        }

        beam.intensity = 0;
    }

    public void Interact()
    {
        ItemData stoneToPlace = null;
        foreach (var item in inventory.inventory)
        {
            if (item != null && item.typeInput == InputType.NumberStone)
            {
                stoneToPlace = item;
                break;
            }
        }

        if (stoneToPlace != null)
        {
            inventory.RemoveItem(stoneToPlace);
            puzzle.ApplyStoneDecrease(stoneToPlace);
            ShineLight();
        }
        else
        {
            Debug.Log("No number stone in inventory to place.");
        }
    }

    public void ShineLight()
    {
        if (lightBeam == null) return;

        lightBeam.SetActive(true);

        // Stop any running coroutine
        if (beamCoroutine != null)
            StopCoroutine(beamCoroutine);

        // Reset scale and alpha for a fresh start
        lightBeam.transform.localScale = baseScale;

        Renderer r = lightBeam.GetComponent<Renderer>();
        if (r != null && r.material.HasProperty("_TintColor"))
        {
            Color color = r.material.GetColor("_TintColor");
            color.a = 1f; // reset alpha
            r.material.SetColor("_TintColor", color);
        }

        beamCoroutine = StartCoroutine(ShineThenFade());
    }

    private IEnumerator ShineThenFade()
    {
        Renderer r = lightBeam.GetComponent<Renderer>();
        string colorProperty = r != null && r.material.HasProperty("_Color") ? "_Color" : "_TintColor";

        // --- Scale Up ---
        Vector3 startScale = lightBeam.transform.localScale; // should be (baseScale.x, 0, baseScale.z)
        Vector3 targetScale = new Vector3(baseScale.x, baseScale.y + growAmount, baseScale.z);

        float t = 0f;
        while (t < growDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / growDuration);

            // Scale
            lightBeam.transform.localScale = Vector3.Lerp(startScale, targetScale, lerp);

            // Light intensity
            beam.intensity = Mathf.Lerp(0f, beamIntensity, lerp);

            yield return null;
        }

        // --- Fade Out ---
        Color startColor = r.material.GetColor(colorProperty);
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / fadeDuration);

            // Fade color
            r.material.SetColor(colorProperty, Color.Lerp(startColor, endColor, lerp));

            // Light intensity fade
            beam.intensity = Mathf.Lerp(beamIntensity, 0f, lerp);

            yield return null;
        }

        // Optionally disable at end
        // lightBeam.SetActive(false);
    }

}

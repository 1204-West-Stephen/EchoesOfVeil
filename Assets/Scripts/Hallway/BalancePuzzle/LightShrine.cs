using UnityEngine;
using System.Collections;

public class LightShrine : MonoBehaviour, i_Interactable
{
    public BalancePuzzle puzzle;
    public GameObject lightBeam;

    private GameObject player;
    private Inventory inventory;

    [Header("Beam Settings")]
    public float growAmount = 0.16f;
    public float growDuration = 1f;
    public float fadeDuration = 5f;

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
        if (r == null || !r.material.HasProperty("_TintColor"))
        {
            yield break;
        }

        // --- Scale Up ---
        Vector3 initialScale = lightBeam.transform.localScale;
        Vector3 targetScale = initialScale + new Vector3(0, growAmount, 0);

        float t = 0f;
        while (t < growDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / growDuration);
            lightBeam.transform.localScale = Vector3.Lerp(initialScale, targetScale, lerp);
            yield return null;
        }

        // --- Fade Out ---
        Color startColor = r.material.GetColor("_TintColor");
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / fadeDuration);
            r.material.SetColor("_TintColor", Color.Lerp(startColor, endColor, lerp));
            yield return null;
        }

        lightBeam.SetActive(false);
    }
}

using UnityEngine;
using System.Collections;

public class LightShrine : MonoBehaviour, i_Interactable
{
    public BalancePuzzle puzzle;
    public GameObject lightBeam;
    public Light beam;

    private GameObject player;
    private Inventory inventory;

    private AudioSource source;
    public AudioClip clip;

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

        source = player.GetComponent<AudioSource>();

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
        ItemData stoneToPlace = inventory.GetSelectedItem();    

        if (stoneToPlace != null && UseStone(inventory))
        {
            puzzle.ApplyStoneDecrease(stoneToPlace);
            ShineLight();
        }
        else
        {
            Debug.Log("No number stone in inventory to place.");
        }
    }

    private bool UseStone(Inventory inventory)
    {
        ItemData selectedItem = inventory.GetSelectedItem();

        if (selectedItem == null) return false;

        if (selectedItem.typeInput == GetRequiredInputType())
        {
            inventory.RemoveSelectedItem();

            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, transform.position, 0.2f);
            }
            Debug.Log("Key consumed.");
            return true;
        }

        return false;
    }

    public void ShineLight()
    {
        if (lightBeam == null) return;

        lightBeam.SetActive(true);

        if (beamCoroutine != null)
            StopCoroutine(beamCoroutine);

        lightBeam.transform.localScale = baseScale;

        Renderer r = lightBeam.GetComponent<Renderer>();
        if (r != null && r.material.HasProperty("_TintColor"))
        {
            Color color = r.material.GetColor("_TintColor");
            color.a = 1f;
            r.material.SetColor("_TintColor", color);
        }

        beamCoroutine = StartCoroutine(ShineThenFade());
    }

    private IEnumerator ShineThenFade()
    {
        Renderer r = lightBeam.GetComponent<Renderer>();
        string colorProperty = r != null && r.material.HasProperty("_Color") ? "_Color" : "_TintColor";

        Vector3 startScale = lightBeam.transform.localScale; // should be (baseScale.x, 0, baseScale.z)
        Vector3 targetScale = new Vector3(baseScale.x, baseScale.y + growAmount, baseScale.z);

        float t = 0f;
        while (t < growDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / growDuration);
            lightBeam.transform.localScale = Vector3.Lerp(startScale, targetScale, lerp);
            beam.intensity = Mathf.Lerp(0f, beamIntensity, lerp);

            yield return null;
        }

        Color startColor = r.material.GetColor(colorProperty);
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / fadeDuration);
            r.material.SetColor(colorProperty, Color.Lerp(startColor, endColor, lerp));
            beam.intensity = Mathf.Lerp(beamIntensity, 0f, lerp);

            yield return null;
        }
    }

    private InputType GetRequiredInputType()
    {
        return InputType.NumberStone;
    }

}

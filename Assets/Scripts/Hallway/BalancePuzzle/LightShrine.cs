using UnityEngine;

public class LightShrine : MonoBehaviour, i_Interactable
{
    public BalancePuzzle puzzle;
    public GameObject lightBeam;

    private GameObject player;
    private Inventory inventory;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();

        Vector3 lightScale = lightBeam.transform.localScale;
        lightScale.y = 0.0f;

        lightBeam.transform.localScale = lightScale;
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

    private void ShineLight()
    {
        if (lightBeam != null)
        {
            for (float i = 0; i < 0.16f; i += 0.01f)
            {
                Vector3 lightscale = lightBeam.transform.localScale;

                lightscale.y += i;

                lightBeam.transform.localScale = lightscale;
            }
        }
    }
}
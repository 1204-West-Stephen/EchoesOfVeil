using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public ItemData item;

    public Image iconImage;
    public TextMeshProUGUI nameText;
    public GameObject selectionHighlight; // Assign in Inspector (e.g., outline Image or glow)

    public void SetItem(ItemData newItem)
    {
        item = newItem;

        if (item != null)
        {
            iconImage.sprite = item.sprite;
            iconImage.enabled = true;
            nameText.text = item.itemName;
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
            nameText.text = "";
        }
    }

    public void SetSelected(bool isSelected)
    {
        if (selectionHighlight != null)
        {
            selectionHighlight.SetActive(isSelected);
        }
    }
}

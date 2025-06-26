using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum InputType
{
    None,
    Part,
    Scroll,
    Key,
    Book
}

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int puzzleNumber;
    public InputType typeInput;
    public Sprite sprite;
    public bool canBeInspected;
    public Canvas itemInspectUI;

    [HideInInspector] public int keyID; // Only shown if typeInput == Key
}

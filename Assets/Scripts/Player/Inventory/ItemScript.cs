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
    Book,
    Tablet,
    MetalPiece,
    TorchHilt,
    NumberStone,
    GuardianItem,
    SeasonItem
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

    [HideInInspector] public int keyID;
    [HideInInspector] public int tabletNumber;
    [HideInInspector] public GameObject prefab;
    [HideInInspector] public int stoneValue;
}

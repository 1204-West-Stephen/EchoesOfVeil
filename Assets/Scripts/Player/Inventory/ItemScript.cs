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
    SeasonItem,
    ArtPiece,
    PillarItem
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
    public Vector3 scale = Vector3.one;
    public Vector3 rotation;
    public GameObject prefab;

    [HideInInspector] public int keyID;
    [HideInInspector] public int tabletNumber;
    [HideInInspector] public int stoneValue;
}

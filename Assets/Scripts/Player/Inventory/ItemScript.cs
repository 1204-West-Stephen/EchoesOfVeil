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
    public GameObject prefab;

    [Header("Normal Scale/Rotation")]
    public Vector3 scale = Vector3.one;
    public Vector3 rotation;

    [Header("InHand Scale/Rotation")]
    public Vector3 inHandScale = Vector3.one;
    public Vector3 inHandRotation;

    [HideInInspector] public int keyID;
    [HideInInspector] public int tabletNumber;
    [HideInInspector] public int stoneValue;
}

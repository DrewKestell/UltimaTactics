using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    // Gameplay
    public ItemName Name;
    public ItemType Type;
    public EquipSlot EquipSlot;
    public bool Consumable;
    public bool Placeable;
    public bool Stackable;
    public bool Usable;

    // Visuals
    public Texture2D InventorySprite;
    public Mesh Mesh;
    public Material Material;
}

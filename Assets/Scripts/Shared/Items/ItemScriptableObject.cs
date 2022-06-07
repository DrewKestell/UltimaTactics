using UnityEngine;

public abstract class ItemScriptableObject : ScriptableObject
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
    public Sprite InventorySprite;
    public Mesh Mesh;
    public Material Material;
}

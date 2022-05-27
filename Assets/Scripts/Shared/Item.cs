using Unity.Netcode;

public struct Item : INetworkSerializable
{
    public string Name;
    public int InventorySpriteId;
    public int EquipMeshId;
    public int EquipMaterialId;
    public EquipSlot EquipSlot;
    public float Weight;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Name);
        serializer.SerializeValue(ref InventorySpriteId);
        serializer.SerializeValue(ref EquipMeshId);
        serializer.SerializeValue(ref EquipMaterialId);
        serializer.SerializeValue(ref EquipSlot);
        serializer.SerializeValue(ref Weight);
    }
}

public enum EquipSlot
{
    None,
    Chest,
    Legs,
    Feet,
    Head
}
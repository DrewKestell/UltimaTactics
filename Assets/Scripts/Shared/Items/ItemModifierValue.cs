using Unity.Netcode;

public struct ItemModifierValue : INetworkSerializable
{
    public ItemModifier Modifier;
    public float Value;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Modifier);
        serializer.SerializeValue(ref Value);
    }

    public override string ToString()
    {
        return $"Modifier={{ {Modifier}, {Value} }}";
    }
}

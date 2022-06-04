using System;
using Unity.Netcode;

public struct ItemModifierValue : INetworkSerializable, IEquatable<ItemModifierValue>
{
    public ItemModifier Modifier;
    public float Value;

    public bool Equals(ItemModifierValue other)
    {
        return Modifier == other.Modifier && Value == other.Value;
    }

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

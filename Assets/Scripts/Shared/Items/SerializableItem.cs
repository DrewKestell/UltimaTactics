using Unity.Collections;
using Unity.Netcode;

public struct SerializableItem : INetworkSerializable
{
    public ItemName Name;
    public FixedList128Bytes<ItemModifierValue> Modifiers;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Name);

        // Modifiers
        var length = 0;
        if (!serializer.IsReader)
        {
            length = Modifiers.Length;
        }
        serializer.SerializeValue(ref length);

        if (serializer.IsReader)
        {
            Modifiers = new FixedList128Bytes<ItemModifierValue>();
        }
        for (int n = 0; n < length; ++n)
        {
            if (serializer.IsReader)
            {
                var mod = new ItemModifierValue();
                serializer.SerializeValue(ref mod);
                Modifiers.Add(mod);
            }
            else
            {
                serializer.SerializeValue(ref Modifiers.ElementAt(n));
            }
        }
    }
}

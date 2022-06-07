using Newtonsoft.Json;
using System;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;

public struct SerializableItem : INetworkSerializable, IEquatable<SerializableItem>
{
    public ItemName Name;
    [JsonConverter(typeof(FixedList128BytesConverter))]
    public FixedList128Bytes<ItemModifierValue> Modifiers;

    public bool Equals(SerializableItem other)
    {
        return 
            Name == other.Name &&
            Modifiers.Length == other.Modifiers.Length &&
            Modifiers.All(m => other.Modifiers.Contains(m));
    }

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

public class FixedList128BytesConverter : JsonConverter<FixedList128Bytes<ItemModifierValue>>
{
    public override void WriteJson(JsonWriter writer, FixedList128Bytes<ItemModifierValue> value, JsonSerializer serializer)
    {
        var arr = value.ToArray();
        writer.WriteValue(JsonConvert.SerializeObject(arr));
    }

    public override FixedList128Bytes<ItemModifierValue> ReadJson(JsonReader reader, Type objectType, FixedList128Bytes<ItemModifierValue> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (hasExistingValue)
        {
            return existingValue;
        }

        var val = reader.ReadAsString();
        var arr = JsonConvert.DeserializeObject<ItemModifierValue[]>(val);
        var fixedList = new FixedList128Bytes<ItemModifierValue>();
        
        foreach (var i in arr)
        {
            fixedList.Add(i);
        }

        return fixedList;
    }
}
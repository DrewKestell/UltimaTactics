using Unity.Netcode;

public partial class Inventory : WorldSaved
{
    public PrimitiveKeyedNetworkDictionary<int, SerializableItem> Items;

    private void Awake()
    {
        Items = new();
    }
}

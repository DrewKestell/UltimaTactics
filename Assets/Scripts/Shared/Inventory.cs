using Unity.Netcode;

public partial class Inventory : NetworkBehaviour
{
    public PrimitiveKeyedNetworkDictionary<int, SerializableItem> Items;

    private void Awake()
    {
        Items = new();
    }
}

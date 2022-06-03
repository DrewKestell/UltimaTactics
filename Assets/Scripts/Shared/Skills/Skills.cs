using Unity.Netcode;

public partial class Skills : WorldSaved
{
    public PrimitiveKeyedNetworkDictionary<int, float> Values;

    public void Awake()
    {
        Values = new();
    }
}

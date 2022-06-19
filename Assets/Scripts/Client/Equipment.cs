#if CLIENT_BUILD || UNITY_EDITOR
using Unity.Netcode;

public partial class Equipment : WorldSaved
{
#if CLIENT_BUILD
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // TODO: initialize items
    }
#endif
}
#endif

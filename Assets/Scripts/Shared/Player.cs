using Unity.Netcode;

public partial class Player : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkSpawn();
    }    
}

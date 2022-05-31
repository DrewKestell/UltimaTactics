#if SERVER_BUILD || UNITY_EDITOR
using Unity.Netcode;

public partial class Player : NetworkBehaviour
{
#if SERVER_BUILD
    public void NetworkSpawn()
    {

    }
#endif
}
#endif

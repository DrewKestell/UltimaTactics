#if CLIENT_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine.InputSystem;

public partial class Player : NetworkBehaviour
{
#if CLIENT_BUILD
    public void NetworkSpawn()
    {
        if (!IsLocalPlayer)
        {
            // Player objects get spawned on all clients, but each client should only control their own Local Player object,
            // so we disable the PlayerInput component on all non-local players.
            gameObject.GetComponent<PlayerInput>().enabled = false;
        }
    }
#endif
}
#endif

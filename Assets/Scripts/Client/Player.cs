#if CLIENT_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : NetworkBehaviour
{
#if CLIENT_BUILD
    public void NetworkSpawn()
    {
        if (!IsLocalPlayer)
        {
            Destroy(gameObject.GetComponent<CharacterController>());
            Destroy(gameObject.GetComponent<PlayerController>());
            Destroy(gameObject.GetComponent<PlayerInput>());
        }
    }
#endif
}
#endif

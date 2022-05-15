using Unity.Netcode;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
#if SERVER_BUILD
    private void Start()
    {
        NetworkManager.Singleton.StartServer();
    }
#endif
}

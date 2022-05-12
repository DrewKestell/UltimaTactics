#if SERVER_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.Singleton.StartClient();
    }
}
#endif
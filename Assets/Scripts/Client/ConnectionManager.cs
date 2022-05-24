#if CLIENT_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;

public partial class ConnectionManager : NetworkBehaviour
{
#if CLIENT_BUILD
    [SerializeField] private UIMediator uiMediator;

    public void ConnectClient(string email, string password)
    {
        var payload = JsonUtility.ToJson(new ConnectionPayload
        {
            Email = email,
            Password = password
        });
        var payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
        NetworkManager.Singleton.StartClient();
    }
#endif
}
#endif

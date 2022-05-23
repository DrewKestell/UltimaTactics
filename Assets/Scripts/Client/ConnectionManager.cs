using Unity.Netcode;
using UnityEngine;
#if CLIENT_BUILD || UNITY_EDITOR

public partial class ConnectionManager : MonoBehaviour
{
#if CLIENT_BUILD

    public void ConnectClient()
    {
        Debug.Log("hello");
        var payload = JsonUtility.ToJson(new ConnectionPayload
        {
            Email = "TODO",
            Password = "TODO"
        });
        var payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
        NetworkManager.Singleton.StartClient();
    }
#endif
}
#endif

#if CLIENT_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class ConnectionManager : NetworkBehaviour
{
#if CLIENT_BUILD
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

        //NetworkManager.SceneManager.LoadScene("Login", LoadSceneMode.Additive);
    }
#endif
}
#endif

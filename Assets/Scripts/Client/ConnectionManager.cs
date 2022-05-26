#if CLIENT_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class ConnectionManager : NetworkBehaviour
{
#if CLIENT_BUILD
    private void OnAwake()
    {
        PubSub.Instance.Subscribe<EnterWorldSuccessfulEvent>(this, EnterWorldSuccessful);
    }

    private void Enable()
    {
    }

    private void OnStart()
    {
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        //PubSub.Instance.Unsubscribe<EnterWorldSuccessfulEvent>(this);
    }

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

    public void EnterWorldSuccessful(EnterWorldSuccessfulEvent e)
    {
        SceneManager.LoadScene("World", LoadSceneMode.Single);

        Instance.RequestCharacterAssetsServerRpc(e.CharacterId);
    }
#endif
}
#endif

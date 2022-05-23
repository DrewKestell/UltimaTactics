#if SERVER_BUILD || UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;

public partial class ConnectionManager : MonoBehaviour
{
#if SERVER_BUILD
    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += ServerStartedHandler;

        NetworkManager.Singleton.StartServer();
    }

    void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate connectionApprovedCallback)
    {
        Debug.Log("Approval check");

        var payload = System.Text.Encoding.UTF8.GetString(connectionData);
        var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload); // https://docs.unity3d.com/2020.2/Documentation/Manual/JSONSerialization.html

        var authResult = AccountManager.Instance.Authenticate(connectionPayload.Email, connectionPayload.Password);

        Debug.Log($"Email: {connectionPayload.Email} Password: {connectionPayload.Password} AuthResult: {authResult}");

        connectionApprovedCallback(
            createPlayerObject: false,
            playerPrefabHash: null,
            approved: authResult,
            position: null,
            rotation: null);
    }

    void ServerStartedHandler()
    {
        Debug.Log("Server started successfully.");
    }
#endif
}
#endif
#if SERVER_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class ConnectionManager : NetworkBehaviour
{
#if SERVER_BUILD
    private readonly Dictionary<ulong, int> clientAccountMap = new();

    private void Start()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += ServerStartedHandler;

        NetworkManager.Singleton.StartServer();

        //NetworkManager.Singleton.SceneManager.VerifySceneBeforeLoading += VerifySceneBeforeLoadingHandler;
        //NetworkManager.Singleton.SceneManager.LoadScene("World", LoadSceneMode.Single);
        //SceneManager.LoadScene("World", LoadSceneMode.Additive);
    }

    void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate connectionApprovedCallback)
    {
        var payload = System.Text.Encoding.UTF8.GetString(connectionData);
        var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload); // https://docs.unity3d.com/2020.2/Documentation/Manual/JSONSerialization.html
        (bool authResult, int? accountId) = AccountManager.Instance.Authenticate(connectionPayload.Email, connectionPayload.Password);

        if (authResult)
        {
            connectionApprovedCallback(
                createPlayerObject: false,
                playerPrefabHash: null,
                approved: authResult,
                position: null,
                rotation: null);

            clientAccountMap.Add(clientId, accountId.Value);

            var characters = SqlRepository.Instance.ListAccountCharacters(connectionPayload.Email);
            var characterListItems = characters.Select(c => new CharacterListItem { Id = c.Id, Name = c.Name }).ToArray();
            LoadCharacterSelectClientRpc(characterListItems, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { clientId } } });
        }
        else
        {
            // TODO
        }
    }

    void ServerStartedHandler()
    {
        Debug.Log("Server started successfully.");
    }

    bool VerifySceneBeforeLoadingHandler(int sceneIndex, string sceneName, LoadSceneMode loadSceneMode)
    {
        Debug.Log($"VerifySceneBeforeLoadingHandler: SceneIndex={sceneIndex} SceneName={sceneName} LoadSceneMode={loadSceneMode}");

        return true;
        //return loadSceneMode == LoadSceneMode.Additive;
    }
#endif
}
#endif
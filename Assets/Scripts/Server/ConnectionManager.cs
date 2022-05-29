#if SERVER_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class ConnectionManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject inventoryPrefab;
    [SerializeField] private GameObject testPrefab;

#if SERVER_BUILD
    private readonly Dictionary<ulong, int> clientAccountMap = new(); // TODO: put this on Play prefab?
    private readonly Dictionary<ulong, bool> clientInGameMap = new(); // TODO: put this on Play prefab?

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        StartButtons();

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("ShowParent"))
        {
            var clientId = clientPlayerMap.First().Key;
            testObj1.GetComponent<NetworkObject>().NetworkShow(clientId);
        }
        if (GUILayout.Button("ShowChild"))
        {
            var clientId = clientPlayerMap.First().Key;
            testObj2.GetComponent<NetworkObject>().NetworkShow(clientId);
        }
        if (GUILayout.Button("HideParent"))
        {
            var clientId = clientPlayerMap.First().Key;
            testObj1.GetComponent<NetworkObject>().NetworkHide(clientId);
        }
        if (GUILayout.Button("HideChild"))
        {
            var clientId = clientPlayerMap.First().Key;
            testObj2.GetComponent<NetworkObject>().NetworkHide(clientId);
        }
    }

    private void OnAwake()
    {
    }

    private void OnStart()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += ServerStartedHandler;

        NetworkManager.Singleton.StartServer();

        SceneManager.LoadScene("World", LoadSceneMode.Single);
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
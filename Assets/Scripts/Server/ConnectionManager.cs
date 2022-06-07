#if SERVER_BUILD || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class ConnectionManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject inventoryPrefab;
    [SerializeField] private GameObject skillsPrefab;

    [SerializeField] private GameObject testNetworkObject;

#if SERVER_BUILD
    private readonly Dictionary<ulong, int> clientAccountMap = new();
    private readonly Dictionary<ulong, bool> clientInGameMap = new();

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        StartButtons();

        GUILayout.EndArea();
    }

    void StartButtons()
    {
        if (GUILayout.Button("Test"))
        {
            var obj = Instantiate(inventoryPrefab);
            obj.GetComponent<Inventory>().Items.Add(obj.GetInstanceID(), new SerializableItem
            {
                Name = ItemName.Belt,
                Modifiers = new FixedList128Bytes<ItemModifierValue>
                {
                    new ItemModifierValue { Modifier = ItemModifier.LowerReagentCost, Value = 5 },
                    new ItemModifierValue { Modifier = ItemModifier.HitPointIncrease, Value = 10 }
                }
            });
            var networkObj = obj.GetComponent<NetworkObject>();
            networkObj.Spawn();
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
        var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);
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
        // TODO: initialize game state
        Debug.Log("Server started successfully.");
    }

    bool VerifySceneBeforeLoadingHandler(int sceneIndex, string sceneName, LoadSceneMode loadSceneMode)
    {
        Debug.Log($"VerifySceneBeforeLoadingHandler: SceneIndex={sceneIndex} SceneName={sceneName} LoadSceneMode={loadSceneMode}");

        return true;
    }
#endif
}
#endif
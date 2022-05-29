using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public partial class ConnectionManager : NetworkBehaviour
{
    public static ConnectionManager Instance;

    private static Dictionary<ulong, GameObject> clientPlayerMap = new(); // TODO: don't need this, you can use NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject

    private List<NetworkObject> networkObjectToShowOnConnect = new();

    private static GameObject testObj1;
    private static GameObject testObj2;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestCharacterCreationAssetsServerRpc(ServerRpcParams serverRpcParams = default)
    {
#if SERVER_BUILD
        Debug.Log($"{nameof(RequestCharacterCreationAssetsServerRpc)} invoked. SenderClientId: {serverRpcParams.Receive.SenderClientId}");
        // TODO: any assets that come from the server should be sent back to the client here
        ReturnCharacterCreationAssetsClientRpc(ReturnToSameClientParams(serverRpcParams));
#endif
    }

    [ServerRpc(RequireOwnership = false)]
    public void CreateCharacterServerRpc(string name, int skillId1, int skillId2, int skillId3, ServerRpcParams serverRpcParams = default)
    {
#if SERVER_BUILD
        Debug.Log($"{nameof(CreateCharacterServerRpc)} invoked. SenderClientId: {serverRpcParams.Receive.SenderClientId}");

        // TODO: validate uniqueness of character name

        var accountId = clientAccountMap[serverRpcParams.Receive.SenderClientId];
        var characterListItem = CharacterGenerator.CreateCharacter(accountId, name, skillId1, skillId2, skillId3);

        CharacterCreationSuccessfulClientRpc(characterListItem, ReturnToSameClientParams(serverRpcParams));
#endif
    }

    [ServerRpc(RequireOwnership = false)]
    public void EnterWorldServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {
#if SERVER_BUILD
        Debug.Log($"{nameof(EnterWorldServerRpc)} invoked. SenderClientId: {serverRpcParams.Receive.SenderClientId}");

        // TODO: validate ownership of character to this client accountId
        var character = SqlRepository.Instance.GetCharacter(clientAccountMap[serverRpcParams.Receive.SenderClientId], characterId);
        if (character != null)
        {
            EnterWorldSuccessfulClientRpc(characterId, ReturnToSameClientParams(serverRpcParams));
        }
#endif
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestCharacterAssetsServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {
#if SERVER_BUILD
        Debug.Log($"{nameof(RequestCharacterAssetsServerRpc)} invoked. SenderClientId: {serverRpcParams.Receive.SenderClientId}");

        var clientId = serverRpcParams.Receive.SenderClientId;
        clientInGameMap.Add(clientId, true);
        // TODO: validate ownership of character to this client accountId
        var instance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        instance.name = $"Player_{clientId}";
        clientPlayerMap.Add(clientId, instance);
        //instance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId); TODO: uncomment me to revert test
        instance.GetComponent<NetworkObject>().CheckObjectVisibility = (cid) => {
            return cid == clientId;
        };
        instance.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);

        var character = SqlRepository.Instance.GetCharacter(clientAccountMap[clientId], characterId);
        var characterSkills = SqlRepository.Instance.GetCharacterSkills(characterId);

        var inventoryInstance = Instantiate(inventoryPrefab);
        inventoryInstance.GetComponent<NetworkObject>().CheckObjectVisibility = (cid) => {
            return cid == clientId;
        };
        inventoryInstance.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);

        var characterAssets = new CharacterAssets
        {
            Name = character.Name,
            SkillIds = characterSkills.Skills.Keys.ToArray(),
            SkillValues = characterSkills.Skills.Values.ToArray()
        };

        // TEST

        var test = Instantiate(testPrefab);
        var testNetworkObject = test.GetComponent<NetworkObject>();
        testNetworkObject.CheckObjectVisibility = (cid) => {
            return clientInGameMap.ContainsKey(cid) && clientInGameMap[cid]; // show to any clients in game
        };
        test.GetComponent<NetworkObject>().Spawn();
        testObj1 = test;

        //var test2 = Instantiate(testPrefab2);
        //test2.GetComponent<NetworkObject>().CheckObjectVisibility = (cid) =>
        //{
        //    return clientInGameMap.ContainsKey(cid) && clientInGameMap[cid]; // show to any clients in game
        //};
        //test2.GetComponent<NetworkObject>().Spawn();
        //test2.transform.SetParent(test.transform);
        //testObj2 = test2;

        //foreach (var obj in networkObjectToShowOnConnect)
        //{
        //    obj.NetworkShow(clientId);
        //}

        //networkObjectToShowOnConnect.Add(testNetworkObject);

        // END TEST

        RequestCharacterAssetsSuccessfulClientRpc(characterAssets);
#endif
    }

    [ClientRpc]
    public void LoadCharacterSelectClientRpc(CharacterListItem[] characters, ClientRpcParams clientRpcParams = default)
    {
        var e = new LoginSuccessfulEvent(characters);

        PubSub.Instance.Publish(this, e);
    }

    [ClientRpc]
    public void ReturnCharacterCreationAssetsClientRpc(ClientRpcParams clientRpcParams = default)
    {
        var e = new ReceivedCharacterCreationAssetsEvent();

        PubSub.Instance.Publish(this, e);
    }

    [ClientRpc]
    public void CharacterCreationSuccessfulClientRpc(CharacterListItem character, ClientRpcParams clientRpcParams = default)
    {
        var e = new CharacterCreationSuccessfulEvent(character);

        PubSub.Instance.Publish(this, e);
    }

    [ClientRpc]
    public void EnterWorldSuccessfulClientRpc(int characterId, ClientRpcParams clientRpcParams = default)
    {
        var e = new EnterWorldSuccessfulEvent(characterId);

        PubSub.Instance.Publish(this, e);
    }

    [ClientRpc]
    public void RequestCharacterAssetsSuccessfulClientRpc(CharacterAssets characterAssets, ClientRpcParams clientRpcParams = default)
    {
        var e = new RequestCharacterAssetsSuccessfulEvent(characterAssets);

        PubSub.Instance.Publish(this, e);
    }

    private ClientRpcParams ReturnToSameClientParams(ServerRpcParams serverRpcParams)
    {
        return new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]
                {
                    serverRpcParams.Receive.SenderClientId
                }
            }
        };
    }
}

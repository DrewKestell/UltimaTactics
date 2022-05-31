using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public partial class ConnectionManager : NetworkBehaviour
{
    public static ConnectionManager Instance;

    private List<NetworkObject> networkObjectToShowOnConnect = new();

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
    public void CreateCharacterServerRpc(string name, SkillName skillName1, SkillName skillName2, SkillName skillName3, ServerRpcParams serverRpcParams = default)
    {
#if SERVER_BUILD
        Debug.Log($"{nameof(CreateCharacterServerRpc)} invoked. SenderClientId: {serverRpcParams.Receive.SenderClientId}");

        // TODO: validate uniqueness of character name

        var accountId = clientAccountMap[serverRpcParams.Receive.SenderClientId];
        var characterListItem = CharacterGenerator.CreateCharacter(accountId, name, skillName1, skillName2, skillName3);

        CharacterCreationSuccessfulClientRpc(characterListItem, ReturnToSameClientParams(serverRpcParams));
#endif
    }

    [ServerRpc(RequireOwnership = false)]
    public void EnterWorldServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {
#if SERVER_BUILD
        Debug.Log($"{nameof(EnterWorldServerRpc)} invoked. SenderClientId: {serverRpcParams.Receive.SenderClientId}");

        // TODO: validate ownership of character to this client accountId
        var character = SqlRepository.Instance.GetCharacter(characterId);
        if (character != null)
        {
            clientInGameMap.Add(serverRpcParams.Receive.SenderClientId, true);
            EnterWorldSuccessfulClientRpc(characterId, ReturnToSameClientParams(serverRpcParams));
        }
#endif
    }

    [ServerRpc(RequireOwnership = false)]
    public void CreatePlayerServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {
#if SERVER_BUILD
        Debug.Log($"{nameof(CreatePlayerServerRpc)} invoked. SenderClientId: {serverRpcParams.Receive.SenderClientId}");

        var clientId = serverRpcParams.Receive.SenderClientId;
        var accountId = clientAccountMap[clientId];

        // TODO: validate ownership of character to this client accountId
        var character = SqlRepository.Instance.GetCharacter(characterId);

        // Instantiate Player object
        var playerObject = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        playerObject.GetComponent<PlayerState>().CharacterId.Value = characterId;
        playerObject.name = $"Player_{clientId}";
        var playerNetworkObject = playerObject.GetComponent<NetworkObject>();
        playerNetworkObject.CheckObjectVisibility = (cid) => cid == clientId;
        playerNetworkObject.SpawnAsPlayerObject(clientId);
        networkObjectToShowOnConnect.Add(playerNetworkObject);

        // Initialize PlayerState component
        var playerStateComponent = playerObject.GetComponent<PlayerState>();
        playerStateComponent.AccountId.Value = accountId;
        playerStateComponent.CharacterId.Value = characterId;
        playerStateComponent.Name.Value = character.Name;

        // Instantiate Skills object
        var skillsObject = Instantiate(skillsPrefab);
        var skillsComponent = skillsObject.GetComponent<Skills>();
        skillsComponent.Deserialize(characterId);
        var skillsNetworkObject = skillsObject.GetComponent<NetworkObject>();
        skillsNetworkObject.CheckObjectVisibility = (cid) => cid == clientId;
        skillsNetworkObject.SpawnWithOwnership(clientId);
        skillsNetworkObject.TrySetParent(playerObject);

        // TODO: Instantiate Inventory object

        foreach (var obj in networkObjectToShowOnConnect)
        {
            foreach (var p in clientInGameMap)
            {
                if (p.Value && !obj.IsNetworkVisibleTo(p.Key))
                    obj.NetworkShow(p.Key);
            }
        }

        CreatePlayerSuccessfulClientRpc();
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
    public void CreatePlayerSuccessfulClientRpc(ClientRpcParams clientRpcParams = default)
    {
        var e = new CreatePlayerSuccessfulEvent();

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

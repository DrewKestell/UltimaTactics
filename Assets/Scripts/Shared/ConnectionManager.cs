using System.Linq;
using Unity.Netcode;
using UnityEngine;

public partial class ConnectionManager : NetworkBehaviour
{
    public static ConnectionManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestCharacterCreationAssetsServerRpc(ServerRpcParams serverRpcParams = default)
    {
#if SERVER_BUILD
        Debug.Log($"{nameof(RequestCharacterCreationAssetsServerRpc)} invoked. SenderClientId: {serverRpcParams.Receive.SenderClientId}");
        var skills = SqlRepository.Instance.ListSkills();
        var skillListItems = skills.Select(s => new SkillListItem { Id = s.Id, Name = s.Name }).ToArray();

        ReturnCharacterCreationAssetsClientRpc(skillListItems, ReturnToSameClientParams(serverRpcParams));
#endif
    }

    [ServerRpc(RequireOwnership = false)]
    public void CreateCharacterServerRpc(string name, int skillId1, int skillId2, int skillId3, ServerRpcParams serverRpcParams = default)
    {
#if SERVER_BUILD
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
        // TODO: validate ownership of character to this client accountId
        var character = SqlRepository.Instance.GetCharacter(clientAccountMap[serverRpcParams.Receive.SenderClientId], characterId);
        if (character != null)
        {
            var instance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            instance.GetComponent<NetworkObject>().SpawnAsPlayerObject(serverRpcParams.Receive.SenderClientId);
            EnterWorldSuccessfulClientRpc(ReturnToSameClientParams(serverRpcParams));
        }
#endif
    }

    [ClientRpc]
    public void LoadCharacterSelectClientRpc(CharacterListItem[] characters, ClientRpcParams clientRpcParams = default)
    {
        var e = new LoginSuccessfulEvent(characters);

        PubSub.Instance.Publish(this, e);
    }

    [ClientRpc]
    public void ReturnCharacterCreationAssetsClientRpc(SkillListItem[] skills, ClientRpcParams clientRpcParams = default)
    {
        var e = new ReceivedCharacterCreationAssetsEvent(skills);

        PubSub.Instance.Publish(this, e);
    }

    [ClientRpc]
    public void CharacterCreationSuccessfulClientRpc(CharacterListItem character, ClientRpcParams clientRpcParams = default)
    {
        var e = new CharacterCreationSuccessfulEvent(character);

        PubSub.Instance.Publish(this, e);
    }

    [ClientRpc]
    public void EnterWorldSuccessfulClientRpc(ClientRpcParams clientRpcParams = default)
    {
        var e = new EnterWorldSuccessfulEvent();

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

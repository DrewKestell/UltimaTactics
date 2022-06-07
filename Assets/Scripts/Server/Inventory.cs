#if SERVER_BUILD || UNITY_EDITOR
using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.Netcode;

public partial class Inventory : WorldSaved
{
#if SERVER_BUILD
    private int characterId = -1;

    public override void Serialize()
    {
        if (characterId == -1)
        {
            characterId = GetComponentInParent<PlayerState>().CharacterId.Value;
        }

        // Unity JsonUtility does not support dictionaries, so use Newtonsoft for now
        var itemsJson = JsonConvert.SerializeObject(Items.ToDictionary());
        SqlRepository.Instance.UpdateCharacterItems(itemsJson, characterId);
    }

    // TODO: figure out a better way to get CharacterID from the GameObject hierarchy - it's tricky
    // because we need to instantiate, reparent, then spawn, so it moves in the hierarchy and causes problems on the client.
    public override void Deserialize(int characterId)
    {
        // Unity JsonUtility does not support dictionaries, so use Newtonsoft for now
        var itemsJson = SqlRepository.Instance.GetCharacterItems(characterId);
        var dict = JsonConvert.DeserializeObject<Dictionary<int, SerializableItem>>(itemsJson);

        Items.Dispose();
        Items = new PrimitiveKeyedNetworkDictionary<int, SerializableItem>(dict);
    }
#endif
}
#endif

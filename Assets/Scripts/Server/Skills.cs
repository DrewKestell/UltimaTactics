#if SERVER_BUILD || UNITY_EDITOR
using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.Netcode;

public partial class Skills : WorldSaved
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
        var skillsJson = JsonConvert.SerializeObject(Values.ToDictionary());
        SqlRepository.Instance.UpdateCharacterSkills(skillsJson, characterId);
    }

    // TODO: figure out a better way to get CharacterID from the GameObject hierarchy - it's tricky
    // because we need to instantiate, reparent, then spawn, so it moves in the hierarchy and causes problems on the client.
    public override void Deserialize(int characterId)
    {
        // Unity JsonUtility does not support dictionaries, so use Newtonsoft for now
        var skillsJson = SqlRepository.Instance.GetCharacterSkills(characterId);
        var dict = JsonConvert.DeserializeObject<Dictionary<int, float>>(skillsJson);

        Values.Dispose();
        Values = new PrimitiveKeyedNetworkDictionary<int, float>(dict);
    }
#endif
}
#endif

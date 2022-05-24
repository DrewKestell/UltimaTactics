using Newtonsoft.Json;
using System.Collections.Generic;

public class CharacterSkills
{
    public CharacterSkills(int id, IDictionary<int, float> skills, int characterId)
    {
        Id = id;
        CharacterId = characterId;
        Skills = skills;
    }

    public CharacterSkills(int id, string skills, int characterId)
    {
        Id = id;
        Skills = JsonConvert.DeserializeObject<IDictionary<int, float>>(skills);
        CharacterId = characterId;
    }

    public int Id { get; }

    public IDictionary<int, float> Skills { get; }

    public int CharacterId { get; }
}

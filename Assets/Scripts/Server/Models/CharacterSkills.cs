using System.Collections.Generic;

public class CharacterSkills
{
    public CharacterSkills(int id, string skills, int characterId)
    {
        Id = id;
        CharacterId = characterId;

        // Skills = TODO
    }

    public int Id { get; }

    public IDictionary<string, float> Skills { get; }

    public int CharacterId { get; }
}

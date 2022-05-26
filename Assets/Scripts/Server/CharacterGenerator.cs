#if SERVER_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

public static class CharacterGenerator
{
#if SERVER_BUILD
    public static CharacterListItem CreateCharacter(int accountId, string name, int skillId1, int skillId2, int skillId3)
    {
        var character = new Character(0, name, accountId);
        var characterId = SqlRepository.Instance.InsertCharacter(character);
        var allSkills = SkillManager.Instance.AllSkills;

        var values = new Dictionary<int, float>();
        foreach (var skill in allSkills)
        {
            float value;
            if (skill.Key == skillId1 || skill.Key == skillId2 || skill.Key == skillId3)
            {
                value = 35.0f;
            }
            else
            {
                value = (float)System.Math.Round((float)Random.Range(0, 25.0f), 1);
            }
            values.Add(skill.Key, value);
        }
        var characterSkills = new CharacterSkills(0, values, characterId);
        SqlRepository.Instance.InsertCharacterSkills(characterSkills);

        return new CharacterListItem { Id = characterId, Name = name };
    }
#endif
}
#endif

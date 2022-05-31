#if SERVER_BUILD || UNITY_EDITOR
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterGenerator
{
#if SERVER_BUILD
    public static CharacterListItem CreateCharacter(int accountId, string name, SkillName skillName1, SkillName skillName2, SkillName skillName3)
    {
        var character = new Character(0, name, accountId);
        var characterId = SqlRepository.Instance.InsertCharacter(character);
        var allSkills = SkillsManager.Instance.AllSkills;

        var dict = new Dictionary<int, float>();
        foreach (var skill in allSkills)
        {
            var skillName = skill.Key;
            float value;
            if (skillName == skillName1 || skillName == skillName2 || skillName == skillName3)
            {
                value = 35.0f;
            }
            else
            {
                value = (float)System.Math.Round((float)Random.Range(0, 25.0f), 1);
            }
            dict.Add((int)skillName, value);
        }

        // Unity JsonUtility does not support arrays, so use Newtonsoft for now
        var skillsJson = JsonConvert.SerializeObject(dict);
        SqlRepository.Instance.InsertCharacterSkills(skillsJson, characterId);

        return new CharacterListItem { Id = characterId, Name = name };
    }
#endif
}
#endif

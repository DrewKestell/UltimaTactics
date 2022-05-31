using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillScriptableObject", order = 2)]
public class SkillScriptableObject : ScriptableObject
{
    public SkillName Name;
    public bool Activatable;
}

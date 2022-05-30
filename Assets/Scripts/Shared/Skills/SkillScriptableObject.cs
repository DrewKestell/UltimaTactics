using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillScriptableObject", order = 2)]
public class SkillScriptableObject : ScriptableObject
{
    public string Name;
    public bool Activatable;

    public override string ToString()
    {
        return $"Name={Name} Activatable={Activatable}";
    }
}

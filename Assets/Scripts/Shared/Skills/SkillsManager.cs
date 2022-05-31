using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    [SerializeField] private SkillScriptableObject[] Skills;

    public Dictionary<SkillName, SkillScriptableObject> AllSkills;

    public static SkillsManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        AllSkills = Skills.ToDictionary(s => s.Name, s => s);
    }
}

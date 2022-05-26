using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private Skill[] skills;

    public static SkillManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        AllSkills = skills.ToDictionary(s => s.Id, s => s);
    }

    public Dictionary<int, Skill> AllSkills { get; private set; }
}

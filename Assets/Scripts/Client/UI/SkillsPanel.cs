#if CLIENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TMPro;
using UnityEngine;

public class SkillsPanel : MonoBehaviour
{
    [SerializeField] private RectTransform scrollableContent;
    [SerializeField] private GameObject skillListItemPrefab;

    private readonly Dictionary<int, GameObject> skillListItemMap = new();

#if CLIENT_BUILD
    private void Awake()
    {
        gameObject.transform.parent.gameObject.SetActive(false);

        PubSub.Instance.Subscribe<InitializeSkillsPanelEvent>(this, InitializeSkillsPanel);
        PubSub.Instance.Subscribe<SkillChangedEvent>(this, HandleSkillChange);
    }

    public void InitializeSkillsPanel(InitializeSkillsPanelEvent e)
    {
        foreach (var skill in e.Skills)
        {
            var instance = Instantiate(skillListItemPrefab, scrollableContent);
            skillListItemMap.Add(skill.Key, instance);
            instance.transform.GetChild(0).GetComponent<TMP_Text>().text = ((SkillName)skill.Key).GetAttribute<DisplayAttribute>().Name;
            instance.transform.GetChild(1).GetComponent<TMP_Text>().text = skill.Value.ToString("0.0");
        }

        scrollableContent.sizeDelta = new Vector2(240, 28 * e.Skills.Count);
    }

    public void HandleSkillChange(SkillChangedEvent e)
    {
        skillListItemMap[(int)e.SkillName].transform.GetChild(1).GetComponent<TMP_Text>().text = e.NewValue.ToString("0.0");
    }
#endif
}
#endif

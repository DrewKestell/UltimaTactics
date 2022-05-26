#if CLIENT_BUILD || UNITY_EDITOR
using TMPro;
using UnityEngine;

public class SkillsPanel : MonoBehaviour
{
    [SerializeField] private RectTransform scrollableContent;
    [SerializeField] private GameObject skillListItemPrefab;

#if CLIENT_BUILD
    private void Awake()
    {
        PubSub.Instance.Subscribe<RequestCharacterAssetsSuccessfulEvent>(this, InitializeSkillsPanel);
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void InitializeSkillsPanel(RequestCharacterAssetsSuccessfulEvent e)
    {
        var allSkills = SkillManager.Instance.AllSkills;

        foreach (var skill in e.CharacterAssets.Skills)
        {
            var instance = Instantiate(skillListItemPrefab, scrollableContent);
            instance.transform.GetChild(0).GetComponent<TMP_Text>().text = allSkills[skill.Key].Name;
            instance.transform.GetChild(1).GetComponent<TMP_Text>().text = skill.Value.ToString("0.0");
        }

        scrollableContent.sizeDelta = new Vector2(240, 28 * allSkills.Count);
    }
#endif
}
#endif

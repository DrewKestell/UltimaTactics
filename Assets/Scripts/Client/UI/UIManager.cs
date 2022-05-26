#if CLIENT_BUILD || UNITY_EDITOR
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private RectTransform characterPanel;
    private RectTransform inventoryPanel;
    private RectTransform skillsPanel;
    private RectTransform settingsPanel;

#if CLIENT_BUILD
    private void Awake()
    {
        characterPanel = transform.GetChild(1).GetComponent<RectTransform>();
        inventoryPanel = transform.GetChild(2).GetComponent<RectTransform>();
        skillsPanel = transform.GetChild(3).GetComponent<RectTransform>();
        settingsPanel = transform.GetChild(4).GetComponent<RectTransform>();
    }

    public void CharacterButtonOnClick()
    {
        characterPanel.gameObject.SetActive(!characterPanel.gameObject.activeInHierarchy);
        characterPanel.transform.SetAsLastSibling();
    }

    public void InventoryButtonOnClick()
    {
        inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeInHierarchy);
        inventoryPanel.transform.SetAsLastSibling();
    }

    public void SkillsButtonOnClick()
    {
        skillsPanel.gameObject.SetActive(!skillsPanel.gameObject.activeInHierarchy);
        skillsPanel.transform.SetAsLastSibling();
    }

    public void SettingsButtonOnClick()
    {
        settingsPanel.gameObject.SetActive(!settingsPanel.gameObject.activeInHierarchy);
        settingsPanel.transform.SetAsLastSibling();
    }

    public void CharacterPanelCloseButtonOnClick()
    {
        characterPanel.gameObject.SetActive(false);
    }

    public void InventoryPanelCloseButtonOnClick()
    {
        inventoryPanel.gameObject.SetActive(false);
    }

    public void SkillsPanelCloseButtonOnClick()
    {
        skillsPanel.gameObject.SetActive(false);
    }

    public void SettingsPanelCloseButtonOnClick()
    {
        settingsPanel.gameObject.SetActive(false);
    }
#endif
}
#endif
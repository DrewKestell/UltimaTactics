using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform characterPanel;
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private RectTransform skillsPanel;
    [SerializeField] private RectTransform settingsPanel;

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
}

#if CLIENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIMediator : MonoBehaviour
{
    // Misc.
    [SerializeField] private ConnectionManager connectionManager;

    // Login Panel
    [SerializeField] private RectTransform loginPanel;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;

    // Character Select Panel
    [SerializeField] private RectTransform characterSelectPanel;
    [SerializeField] private GameObject characterListItemPrefab;

    // Character Create Panel
    [SerializeField] private RectTransform characterCreatePanel;
    [SerializeField] private TMP_InputField characterNameInput;
    [SerializeField] private TMP_Dropdown skillDropdown1;
    [SerializeField] private TMP_Dropdown skillDropdown2;
    [SerializeField] private TMP_Dropdown skillDropdown3;

    private Dictionary<string, int> skillMap;

#if CLIENT_BUILD
    public void Start()
    {
        PubSub.Instance.Subscribe<LoginSuccessfulEvent>(this, LoadCharacterSelect);
        PubSub.Instance.Subscribe<ReceivedCharacterCreationAssetsEvent>(this, LoadCharacterCreation);
        PubSub.Instance.Subscribe<CharacterCreationSuccessfulEvent>(this, CharacterCreationSuccessful);
    }

    public void LoginButtonOnClick()
    {
        var email = emailInput.text;
        var password = passwordInput.text;

        connectionManager.ConnectClient(email, password);
    }

    public void NewCharacterButtonOnClick()
    {
        // TODO: validate 3 unique skills have been selected
        connectionManager.RequestCharacterCreationAssetsServerRpc();
    }

    public void CreateCharacterButtonOnClick()
    {
        var characterName = characterNameInput.text;
        var skillId1 = skillMap[skillDropdown1.captionText.text];
        var skillId2 = skillMap[skillDropdown2.captionText.text];
        var skillId3 = skillMap[skillDropdown3.captionText.text];

        // TODO: reset form values

        connectionManager.CreateCharacterServerRpc(characterName, skillId1, skillId2, skillId3);
    }

    public void LoadCharacterSelect(LoginSuccessfulEvent e)
    {
        foreach (var character in e.Characters)
        {
            AddCharacterListItem(character);
        }

        loginPanel.gameObject.SetActive(false);
        characterSelectPanel.gameObject.SetActive(true);
    }

    public void LoadCharacterCreation(ReceivedCharacterCreationAssetsEvent e)
    {
        skillMap = e.Skills.ToDictionary(s => s.Name, s => s.Id);

        skillDropdown1.options = e.Skills.Select(s => new TMP_Dropdown.OptionData(s.Name)).ToList();
        skillDropdown2.options = e.Skills.Select(s => new TMP_Dropdown.OptionData(s.Name)).ToList();
        skillDropdown3.options = e.Skills.Select(s => new TMP_Dropdown.OptionData(s.Name)).ToList();

        characterSelectPanel.gameObject.SetActive(false);
        characterCreatePanel.gameObject.SetActive(true);
    }

    public void CharacterCreationSuccessful(CharacterCreationSuccessfulEvent e)
    {
        AddCharacterListItem(e.Character);

        characterCreatePanel.gameObject.SetActive(false);
        characterSelectPanel.gameObject.SetActive(true);
    }

    private void AddCharacterListItem(CharacterListItem character)
    {
        var characterCount = characterSelectPanel.childCount - 1;

        var yPos = -30.0f;
        if (characterCount > 0)
        {
            yPos = -30 + (characterCount * -40);
        }

        var gameObject = Instantiate(characterListItemPrefab, characterSelectPanel);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPos);
        var textComponent = gameObject.GetComponentInChildren<TMP_Text>();
        textComponent.text = character.Name;
    }
#endif
}
#endif
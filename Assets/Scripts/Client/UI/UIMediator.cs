#if CLIENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMediator : MonoBehaviour
{
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

    private Dictionary<string, SkillName> skillMap = new();
    private Dictionary<string, int> characterMap = new();
    private int? currentlySelectedCharacterId;

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

        ConnectionManager.Instance.ConnectClient(email, password);
    }

    public void NewCharacterButtonOnClick()
    {
        // TODO: validate 3 unique skills have been selected
        ConnectionManager.Instance.RequestCharacterCreationAssetsServerRpc();
    }

    public void CreateCharacterButtonOnClick()
    {
        var characterName = characterNameInput.text;
        var skillId1 = skillMap[skillDropdown1.captionText.text];
        var skillId2 = skillMap[skillDropdown2.captionText.text];
        var skillId3 = skillMap[skillDropdown3.captionText.text];

        // TODO: reset form values

        ConnectionManager.Instance.CreateCharacterServerRpc(characterName, skillId1, skillId2, skillId3);
    }

    public void EnterWorldButtonOnClick()
    {
        // TODO: the UI currently deselects the character list item if you click elsewhere,
        // need to do validation here to make sure a character is actually selected
        if (currentlySelectedCharacterId.HasValue)
        {
            ConnectionManager.Instance.EnterWorldServerRpc(currentlySelectedCharacterId.Value);
        }
    }

    public void CharacterListItemOnClick()
    {
        var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        var buttonText = button.GetComponentInChildren<TMP_Text>();
        currentlySelectedCharacterId = characterMap[buttonText.text];
    }

    public void LoadCharacterSelect(LoginSuccessfulEvent e)
    {
        characterMap = e.Characters.ToDictionary(s => s.Name, s => s.Id);

        foreach (var character in e.Characters)
        {
            AddCharacterListItem(character);
        }

        loginPanel.gameObject.SetActive(false);
        characterSelectPanel.gameObject.SetActive(true);
    }

    public void LoadCharacterCreation(ReceivedCharacterCreationAssetsEvent e)
    {
        var allSkills = SkillsManager.Instance.AllSkills;
        skillMap = allSkills.ToDictionary(s => s.Key.GetAttribute<DisplayAttribute>().Name, s => s.Key);
        var optionData = allSkills.Select(s => new TMP_Dropdown.OptionData(s.Key.GetAttribute<DisplayAttribute>().Name)).ToList();

        skillDropdown1.options = optionData;
        skillDropdown2.options = optionData;
        skillDropdown3.options = optionData;

        characterSelectPanel.gameObject.SetActive(false);
        characterCreatePanel.gameObject.SetActive(true);
    }

    public void CharacterCreationSuccessful(CharacterCreationSuccessfulEvent e)
    {
        AddCharacterListItem(e.Character);
        characterMap.Add(e.Character.Name, e.Character.Id);

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

        var instance = Instantiate(characterListItemPrefab, characterSelectPanel);
        instance.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPos);
        var textComponent = instance.GetComponentInChildren<TMP_Text>();
        textComponent.text = character.Name;
        instance.GetComponent<Button>().onClick.AddListener(CharacterListItemOnClick);
    }
#endif
}
#endif
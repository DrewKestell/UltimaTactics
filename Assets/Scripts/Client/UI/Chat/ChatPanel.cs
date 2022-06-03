using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChatPanel : MonoBehaviour
{
    [SerializeField] private RectTransform scrollableContent;
    [SerializeField] private GameObject chatMessageItemPrefab;
    [SerializeField] private TMP_InputField chatInput;

    private void Awake()
    {
        PubSub.Instance.Subscribe<ChatMessageEvent>(this, HandleChatMessage);
    }

    public void HandleChatMessage(ChatMessageEvent e)
    {
        var instance = Instantiate(chatMessageItemPrefab, scrollableContent);
        instance.GetComponent<TMP_Text>().text = $"[{e.Author}] {e.Message}";
    }

    public void OnEnter(InputAction.CallbackContext inputValue)
    {
        if (inputValue.ReadValueAsButton())
        {
            var messageText = chatInput.text;
            if (!string.IsNullOrWhiteSpace(messageText))
            {
                var chatMessage = new SerializablePlayerChatMessage(messageText);
                ConnectionManager.Instance.ChatMessageServerRpc(chatMessage);
            }

            chatInput.text = string.Empty;
            chatInput.ActivateInputField();
        }
    }
}

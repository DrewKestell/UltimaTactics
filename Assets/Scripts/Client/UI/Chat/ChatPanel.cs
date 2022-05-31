using UnityEngine;

public class ChatPanel : MonoBehaviour
{
    [SerializeField] private RectTransform scrollableContent;

    private void Awake()
    {
        PubSub.Instance.Subscribe<ChatMessageEvent>(this, HandleChatMessage);
    }

    public void HandleChatMessage(ChatMessageEvent e)
    {

    }
}

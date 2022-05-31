#if CLIENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChatMessageManager : MonoBehaviour
{
#if CLIENT_BUILD || UNITY_EDITOR
    [SerializeField] private ChatMessageScriptableObject[] ChatMessages;

    public Dictionary<ChatMessage, ChatMessageScriptableObject> AllChatMessages;

    public static ChatMessageManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        AllChatMessages = ChatMessages.ToDictionary(s => s.ChatMessage, s => s);
    }
#endif
}
#endif

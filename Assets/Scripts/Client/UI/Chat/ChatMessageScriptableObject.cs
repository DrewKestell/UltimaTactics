using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ChatMessageScriptableObject", order = 3)]
public class ChatMessageScriptableObject : ScriptableObject
{
    public ChatMessage ChatMessage;
    public string Value;
}

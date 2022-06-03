using System.Text;
using Unity.Netcode;

public struct SerializableChatMessage : INetworkSerializable
{
    public ChatMessage Message => message;
    public ChatMessageType Type => type;
    public string Author => Encoding.ASCII.GetString(authorBuffer);
    public string[] Parameters => Encoding.ASCII.GetString(parametersBuffer).Split('|');

    private ChatMessage message;
    private ChatMessageType type;
    private byte[] authorBuffer;
    private byte[] parametersBuffer;

    public SerializableChatMessage(
        ChatMessage message,
        ChatMessageType type,
        string author,
        params string[] parameters)
    {
        this.message = message;
        this.type = type;
        authorBuffer = Encoding.ASCII.GetBytes(author);
        parametersBuffer = Encoding.ASCII.GetBytes(string.Join('|', parameters));
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref message);
        serializer.SerializeValue(ref type);
        serializer.SerializeValue(ref authorBuffer);
        serializer.SerializeValue(ref parametersBuffer);
    }
}

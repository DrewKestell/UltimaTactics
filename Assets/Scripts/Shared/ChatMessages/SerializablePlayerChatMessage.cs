using System.Text;
using Unity.Netcode;

public struct SerializablePlayerChatMessage : INetworkSerializable
{
    public string MessageText => Encoding.ASCII.GetString(messageTextBuffer);

    private byte[] messageTextBuffer;

    public SerializablePlayerChatMessage(string messageText)
    {
        messageTextBuffer = Encoding.ASCII.GetBytes(messageText);
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref messageTextBuffer);
    }
}

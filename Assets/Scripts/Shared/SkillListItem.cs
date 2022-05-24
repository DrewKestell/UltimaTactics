using Unity.Netcode;

public class SkillListItem : INetworkSerializable
{
    public int Id;
    public string Name;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Id);
        serializer.SerializeValue(ref Name);
    }
}

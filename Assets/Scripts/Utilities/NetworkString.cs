using Unity.Collections;
using Unity.Netcode;

public struct NetworkString32 : INetworkSerializeByMemcpy
{
    private ForceNetworkSerializeByMemcpy<FixedString32Bytes> _info;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref _info);
    }

    public override string ToString()
    {
        return _info.Value.ToString();
    }

    public static implicit operator string(NetworkString32 s) => s.ToString();
    public static implicit operator NetworkString32(string s) => new() { _info = new FixedString32Bytes(s) };
}

public struct NetworkString64 : INetworkSerializeByMemcpy
{
    private ForceNetworkSerializeByMemcpy<FixedString64Bytes> _info;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref _info);
    }

    public override string ToString()
    {
        return _info.Value.ToString();
    }

    public static implicit operator string(NetworkString64 s) => s.ToString();
    public static implicit operator NetworkString64(string s) => new() { _info = new FixedString64Bytes(s) };
}

public struct NetworkString512 : INetworkSerializeByMemcpy
{
    private ForceNetworkSerializeByMemcpy<FixedString512Bytes> _info;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref _info);
    }

    public override string ToString()
    {
        return _info.Value.ToString();
    }

    public static implicit operator string(NetworkString512 s) => s.ToString();
    public static implicit operator NetworkString512(string s) => new() { _info = new FixedString512Bytes(s) };
}
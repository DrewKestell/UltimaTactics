#if SERVER_BUILD
using Unity.Netcode;

public abstract partial class WorldSaved : NetworkBehaviour
{
    private void Start()
    {
        WorldSaver.Instance.TrackObject(GetInstanceID(), this);
    }

    public virtual void Serialize() { }

    public virtual void Deserialize(int characterId) { }
}
#endif
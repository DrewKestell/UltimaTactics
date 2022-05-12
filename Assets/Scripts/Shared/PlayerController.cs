using Unity.Netcode;

public partial class PlayerController : NetworkBehaviour
{
    // Common variables needed on both server and client go here.
    //public NetworkVariable<Vector3> Position = new();

    private void Start()
    {
        OnStartup();
    }

    private void Update()
    {
        OnUpdate();
    }
}
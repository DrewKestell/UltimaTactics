using Unity.Netcode;
using UnityEngine;

public partial class PlayerController : NetworkBehaviour
{
    // Common variables needed on both server and client go here.
    public NetworkVariable<Vector3> Position = new();

    void Start()
    {
        // Common start-up code needed on both server and client
        // can be written here.

        // Call serve/client-specific start-up behaviour,
        // depending on which one we are.
        OnStartup();
    }

    void Move(Vector2 input)
    {
        // TODO: implement move logic.
        // Note that here, we don't know whether the input
        // came from a locally-connected gamepad/keyboard,
        // or a network packet. We can handle it agnostically.
    }
}
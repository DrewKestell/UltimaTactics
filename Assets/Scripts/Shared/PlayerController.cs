using Unity.Netcode;
using UnityEngine;

public partial class PlayerController : NetworkBehaviour
{
    // Common variables needed on both server and client go here.
    [SerializeField] private float moveSpeed;

    public NetworkVariable<float> turnMagnitude = new(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> walkMagnitude = new(default, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        OnStartup();
    }

    private void Update()
    {
        OnUpdate();

        Debug.Log($"IsServer: {NetworkManager.Singleton.IsServer}, IsClient: {NetworkManager.Singleton.IsClient}, TurnMagnitude: {turnMagnitude.Value}, WalkMagnitude: {walkMagnitude.Value}, IsOwner: {IsOwner}");
    }

    private Vector3 GetMoveVector() => moveSpeed * Time.deltaTime * walkMagnitude.Value * transform.forward;
}
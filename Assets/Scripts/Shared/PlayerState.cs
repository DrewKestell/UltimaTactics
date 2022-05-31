using Unity.Netcode;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public NetworkVariable<int> AccountId = new();
    public NetworkVariable<int> CharacterId = new();
    public NetworkVariable<NetworkString64> Name = new();
}

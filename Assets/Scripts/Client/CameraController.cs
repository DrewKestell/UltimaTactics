#if CLIENT_BUILD|| UNITY_EDITOR
using Unity.Netcode;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private NetworkObject player;
    private Vector3 offset;

    private void Start()
    {
        player = NetworkManager.Singleton.LocalClient.PlayerObject;
        offset = transform.position - player.transform.position;
    }

    private void Update()
    {
        transform.position = player.transform.position + offset;
    }
}
#endif
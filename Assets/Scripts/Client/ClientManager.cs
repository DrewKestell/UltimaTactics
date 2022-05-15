using Unity.Netcode;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
#if CLIENT_BUILD
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        if (GUILayout.Button("Client"))
        {
            NetworkManager.Singleton.StartClient();
        }

        GUILayout.EndArea();
    }
#endif
}

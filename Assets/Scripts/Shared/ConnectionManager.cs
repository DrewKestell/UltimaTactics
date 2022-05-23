using Unity.Netcode;
using UnityEngine;

public partial class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

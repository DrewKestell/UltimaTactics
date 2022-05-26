#if CLIENT_BUILD || UNITY_EDITOR
using UnityEngine;

public class UIInitializer : MonoBehaviour
{
    [SerializeField] private GameObject uiCanvasPrefab;

#if CLIENT_BUILD
    private void Awake()
    {
        Instantiate(uiCanvasPrefab, transform);
    }
#endif
}
#endif
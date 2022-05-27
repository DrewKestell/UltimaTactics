#if CLIENT_BUILD || UNITY_EDITOR
using TMPro;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{


#if CLIENT_BUILD
    private void Awake()
    {
        PubSub.Instance.Subscribe<RequestCharacterAssetsSuccessfulEvent>(this, InitializeInventoryPanel);
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void InitializeInventoryPanel(RequestCharacterAssetsSuccessfulEvent e)
    {
        
    }
#endif
}
#endif

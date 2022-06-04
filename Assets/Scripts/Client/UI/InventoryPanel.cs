#if CLIENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject inventoryItemPrefab;

    private readonly Dictionary<int, GameObject> itemMap = new();

#if CLIENT_BUILD
    private void Awake()
    {
        gameObject.transform.parent.gameObject.SetActive(false);

        PubSub.Instance.Subscribe<ItemAddedToInventoryEvent>(this, HandleItemAdded);
        PubSub.Instance.Subscribe<ItemRemovedFromInventoryEvent>(this, HandleItemRemoved);
    }

    public void HandleItemAdded(ItemAddedToInventoryEvent e)
    {
    }

    public void HandleItemRemoved(ItemRemovedFromInventoryEvent e)
    {

    }
#endif
}
#endif

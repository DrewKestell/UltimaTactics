using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [SerializeField] private ItemScriptableObject[] Items;

    public Dictionary<ItemName, ItemScriptableObject> AllItems;

    public static ItemsManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        AllItems = Items.ToDictionary(s => s.Name, s => s);
    }
}

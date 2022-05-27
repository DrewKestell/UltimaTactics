using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySpriteManager : MonoBehaviour
{
    [SerializeField] private InventorySprite[] sprites;

    public static InventorySpriteManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        AllSprites = sprites.ToDictionary(s => s.Id, s => s);
    }

    public Dictionary<int, InventorySprite> AllSprites { get; private set; }
}

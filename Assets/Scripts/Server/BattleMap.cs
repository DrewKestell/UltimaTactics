#if SERVER_BUILD || UNITY_EDITOR
using UnityEngine;

public class BattleMap : MonoBehaviour
{
    [SerializeField] private Vector2 mapDimensions = new(4, 4);
    [SerializeField] private int tileScale = 1;

#if SERVER_BUILD

    void Start()
    {
        BattleMapGenerator.GenerateBattleMap(mapDimensions, tileScale);
    }

    void Update()
    {
        
    }
#endif
}
#endif

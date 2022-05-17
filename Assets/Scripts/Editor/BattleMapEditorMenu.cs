using UnityEditor;
using UnityEngine;

public class BattleMapEditorMenu
{
    private const int tileScale = 1;

    private static readonly Vector2 mapDimensions = new(4, 4);

    [MenuItem("Battle Map/Generate %&h")]
    public static void Generate()
    {
        BattleMapGenerator.GenerateBattleMap(mapDimensions, tileScale);
    }
}

using UnityEngine;

public static class BattleMapGenerator
{
    private const int tileSize = 10;

    private static readonly GameObject battleMap = GameObject.Find("BattleMap");

    public static void GenerateBattleMap(Vector2 mapDimensions, int tileScale)
    {
        for (var x = 0; x < mapDimensions.x; x++)
        {
            for (var y = 0; y < mapDimensions.y; y++)
            {
                BuildTile(x, y, tileScale);
            }
        }
    }

    private static void BuildTile(int x, int y, int tileScale)
    {
        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.parent = battleMap.transform;
        plane.name = $"Tile_{x}_{y}";
        plane.transform.localPosition = new Vector3(x * tileSize * tileScale, 0, y * tileSize * tileScale);
        plane.transform.localScale = new Vector3(tileScale, 1, tileScale);

        var tileComponent = plane.AddComponent<Tile>();
    }
}

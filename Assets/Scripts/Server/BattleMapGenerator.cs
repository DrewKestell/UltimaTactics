using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public static class BattleMapGenerator
{
    private const int gameTileSize = 10;

    private static readonly GameObject battleMap = GameObject.Find("BattleMap");
    private static readonly GameObject gameTiles = GameObject.Find("GameTiles");
    private static readonly GameObject renderTiles = GameObject.Find("RenderTiles");

    private static readonly Material[] materials;
    private static readonly Texture2D textureMap;

    static BattleMapGenerator()
    {
        var battleMapComponent = battleMap.GetComponent<BattleMap>();
        materials = battleMapComponent.Materials;
        textureMap = battleMapComponent.TextureMap;
    }

    public static void GenerateBattleMap(Vector2 mapDimensions, int tileScale)
    {
        for (var x = 0; x < mapDimensions.x; x++)
        {
            for (var y = 0; y < mapDimensions.y; y++)
            {
                BuildGameTile(x, y, tileScale);
            }
        }
    }

    private static void BuildGameTile(int gameTileX, int gameTileY, int tileScale)
    {
        var scaledGameTileSize = tileScale * gameTileSize;

        // create game tiles
        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.parent = gameTiles.transform;
        plane.name = $"GameTile_{gameTileX}_{gameTileY}";
        plane.transform.localPosition = new Vector3(gameTileX * scaledGameTileSize, 0, gameTileY * scaledGameTileSize);
        plane.transform.localScale = new Vector3(tileScale, 1, tileScale);

        Assert.AreEqual(0, scaledGameTileSize % 2);
        var meshRenderer = plane.GetComponent<MeshRenderer>();
        var material = GetMaterialFromTextureMap((int)plane.transform.localPosition.x, (int)plane.transform.localPosition.z, scaledGameTileSize);
        meshRenderer.sharedMaterial = material;
    }

    private static Material GetMaterialFromTextureMap(int tileCenterPosX, int tileCenterPosY, int scaledRenderTileSize)
    {
        var dict = new Dictionary<Color, int>();
        for (var x = tileCenterPosX; x < tileCenterPosX + scaledRenderTileSize; x++)
        {
            for (var y = tileCenterPosY; y < tileCenterPosY + scaledRenderTileSize; y++)
            {
                var color = textureMap.GetPixel(x, y);
                Debug.Log(color);
                Debug.Log($"{x}, {y}");
                if (dict.ContainsKey(color))
                {
                    dict[color]++;
                }
                else
                {
                    dict.Add(color, 1);
                }
            }
        }

        var last = dict.OrderBy(c => c.Value).Last().Key;
        Debug.Log(last);
        if (last.r == 0 && last.g == 0 && last.b == 0)
        {
            return materials[1];
        }
        else
        {
            return materials[0];
        }
    }
}

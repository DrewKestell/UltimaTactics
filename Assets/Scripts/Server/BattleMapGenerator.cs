using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public static class BattleMapGenerator
{
    private const int gameTileSize = 10;
    private const int renderTilesPerGameTile = 5;

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
        Object.DestroyImmediate(plane.GetComponent<MeshRenderer>());
        plane.transform.parent = gameTiles.transform;
        plane.name = $"GameTile_{gameTileX}_{gameTileY}";
        plane.transform.localPosition = new Vector3(gameTileX * scaledGameTileSize, 0, gameTileY * scaledGameTileSize);
        plane.transform.localScale = new Vector3(tileScale, 1, tileScale);

        var tileComponent = plane.AddComponent<Tile>();

        // create render tiles
        Assert.AreEqual(0, scaledGameTileSize % renderTilesPerGameTile);
        for (var x = 0; x < renderTilesPerGameTile; x++)
        {
            for (var y = 0; y < renderTilesPerGameTile; y++)
            {
                BuildRenderTile(
                    (int)plane.transform.localPosition.x,
                    (int)plane.transform.localPosition.z,
                    x,
                    y,
                    scaledGameTileSize,
                    tileScale);
            }
        }
    }

    private static void BuildRenderTile(int gameTileXPos, int gameTileYPos, int renderTileX, int renderTileY, int scaledGameTileSize, int tileScale)
    {
        var scaledRenderTileSize = scaledGameTileSize / renderTilesPerGameTile;

        var renderTileXPos = gameTileXPos + (renderTileX * scaledRenderTileSize);
        var renderTileYPos = gameTileYPos + (renderTileY * scaledRenderTileSize);

        var color = GetRenderTileColorFromTextureMap(renderTileXPos, renderTileYPos, scaledRenderTileSize);
        var renderTilePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        renderTilePlane.transform.parent = renderTiles.transform;
        renderTilePlane.name = $"RenderTile_{renderTileXPos}_{renderTileYPos}";
        renderTilePlane.transform.localPosition = new Vector3(renderTileXPos, 0, renderTileYPos);
        renderTilePlane.transform.localScale = new Vector3(tileScale, 1, tileScale);
        var meshRenderer = renderTilePlane.GetComponent<MeshRenderer>();
        meshRenderer.material.SetColor("_Color", color);
    }

    private static Color GetRenderTileColorFromTextureMap(int renderTileXPos, int renderTileYPos, int scaledRenderTileSize)
    {
        var dict = new Dictionary<Color, int>();
        for (var x = renderTileXPos; x < renderTileXPos + scaledRenderTileSize; x++)
        {
            for (var y = renderTileYPos; y < renderTileYPos + scaledRenderTileSize; y++)
            {
                var color = textureMap.GetPixel(x, y);

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

        return dict.OrderBy(c => c.Value).Last().Key;
    }
}

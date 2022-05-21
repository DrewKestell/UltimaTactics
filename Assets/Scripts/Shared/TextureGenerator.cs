using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static FastNoise;

public static class TextureGenerator
{
    public static Texture2D GenerateTexture(int width, int height, int threshold)
    {
        var texture = new Texture2D(width, height);
        var seed = UnityEngine.Random.Range(0, int.MaxValue);
        var fastNoise = new FastNoise(seed);
        fastNoise.SetNoiseType(NoiseType.Perlin);
        fastNoise.SetFrequency(0.005f);
        fastNoise.SetInterp(Interp.Quintic);
        fastNoise.SetFractalType(FractalType.FBM);
        fastNoise.SetFractalOctaves(5);
        fastNoise.SetFractalLacunarity(2.0f);
        fastNoise.SetFractalGain(0.5f);

        var noiseValues = new List<float>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                noiseValues.Add(fastNoise.GetNoise(x, y));
            }
        }

        noiseValues.Sort();
        var min = math.abs(noiseValues[0]);
        var max = noiseValues[noiseValues.Count - 1] + min;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var noise = (int)(((fastNoise.GetNoise(x, y) + min) / max) * 255);

                Color diffuseColor;

                if (noise < threshold)
                    diffuseColor = new Color(0, 0, 0);
                else
                    diffuseColor = new Color(255, 255, 255);

                texture.SetPixel(x, y, diffuseColor);
            }
        }

        return texture;
    }

    public static Texture2D Resize(Texture2D texture2D, int width, int height)
    {
        var rt = new RenderTexture(width, height, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        var result = new Texture2D(width, height);
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();
        return result;
    }
}

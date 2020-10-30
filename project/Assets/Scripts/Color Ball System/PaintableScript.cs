using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableScript : MonoBehaviour
{
    [Tooltip("Number of pixels per 1 unit of size in world coordinates.")]
    [Range(16, 8182)]
    public int textureSize = 64;

    private readonly Color c_color = new Color(0, 0, 0, 0);

    private Material material;
    private Texture2D texture;
    private bool isEnabled = false;


    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (null == renderer)
            return;

        foreach (Material material in renderer.materials)
        {
            if (material.shader.name.Contains("Custom/DecalShader"))
            {
                this.material = material;
                break;
            }
        }

        if (null != material)
        {
            texture = new Texture2D(textureSize, textureSize);
            for (int x = 0; x < textureSize; ++x)
                for (int y = 0; y < textureSize; ++y)
                    texture.SetPixel(x, y, c_color);
            texture.Apply();

            material.SetTexture("_DrawingTex", texture);
            isEnabled = true;
        }
    }

    public void PaintOnColored(Vector2 textureCoord, Texture2D splashTexture, Color color)
    {
        MyPaintOn(textureCoord, splashTexture, color);
    }

    private void MyPaintOn(Vector2 textureCoord, Texture2D splashTexture, Color targetColor)
    {
        if (isEnabled)
        {
            int reqnx = splashTexture.width;
            int reqny = splashTexture.height;
            int reqX = (int)(textureCoord.x * textureSize) - (reqnx / 2);
            int reqY = (int)(textureCoord.y * textureSize) - (reqny / 2);
            int right = texture.width - 1;
            int bottom = texture.height - 1;

            int x = IntMax(reqX, 0);
            int y = IntMax(reqY, 0);
            int nx = IntMin(x + reqnx, right) - x;
            int ny = IntMin(y + reqny, bottom) - y;

            Color[] pixels = texture.GetPixels(x, y, nx, ny);

            int counter = 0;
            for (int i = 0; i < nx; ++i)
            {
                for (int j = 0; j < ny; ++j)
                {
                    float currAlpha = splashTexture.GetPixel(i, j).a;
                    if (currAlpha == 1)
                        pixels[counter] = targetColor;
                    else
                    {
                        Color currColor = pixels[counter];
                        // resulting color is an addition of splash texture to the texture based on alpha
                        Color newColor = Color.Lerp(currColor, targetColor, currAlpha);
                        // but resulting alpha is a sum of alphas (adding transparent color should not make base color more transparent)
                        newColor.a = pixels[counter].a + currAlpha;
                        pixels[counter] = newColor;
                    }
                    counter++;
                }
            }

            texture.SetPixels(x, y, nx, ny, pixels);
            texture.Apply();
        }
    }

    private int IntMax(int a, int b)
    {
        return a > b ? a : b;
    }

    private int IntMin(int a, int b)
    {
        return a < b ? a : b;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableScript : MonoBehaviour
{
    [Tooltip("Number of pixels per 1 unit of size in world coordinates.")]
    [Range(16, 8182)]
    public int textureSize = 64;

    private readonly Color baseColor = new Color(0, 0, 0, 0);

    private Material material;
    private Texture2D texture;
    private bool isEnabled = false;


    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (null == renderer)
        { return; }

        foreach (Material material in renderer.materials)
        {
            if (material.shader.name.Contains("Custom/DecalShader"))
            {
                this.material = material;
                break;
            }
        }

        if (material != null)
        {
            texture = new Texture2D(textureSize, textureSize);

            for (int x = 0; x < textureSize; x++)
            {
                for (int y = 0; y < textureSize; y++)
                {
                    texture.SetPixel(x, y, baseColor);
                }
            }
            texture.Apply();

            material.SetTexture("_DrawingTex", texture);
            isEnabled = true;
        }
    }


    public void PaintOn(Vector2 textureCoord, Texture2D splashTexture, Color color)
    {
        if (isEnabled)
        {
            int realX = (int)(textureCoord.x * textureSize) - (splashTexture.width / 2);
            int realY = (int)(textureCoord.y * textureSize) - (splashTexture.height / 2);

            for (int texX = 0; texX < splashTexture.width; texX++)
            {
                for (int texY = 0; texY < splashTexture.height; texY++)
                {
                    Color result = Color.Lerp(texture.GetPixel(realX + texX, realY + texY), color, splashTexture.GetPixel(texX, texY).a);
                    result.a += splashTexture.GetPixel(texX, texY).a;

                    texture.SetPixel(realX + texX, realY + texY, result);
                }
            }
        }

        texture.Apply();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPaint : MonoBehaviour {

    private readonly Color c_color = new Color(0, 0, 0, 0);
    //`... (correct renderer and material availability check) ...

    public int textureWidth, textureHeight;

    public Texture2D  m_texture;
    public Material m_material;

    public bool isEnabled;
    public Color color;

    public Color existingColor, targetColor;
    public float alpha;

    public int newX, newY;
    // Use this for initialization
    void Start () {
        //m_material = GetComponent<Material>();
        m_texture = new Texture2D(textureWidth, textureHeight);
        for (int x = 0; x < textureWidth; ++x)
            for (int y = 0; y < textureHeight; ++y)
                m_texture.SetPixel(x, y, color);
        m_texture.Apply();
        m_material.SetTexture("_DrawingTex", m_texture);
        isEnabled = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PaintOn(Vector2 textureCoord, Texture2D splashTexture)
    {
        if (isEnabled)
        {
            int x = (int)(textureCoord.x * textureWidth) - (splashTexture.width / 2);
            int y = (int)(textureCoord.y * textureHeight) - (splashTexture.height / 2);
            for (int i = 0; i < splashTexture.width; ++i)
                for (int j = 0; j<splashTexture.height;  j++)
                {
                    Color result = Color.Lerp(existingColor, targetColor, alpha);   // resulting color is an addition of splash texture to the texture based on alpha
                    result.a = existingColor.a + alpha;                             // but resulting alpha is a sum of alphas (adding transparent color should not make base color more transparent)
                    m_texture.SetPixel(newX, newY, result);
                }
        }

        m_texture.Apply();
    }
}


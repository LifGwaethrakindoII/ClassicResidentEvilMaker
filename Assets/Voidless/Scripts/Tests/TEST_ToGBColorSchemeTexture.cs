using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Voidless;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TEST_ToGBColorSchemeTexture : MonoBehaviour
{
    [SerializeField] private string path;
    [SerializeField] private Sprite sprite;
    [SerializeField] private ColorSwatches swatches;
    [SerializeField] private Texture2D texture;

    [Button("Test")]
    private void Test()
    {
        texture = sprite.ToTexture();
        Color[] pixels = sprite.texture.GetPixels
        ( 
            (int)sprite.textureRect.x, 
            (int)sprite.textureRect.y, 
            (int)sprite.textureRect.width, 
            (int)sprite.textureRect.height
        );
        Color[] newPixels = new Color[pixels.Length];

        for(int i = 0; i < pixels.Length; i++)
        {
            float min = Mathf.Infinity;
            float key = 0.0f;
            float t = pixels[i].GetNormalizedValue();

            foreach(float value in swatches.orderedMap.Keys)
            {
                float d = Mathf.Abs(t - value);
                
                if(d < min)
                {
                    key = value;
                    min = d;
                }
            }

            Color color = swatches.orderedMap[key];
            newPixels[i] = color;
        }

        texture.MarkTextureAsReadable();
        texture.SetPixels(newPixels);
        texture.Apply();
/*#if UNITY_EDITOR
        AssetDatabase.CreateAsset(texture, "Assets/" + path);
        AssetDatabase.Refresh();
#endif*/
    }
}

using UnityEngine;
using UnityEditor;
 
public class SpritePostProcessor : AssetPostprocessor {

    int pixelsPerUnit = 128;
    bool mipMapEnabled = false;
    FilterMode filterMode = FilterMode.Point;

    void OnPostprocessTexture(Texture2D texture) {
        TextureImporter ti = (assetImporter as TextureImporter);
        ti.spritePixelsPerUnit = pixelsPerUnit;
        ti.filterMode = filterMode;

        ti.mipmapEnabled = mipMapEnabled;
        ti.alphaIsTransparency = true;

        TextureImporterSettings importerSettings = new TextureImporterSettings();
        ti.ReadTextureSettings(importerSettings);


        ti.SetTextureSettings(importerSettings);
    }
}
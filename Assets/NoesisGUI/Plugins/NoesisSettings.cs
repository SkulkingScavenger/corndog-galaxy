using UnityEngine;
using System.IO;

/// <summary>
/// Noesis global settings
/// </summary>
public class NoesisSettings: ScriptableObject
{
    public static bool IsNoesisEnabled()
    {
#if UNITY_EDITOR
        string lockfile = Path.Combine(Application.dataPath, "NoesisGUI/Plugins/Editor/installing");
        return !File.Exists(lockfile) && !UnityEditorInternal.InternalEditorUtility.inBatchMode;
#else
        return true;
#endif
    }

    private static NoesisSettings _settings;

    public static NoesisSettings Get()
    {
        if (_settings == null)
        {
            _settings = Resources.Load<NoesisSettings>("NoesisSettings");

#if UNITY_EDITOR
            if (_settings == null)
            {
                _settings = (NoesisSettings)ScriptableObject.CreateInstance(typeof(NoesisSettings));
                Directory.CreateDirectory(Application.dataPath + "/NoesisGUI/Settings/Resources");
                UnityEditor.AssetDatabase.CreateAsset(_settings, "Assets/NoesisGUI/Settings/Resources/NoesisSettings.asset");
            }
#endif
        }

        return _settings;
    }

    [Tooltip("Sets a collection of application-scope resources, such as styles and brushes. " +
        "Provides a simple way to support a consistent theme across your application")]
    public NoesisXaml applicationResources;

    public enum TextureSize
    {
        _256x256,
        _512x512,
        _1024x1024,
        _2048x2048,
        _4096x4096
    }

    [Header("Text Rendering")]
    [Tooltip("Dimensions of texture used to cache glyphs")]
    public TextureSize glyphTextureSize = TextureSize._1024x1024;

    [Range(32, 256)]
    [Tooltip("Glyphs with size above this are rendered using triangles")]
    public uint glyphMeshThreshold = 96;

    [Header("Offscreen")]
    [Tooltip("Dimensions of offscreen textures (0 = automatic)")]
    public Vector2 offscreenTextureSize;

    [Tooltip("Number of offscreen textures created at startup")]
    public uint offscreenInitSurfaces = 0;

    [Tooltip("Maximum number of offscreen textures (0 = unlimited)")]
    public uint offscreenMaxSurfaces = 0;

    [Header("Editor Settings")]
    [Tooltip("Enables generation of thumbnails and previews")]
    public bool previewEnabled = true;

    public enum LogVerbosity
    {
        Quiet,
        Normal,
        Bindings
    }

    public LogVerbosity logVerbosity = LogVerbosity.Quiet;

    [Space(10)]
    [Tooltip("XAMLs to register at startup. They can be loaded using Noesis.GUI.LoadXaml() without referrencing them in scripts or copying to /Resources")]
    public NoesisXaml[] preloadedXamls;
}

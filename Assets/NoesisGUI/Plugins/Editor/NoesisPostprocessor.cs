using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Linq;

/// <summary>
/// Post-processor for XAML and Fonts
/// </summary>
public class NoesisPostprocessor : AssetPostprocessor
{
    public static void ImportAllAssets()
    {
        var assets = AssetDatabase.FindAssets("").Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Where(s => s.EndsWith(".xaml", StringComparison.OrdinalIgnoreCase) ||
                        s.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) ||
                        s.EndsWith(".otf", StringComparison.OrdinalIgnoreCase))
            .Distinct().ToArray();

        NoesisPostprocessor.ImportAssets(assets, (progress, asset) => EditorUtility.DisplayProgressBar("Reimport All XAMLs", asset, progress));
        EditorUtility.ClearProgressBar();
    }

    private delegate void UpdateProgress(float progress, string asset);

    private static void ImportAssets(string[] assets, UpdateProgress d)
    {
        int numFonts = assets.Count(asset => HasExtension(asset, ".ttf") || HasExtension(asset, ".otf"));
        int numXamls = assets.Count(asset => HasExtension(asset, ".xaml"));
        int numAssets = numFonts + numXamls;

        float delta = 1.0f / numAssets;
        float progress = 0.0f;

        // Do fonts first because XAML depends on the generated .asset
        foreach (var asset in assets)
        {
            if (HasExtension(asset, ".ttf") || HasExtension(asset, ".otf"))
            {
                ImportFont(asset);

                if (d != null)
                {
                    d(progress, asset);
                    progress += delta;
                }
            }
        }

        // First, create all .asset resources to allow dependencies between XAMLs
        foreach (var asset in assets)
        {
            if (HasExtension(asset, ".xaml"))
            {
                CreateXamlAsset(asset);
            }
        }

        // And now, fully import each XAML
        foreach (var asset in assets)
        {
            if (HasExtension(asset, ".xaml"))
            {
                ImportXaml(asset);

                if (d != null)
                {
                    d(progress, asset);
                    progress += delta;
                }
            }
        }
    }

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (NoesisSettings.IsNoesisEnabled())
        {
            ImportAssets(importedAssets.Concat(movedAssets).ToArray(), null);
        }
    }

    private static bool HasExtension(string filename, string extension)
    {
        return filename.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Imports a TrueType into a NoesisFont asset
    /// </summary>
    private static void ImportFont(string filename)
    {
        using (FileStream file = File.Open(filename, FileMode.Open))
        {
            string path = Path.ChangeExtension(filename, ".asset");
            var font = AssetDatabase.LoadAssetAtPath<NoesisFont>(path);

            if (font == null)
            {
                font = (NoesisFont)ScriptableObject.CreateInstance(typeof(NoesisFont));
                font.source = filename;
                font.content = new byte[file.Length];
                file.Read(font.content, 0, (int)file.Length);
                AssetDatabase.CreateAsset(font, path);
            }
            else
            {
                font.source = filename;
                font.content = new byte[file.Length];
                file.Read(font.content, 0, (int)file.Length);
                EditorUtility.SetDirty(font);
            }
        }
    }

    /// <summary>
    /// Unity always expects paths with forward slashes (/) and rooted at "Assets/"
    /// </summary>
    private static string NormalizePath(string uri)
    {
        string full = Path.GetFullPath(uri).Replace('\\', '/');
        return full.Substring(full.IndexOf("Assets/"));
    }

    /// <summary>
    /// In XAML, URIs are relative by default. Absolute URIs use the following syntaxes:
    ///     "pack://application:,,,/path1/path2/resource.ext"
    ///     "/ReferencedAssembly;component/path1/path2/resource.ext"
    ///     "/path1/path2/resource.ext"
    /// </summary>
    private static string AbsolutePath(string parent, string uri)
    {
        const string PackUri = "pack://application:,,,";
        int n = uri.IndexOf(PackUri);
        if (n != -1)
        {
            uri = uri.Substring(n + PackUri.Length);
        }

        const string ComponentUri = ";component";
        n = uri.IndexOf(ComponentUri);
        if (n != -1)
        {
            uri = uri.Substring(n + ComponentUri.Length);
        }

        if (uri.StartsWith("/"))
        {
            return NormalizePath(uri);
        }
        else
        {
            return NormalizePath(parent + "/" + uri);
        }
    }

    /// <summary>
    /// Returns all the usages found of the given keyword under quotation marks
    /// </summary>
    private static List<string> ScanKeyword(string text, string keyword)
    {
        List<string> strings = new List<string>();

        int cur = 0;
        int pos;

        while ((pos = text.IndexOf(keyword, cur, StringComparison.OrdinalIgnoreCase)) != -1)
        {
            int start = pos;
            while (start >= 0 && text[start] != '\"' && text[start] != '\'' && text[start] != '>')
            {
                start--;
            }

            int end = pos;
            while (end < text.Length && text[end] != '\"' && text[end] != '\'' && text[end] != '<')
            {
                end++;
            }

            if (start >= 0 && end < text.Length)
            {
                strings.Add(text.Substring(start + 1, end - start - 1));
            }

            cur = pos + keyword.Length;
        }

        return strings;
    }

    private static void ScanTexture(string directory, string text, string extension, HashSet<Texture> textures)
    {
        List<string> keywords = ScanKeyword(text, extension);

        foreach (var keyword in keywords)
        {
            string uri = AbsolutePath(directory, keyword);
            string guid = AssetDatabase.AssetPathToGUID(uri);

            if (!String.IsNullOrEmpty(guid))
            {
                Texture t = AssetDatabase.LoadAssetAtPath<Texture>(uri);

                if (t != null)
                {
                    textures.Add(t);
                }
            }
        }
    }

    private static void ScanTextures(NoesisXaml xaml, string directory, string text)
    {
        var textures = new HashSet<Texture>();

        ScanTexture(directory, text, ".jpg", textures);
        ScanTexture(directory, text, ".tga", textures);
        ScanTexture(directory, text, ".png", textures);
        ScanTexture(directory, text, ".gif", textures);
        ScanTexture(directory, text, ".bmp", textures);

        xaml.textures = textures.ToArray();
        xaml.texturePaths = textures.Select(t => AssetDatabase.GetAssetPath(t)).ToArray();
    }

    private static void FindFamilyNames(string directory, string family, HashSet<NoesisFont> fonts)
    {
        try
        {
            var files = Directory.GetFiles(directory)
                .Where(s => s.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase) || 
                            s.EndsWith(".otf", StringComparison.OrdinalIgnoreCase));

            foreach (var filename in files)
            {
                using (FileStream file = File.Open(filename, FileMode.Open))
                {
                    if (NoesisUnity.HasFamily(file, family))
                    {
                        string uri = Path.ChangeExtension(NormalizePath(filename), ".asset");
                        string guid = AssetDatabase.AssetPathToGUID(uri);

                        if (!String.IsNullOrEmpty(guid))
                        {
                            NoesisFont f = AssetDatabase.LoadAssetAtPath<NoesisFont>(uri);

                            if (f != null)
                            {
                                fonts.Add(f);
                            }
                        }
                    }
                }
            }
        }
        catch(System.Exception) {}
    }

    private static void ScanFonts(NoesisXaml xaml, string directory, string text)
    {
        var fonts = new HashSet<NoesisFont>();

        List<string> keywords = ScanKeyword(text, "#");
        foreach (var keyword in keywords)
        {
            int index = keyword.IndexOf('#');
            string folder = AbsolutePath(directory, keyword.Substring(0, index));
            string family = keyword.Substring(index + 1);
            FindFamilyNames(folder, family, fonts);
        }

        xaml.fonts = fonts.ToArray();
    }

    private static void ScanXamls(NoesisXaml xaml, string directory, string text)
    {
        var xamls = new HashSet<NoesisXaml>();

        List<string> keywords = ScanKeyword(text, ".xaml");
        foreach (var keyword in keywords)
        {
            string uri = AbsolutePath(directory, Path.ChangeExtension(keyword, ".asset"));
            string guid = AssetDatabase.AssetPathToGUID(uri);

            if (!String.IsNullOrEmpty(guid))
            {
                NoesisXaml x = AssetDatabase.LoadAssetAtPath<NoesisXaml>(uri);

                if (x != null)
                {
                    xamls.Add(x);
                }
            }
        }

        xaml.xamls = xamls.ToArray();
    }

    private static void ScanDependencies(NoesisXaml xaml, string directory)
    {
        string text = System.Text.Encoding.UTF8.GetString(xaml.content);

        // Remove comments
        Regex exp = new Regex("<!--(.*?)-->", RegexOptions.Singleline);
        text = exp.Replace(text, "");

        ScanTextures(xaml, directory, text);
        ScanFonts(xaml, directory, text);
        ScanXamls(xaml, directory, text);

        xaml.ReloadDependencies();
    }

    private static void ImportXaml(NoesisXaml xaml, string directory, FileStream file)
    {
        xaml.content = new byte[file.Length];
        file.Read(xaml.content, 0, (int)file.Length);

        ScanDependencies(xaml, directory);
    }

    private static void ImportXaml(string filename)
    {
        NoesisUnity.Init();

        string path = Path.ChangeExtension(filename, ".asset");
        var xaml = AssetDatabase.LoadAssetAtPath<NoesisXaml>(path);

        using (FileStream file = File.Open(filename, FileMode.Open))
        {
            string directory = Path.GetDirectoryName(filename);
            ImportXaml(xaml, directory, file);
        }

        // The following steps (updating thumbnail and loading XAML for logging errors) must be
        // done after all XAMLs are imported to avoid dependency order issues. It is also a good
        // idea doing this outside OnPostprocessAllAssets to avoid continuously crashing Unity if
        // Load() raises an unexception exception. That is the reason we use a deferred call.
        EditorApplication.CallbackFunction d = null;

        d = () =>
        {
            // TODO: if Unity is compiling scripts (EditorApplication.isCompiling) we should wait

            EditorApplication.update -= d;

            try
            {
                xaml.Load();
                EditorUtility.SetDirty(xaml);

            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e, xaml);
            }
        };

        EditorApplication.update += d;
    }

    private static void CreateXamlAsset(string filename)
    {
        string uri = Path.ChangeExtension(filename, ".asset");
        var xaml = AssetDatabase.LoadAssetAtPath<NoesisXaml>(uri);

        if (xaml == null)
        {
            xaml = (NoesisXaml)ScriptableObject.CreateInstance(typeof(NoesisXaml));
            xaml.source = filename;
            AssetDatabase.CreateAsset(xaml, uri);
        }
        else
        {
            xaml.source = filename;
        }
    }
}

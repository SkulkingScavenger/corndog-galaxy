using Noesis;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;

/// <summary>
/// Xaml provider
/// </summary>
public class NoesisXamlProvider: XamlProvider
{
    public static NoesisXamlProvider instance = new NoesisXamlProvider();

    NoesisXamlProvider()
    {
        _xamls = new Dictionary<string, Value>();
    }

    public void Register(NoesisXaml xaml)
    {
        string uri = xaml.source;
        Value v;

        if (_xamls.TryGetValue(uri, out v))
        {
            v.refs++;
            v.xaml = xaml;
            _xamls[uri] = v;
        }
        else
        {
            _xamls[uri] = new Value() { refs = 1, xaml = xaml };
        }
    }

    public void Unregister(NoesisXaml xaml)
    {
        string uri = xaml.source;
        Value v;

        if (_xamls.TryGetValue(uri, out v))
        {
            if (v.refs == 1)
            {
                _xamls.Remove(xaml.source);
            }
            else
            {
                v.refs--;
                _xamls[uri] = v;
            }
        }
    }

    public override Stream LoadXaml(string uri)
    {
        Value v;
        if (_xamls.TryGetValue(uri, out v))
        {
            return new MemoryStream(v.xaml.content);
        }

        return null;
    }

    public struct Value
    {
        public int refs;
        public NoesisXaml xaml;
    }

    private Dictionary<string, Value> _xamls;
}

/// <summary>
/// Texture provider
/// </summary>
public class NoesisTextureProvider: TextureProvider
{
    public static NoesisTextureProvider instance = new NoesisTextureProvider();

    NoesisTextureProvider()
    {
        _textures = new Dictionary<string, Value>();
    }

    public void Register(string uri, UnityEngine.Texture texture)
    {
        lock(_lock)
        {
            Value v;
            if (_textures.TryGetValue(uri, out v))
            {
                v.refs++;
                v.texture = texture;
                _textures[uri] = v;
            }
            else
            {
                _textures[uri] = new Value() { refs = 1, texture = texture };
            }
        }
    }

    public void Unregister(string uri)
    {
        lock(_lock)
        {
            Value v;
            if (_textures.TryGetValue(uri, out v))
            {
                if (v.refs == 1)
                {
                    _textures.Remove(uri);
                }
                else
                {
                    v.refs--;
                    _textures[uri] = v;
                }
            }
        }
    }

    public override void GetTextureInfo(string uri, out uint width, out uint height)
    {
        width = 0;
        height = 0;

        Value v;

        lock(_lock)
        {
            _textures.TryGetValue(uri, out v);
        }

        // Mutex must not be locked here beacuse GetNativeTexturePtr() may wait for the render thread
        if (v.texture != null)
        {
            width = (uint)v.texture.width;
            height = (uint)v.texture.height;

            // Store all texture info in C++ TextureProvider so we can create texture in render
            // thread without calling back C# because this breaks Unity mono
            StoreTextureInfo(uri, v.texture);
        }
    }

    public struct Value
    {
        public int refs;
        public UnityEngine.Texture texture;
    }

    private Dictionary<string, Value> _textures;
    private readonly object _lock = new object();

    private void StoreTextureInfo(string uri, UnityEngine.Texture texture)
    {
        UnityEngine.Texture2D texture2D = texture as UnityEngine.Texture2D;

        Noesis_TextureProviderStoreTextureInfo(swigCPtr.Handle, uri, texture.width, texture.height,
            texture2D != null ? texture2D.mipmapCount : 1, texture.GetNativeTexturePtr());
        Noesis.Error.Check();
    }

    internal new static IntPtr Extend(string typeName)
    {
        IntPtr nativeType = Noesis_TextureProviderExtend(
            System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(typeName));
        Noesis.Error.Check();

        return nativeType;
    }

    #region Imports
    [DllImport(Library.Name)]
    static extern IntPtr Noesis_TextureProviderExtend(IntPtr typeName);

    [DllImport(Library.Name)]
    static extern void Noesis_TextureProviderStoreTextureInfo(IntPtr cPtr,
        [MarshalAs(UnmanagedType.LPStr)] string filename, int width, int height, int numLevels,
        IntPtr nativePtr);
    #endregion
}

/// <summary>
/// Font provider
/// </summary>
public class NoesisFontProvider: FontProvider
{
    private static OpenFontCallback _openFont = OpenFont;
    public static NoesisFontProvider instance = new NoesisFontProvider();

    NoesisFontProvider()
    {
        Noesis_FontProviderSetCallback(Noesis.Extend.GetInstanceHandle(this).Handle, _openFont);

        _fonts = new Dictionary<string, Value>();
    }

    public void Register(NoesisFont font)
    {
        string uri = font.source;
        Value v;

        if (_fonts.TryGetValue(uri, out v))
        {
            v.refs++;
            v.font = font;
            _fonts[uri] = v;
        }
        else
        {
            _fonts[uri] = new Value() { refs = 1, font = font };
        }

        string folder = System.IO.Path.GetDirectoryName(uri);
        string filename = System.IO.Path.GetFileName(uri);
        RegisterFont(folder, filename);
    }

    public void Unregister(NoesisFont font)
    {
        string uri = font.source;
        Value v;

        if (_fonts.TryGetValue(uri, out v))
        {
            if (v.refs == 1)
            {
                _fonts.Remove(uri);
            }
            else
            {
                v.refs--;
                _fonts[uri] = v;
            }
        }
    }

    private delegate void OpenFontCallback(IntPtr cPtr, IntPtr stream, string folder, string id);

    [MonoPInvokeCallback(typeof(OpenFontCallback))]
    private static void OpenFont(IntPtr cPtr, IntPtr stream, string folder, string id)
    {
        NoesisFontProvider provider = (NoesisFontProvider)Noesis.Extend.GetExtendInstance(cPtr);

        Value v;
        provider._fonts.TryGetValue(folder + "/" + id, out v);

        if (v.font != null && v.font.content != null)
        {
            Noesis_FontProviderCopyBuffer(cPtr, stream, v.font.content, v.font.content.Length);
        }
    }

    public struct Value
    {
        public int refs;
        public NoesisFont font;
    }

    private Dictionary<string, Value> _fonts;

    internal new static IntPtr Extend(string typeName)
    {
        IntPtr nativeType = Noesis_FontProviderExtend(
            System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(typeName));
        Noesis.Error.Check();

        return nativeType;
    }

    #region Imports
    [DllImport(Library.Name)]
    static extern IntPtr Noesis_FontProviderExtend(IntPtr typeName);

    [DllImport(Library.Name)]
    static extern void Noesis_FontProviderSetCallback(IntPtr cPtr, OpenFontCallback callback);

    [DllImport(Library.Name)]
    static extern void Noesis_FontProviderCopyBuffer(IntPtr cPtr, IntPtr stream, byte[] buffer,
        int bufferSize);
    #endregion
}
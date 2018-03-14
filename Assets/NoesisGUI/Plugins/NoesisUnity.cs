using Noesis;
using UnityEngine;
using System.Runtime.InteropServices;

public class NoesisUnity
{
    private static bool _initialized = false;
    static NoesisSettings _settings;

    public static void Init()
    {
        if (!_initialized)
        {
            _initialized = true;

            Noesis.GUI.Init();

            // Cache this because Unity is crashing internally if we access NoesisSettings from C++ callbacks
            _settings = NoesisSettings.Get();

            Noesis.GUI.SoftwareKeyboard = new UnitySoftwareKeyboard();
            RegisterProviders();
            RegisterLog();
            LoadApplicationResources();
        }
    }

    private static void LoadApplicationResources()
    {
        try
        {
            if (_settings.applicationResources != null)
            {
                ResourceDictionary resources = _settings.applicationResources.Load() as ResourceDictionary;
                if (resources != null)
                {
                    Noesis.GUI.SetApplicationResources(resources);
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Invalid Application Resources in settings");
                }
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }
    }

    private static void RegisterProviders()
    {
        Noesis.GUI.SetXamlProvider(NoesisXamlProvider.instance);
        Noesis.GUI.SetTextureProvider(NoesisTextureProvider.instance);
        Noesis.GUI.SetFontProvider(NoesisFontProvider.instance);
    }

    public static bool HasFamily(System.IO.Stream stream, string family)
    {
        bool hasFamily = Noesis_HasFamily(Extend.GetInstanceHandle(stream).Handle, family);
        Error.Check();

        return hasFamily;
    }

    #region Log management
    private static void RegisterLog()
    {
        Noesis_RegisterUnityLogCallbacks(_unityLog, _unityVerbosity);
#if UNITY_EDITOR
        System.AppDomain.CurrentDomain.DomainUnload += (sender, e) =>
        {
            Noesis_RegisterUnityLogCallbacks(null, null);
        };
#endif
    }

    private delegate void UnityLogCallback(int level, [MarshalAs(UnmanagedType.LPWStr)]string message);
    private static UnityLogCallback _unityLog = UnityLog;

    [MonoPInvokeCallback(typeof(UnityLogCallback))]
    private static void UnityLog(int level, string message)
    {
        switch ((LogLevel)level)
        {
            case LogLevel.Trace:
            case LogLevel.Debug:
            case LogLevel.Info:
            {
                Debug.Log("[NOESIS] " + message);
                break;
            }
            case LogLevel.Warning:
            {
                Debug.LogWarning("[NOESIS] " + message);
                break;
            }
            case LogLevel.Error:
            {
                Debug.LogError("[NOESIS] " + message);
                break;
            }
            default: break;
        }
    }

    private delegate int UnityVerbosityCallback();
    private static UnityVerbosityCallback _unityVerbosity = UnityVerbosity;

    [MonoPInvokeCallback(typeof(UnityVerbosityCallback))]
    private static int UnityVerbosity()
    {
        return (int)_settings.logVerbosity;
    }
    #endregion

    #region Imports
    [DllImport(Library.Name)]
    static extern void Noesis_RegisterUnityLogCallbacks(UnityLogCallback logCallback,
        UnityVerbosityCallback verbosityCallback);

    [DllImport(Library.Name)]
    static extern bool Noesis_HasFamily(System.IntPtr stream, string family);

    #endregion
}
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public enum MonoBehaviourCallback
{
    Update,
    LateUpdate,
    FixedUpdate,
    OnApplicationFocus,
    OnApplicationPause,
    OnApplicationQuit,
    OnDrawGizmos,
    OnGUI,
    OnPreCull,
    OnPreRender,
    OnPostRender,
}

/// <summary>
/// Static class to summarize the runtime platform information
/// </summary>
public static class Runtime
{

    public enum Platform
    {
        Windows,
        OSX,
        Linux,
        Android,
        IOS,
        WindowsStore,
        Playstation4,
        XboxOne,
        PSVita,
        WiiU
    }


    /// <summary>
    /// The total number of platforms in the Platform enum.
    /// </summary>
    public static int NumPlatforms
    {
        get
        {
            if (_numPlatforms == null)
            {
                _numPlatforms = EnumUtils.GetCount<Platform>();
            }
            return _numPlatforms.Value;
        }
    }

    /// <summary>
    /// The current runtime platform.
    /// </summary>
    public static Platform CurrentPlatform
    {
        get
        {
            if (_currentPlatform == null)
            {

                switch (Application.platform)
                {
                    case RuntimePlatform.WSAPlayerARM:
                    case RuntimePlatform.WSAPlayerX64:
                    case RuntimePlatform.WSAPlayerX86:
                        _currentPlatform = Platform.WindowsStore;
                        break;
                    case RuntimePlatform.PS4:
                        _currentPlatform = Platform.Playstation4;
                        break;
                    case RuntimePlatform.XboxOne:
                        _currentPlatform = Platform.XboxOne;
                        break;
                    case RuntimePlatform.WiiU:
                        _currentPlatform = Platform.WiiU;
                        break;
                    case RuntimePlatform.PSP2:
                        _currentPlatform = Platform.PSVita;
                        break;
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.WindowsPlayer:
                        _currentPlatform = Platform.Windows;
                        break;
                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.OSXPlayer:
                        _currentPlatform = Platform.OSX;
                        break;
                    case RuntimePlatform.LinuxPlayer:
                        _currentPlatform = Platform.Linux;
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        _currentPlatform = Platform.IOS;
                        break;
                    case RuntimePlatform.Android:
                        _currentPlatform = Platform.Android;
                        break;
                    default:
                        Debug.LogError("Current platform unsupported");
                        _currentPlatform = Platform.Windows;
                        break;
                }
            }

            return _currentPlatform.Value;
        }
    }


    /// <summary>
    /// Whether or not the game is running on Windows.
    /// </summary>
    public static bool IsWindows { get { return _currentPlatform == Platform.Windows; } }

    /// <summary>
    /// Whether or not the game is running on OSX.
    /// </summary>
    public static bool IsOSX { get { return _currentPlatform == Platform.OSX; } }

    /// <summary>
    /// Whether or not the game is running on Linux.
    /// </summary>
    public static bool IsLinux { get { return _currentPlatform == Platform.Linux; } }

    /// <summary>
    /// Whether or not the game is running on Android.
    /// </summary>
    public static bool IsAndroid { get { return _currentPlatform == Platform.Android; } }

    /// <summary>
    /// Whether or not the game is running on IOS.
    /// </summary>
    public static bool IsIOS { get { return _currentPlatform == Platform.IOS; } }

    /// <summary>
    /// Whether or not the game is running in the Unity Editor.
    /// </summary>
    public static bool IsEditor { get { return Application.isEditor; } }

    /// <summary>
    /// Whether or not the game is running a debug configuration.
    /// </summary>
    public static bool IsDebug { get { return Debug.isDebugBuild; } }

    /// <summary>
    /// Whether or not the game is running in WebGL.
    /// </summary>
    public static bool IsWebGL { get { return Application.platform == RuntimePlatform.WebGLPlayer; } }

    /// <summary>
    /// Whether or not the game is running on a standalone platform.
    /// </summary>
    public static bool IsStandalone { get { return (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer); } }

    /// <summary>
    /// Whether or not the game is running on a mobile platform.
    /// </summary>
    public static bool IsMobile { get { return _currentPlatform == Platform.Android || _currentPlatform == Platform.IOS || _currentPlatform == Platform.PSVita; } }

    /// <summary>
    /// Whether or not the game is running on a console.
    /// </summary>
    public static bool IsConsole { get { return _currentPlatform == Platform.Playstation4 || _currentPlatform == Platform.XboxOne || _currentPlatform == Platform.WiiU; } }

    public static bool IsPlaying { get { return Application.isPlaying && !_isShuttingDown; } }
    public static bool IsShuttingDown { get { return _isShuttingDown; } }
    public static bool StartedInThisSceneFromEditor { get { return IsEditor && _startingScene.IsValid(); } }


    private static Platform? _currentPlatform;
    private static int? _numPlatforms;
    private static Scene _startingScene;
    private static RuntimeUpdatePump _updatePump;
    private static bool _isShuttingDown;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        _startingScene = SceneManager.GetActiveScene();
        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        _updatePump = new GameObject("Runtime Update Pump").AddComponent<RuntimeUpdatePump>();
        _updatePump.delegates[(int)MonoBehaviourCallback.OnApplicationQuit] += OnApplicationQuit;
    }

    static void OnActiveSceneChanged(Scene fromScene, Scene toScene)
    {
        if (_startingScene.IsValid() && _startingScene != toScene)
        {
            _startingScene = new Scene();
        }
    }

    static void OnApplicationQuit()
    {
        _isShuttingDown = true;
    }

    /// <summary>
    /// Register a method to be called during update. DON'T FORGET TO DEREGISTER.
    /// </summary>
    /// <param name="method">The method to call</param>
    /// <param name="type">The update type to call the method in</param>
    public static void Register(Action method, MonoBehaviourCallback type = MonoBehaviourCallback.Update)
    {
        if (_updatePump != null)
        {
            _updatePump.delegates[(int)type] += method;
        }
    }

    /// <summary>
    /// Deregisters a previously registered update method.
    /// </summary>
    /// <param name="method">The method to deregister</param>
    public static void Deregister(Action method, MonoBehaviourCallback type = MonoBehaviourCallback.Update)
    {
        if (_updatePump != null)
        {
            _updatePump.delegates[(int)type] -= method;
        }
    }

    private class RuntimeUpdatePump : MonoBehaviour
    {
        public Action[] delegates = new Action[EnumUtils.GetCount<MonoBehaviourMethodType>()];

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        void Update()
        {
            if (delegates[(int)MonoBehaviourMethodType.Update] != null)
            {
                delegates[(int)MonoBehaviourMethodType.Update].Invoke();
            }
        }

        void LateUpdate()
        {

            if (delegates[(int)MonoBehaviourMethodType.LateUpdate] != null)
            {
                delegates[(int)MonoBehaviourMethodType.LateUpdate].Invoke();
            }
        }

        void OnApplicationFocus()
        {
            if (delegates[(int)MonoBehaviourMethodType.OnApplicationFocus] != null)
            {
                delegates[(int)MonoBehaviourMethodType.OnApplicationFocus].Invoke();
            }
        }

        void OnApplicationPause()
        {
            if (delegates[(int)MonoBehaviourMethodType.OnApplicationPause] != null)
            {
                delegates[(int)MonoBehaviourMethodType.OnApplicationPause].Invoke();
            }
        }
        void OnApplicationQuit()
        {

            if (delegates[(int)MonoBehaviourMethodType.OnApplicationQuit] != null)
            {
                delegates[(int)MonoBehaviourMethodType.OnApplicationQuit].Invoke();
            }
        }

        void OnDrawGizmos()
        {
            if (delegates[(int)MonoBehaviourMethodType.OnDrawGizmos] != null)
            {
                delegates[(int)MonoBehaviourMethodType.OnDrawGizmos].Invoke();
            }
        }

        void OnGUI()
        {
            if (delegates[(int)MonoBehaviourMethodType.OnGUI] != null)
            {
                delegates[(int)MonoBehaviourMethodType.OnGUI].Invoke();
            }
        }


        void OnPreCull()
        {
            if (delegates[(int)MonoBehaviourMethodType.OnPreCull] != null)
            {
                delegates[(int)MonoBehaviourMethodType.OnPreCull].Invoke();
            }
        }

        void OnPreRender()
        {
            if (delegates[(int)MonoBehaviourMethodType.OnPreRender] != null)
            {
                delegates[(int)MonoBehaviourMethodType.OnPreRender].Invoke();
            }
        }

        void OnPostRender()
        {
            if (delegates[(int)MonoBehaviourMethodType.OnPostRender] != null)
            {
                delegates[(int)MonoBehaviourMethodType.OnPostRender].Invoke();
            }
        }
    }


    /* Adapted from https://github.com/nickgravelyn/UnityToolbag */

    public static string GetSaveDirectory(bool includeCompanyName = false)
    {
        string path;

        if (IsWindows)
        {
            path = GetWindowsPath("Saves", includeCompanyName);
        }
        else if (IsOSX)
        {
            path = GetOSXApplicationSupportPath("Saves", includeCompanyName);
        }
        else if (IsLinux)
        {
            path = GetLinuxSaveDirectory(includeCompanyName);
        }
        else
        {
            path = Application.persistentDataPath;
        }

        Directory.CreateDirectory(path);
        return path;
    }

    public static string GetConfigDirectory(bool includeCompanyName = false)
    {
        string path;

        if (IsWindows)
        {
            path = GetWindowsPath("Config", includeCompanyName);
        }
        else if (IsOSX)
        {
            path = GetOSXApplicationSupportPath("Config", includeCompanyName);
        }
        else if (IsLinux)
        {
            path = GetLinuxConfigDirectory(includeCompanyName);
        }
        else
        {
            path = Application.persistentDataPath;
        }

        Directory.CreateDirectory(path);
        return path;
    }

    public static string GetLogDirectory(bool includeCompanyName = false)
    {
        string path;

        if (IsWindows)
        {
            path = GetWindowsPath("Logs", includeCompanyName);
        }
        else if (IsOSX)
        {
            path = GetOSXLogsPath(includeCompanyName);
        }
        else if (IsLinux)
        {
            path = GetLinuxLogDirectory(includeCompanyName);
        }
        else
        {
            path = Application.persistentDataPath;
        }

        Directory.CreateDirectory(path);
        return path;
    }

    private static string AppendProductPath(string path, bool includeCompanyName)
    {
        if (includeCompanyName)
        {
            path = AppendDirectory(path, Application.companyName);
        }

        return AppendDirectory(path, Application.productName);
    }

    private static string AppendDirectory(string path, string dir)
    {
        if (string.IsNullOrEmpty(dir)) return path;

        char[] invalidCharacters = Path.GetInvalidFileNameChars();
        StringBuilder cleanDir = new StringBuilder();

        for (int i = 0; i < dir.Length; i++)
        {
            char c = dir[i];
            bool clean = true;
            for (int j = 0; j < invalidCharacters.Length; j++)
            {
                if (c == invalidCharacters[j])
                {
                    clean = false;
                    break;
                }
            }

            if (clean)
            {
                cleanDir.Append(c);
            }
        }

        return Path.Combine(path, cleanDir.ToString());
    }

    public static string GetWindowsPath(string subdirectory, bool includeCompanyName)
    {
        string result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games");
        result = AppendProductPath(result, includeCompanyName);
        return AppendDirectory(result, subdirectory);
    }

    public static string GetOSXApplicationSupportPath(string subdirectory, bool includeCompanyName)
    {
        string result = Path.Combine(Environment.GetEnvironmentVariable("HOME"), "Library/Application Support");
        result = AppendProductPath(result, includeCompanyName);
        return AppendDirectory(result, subdirectory);
    }

    public static string GetOSXLogsPath(bool includeCompanyName)
    {
        string result = Path.Combine(Environment.GetEnvironmentVariable("HOME"), "Library/Logs");
        return AppendProductPath(result, includeCompanyName);
    }

    public static string GetLinuxSaveDirectory(bool includeCompanyName)
    {
        string result = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
        if (string.IsNullOrEmpty(result))
        {
            string home = Environment.GetEnvironmentVariable("HOME");
            result = Path.Combine(home, ".local/share");
        }

        return AppendProductPath(result, includeCompanyName);
    }

    public static string GetLinuxConfigDirectory(bool includeCompanyName)
    {
        string result = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
        if (string.IsNullOrEmpty(result))
        {
            string home = Environment.GetEnvironmentVariable("HOME");
            result = Path.Combine(home, ".config");
        }

        return AppendProductPath(result, includeCompanyName);
    }

    public static string GetLinuxLogDirectory(bool includeCompanyName)
    {
        return AppendProductPath("/var/log", includeCompanyName);
    }


}


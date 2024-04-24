using Godot;
using Godot.Collections;
using System.Text;

public partial class GameSettings : Node
{
    public enum AntiAliasingModeEnum : long
    {
        DISABLED = 0,
        FXAA = 1,
        MSAA2 = 2,
        MSAA4 = 3,
        MSAA8 = 4,
        TAA = 5
    }

    private const string SETTINGS_FILE_PATH = "./Settings.ini";

    public static GameSettings Instance { get; private set; }
    public bool HasDoneSetup { get; private set; }

    private float _sensitivity;
    private float _masterVolume, _sfxVolume, _playerVolume, _ambientVolume;
    private DisplayServer.WindowMode _windowMode;
    private bool _ssao, _ssr, _ssil, _sdfgi, _bloom;
    private AntiAliasingModeEnum _antiAliasingMode;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Instance.IsValid() == false)
        { Instance = this; }
        ReadFromFile();
        if (HasDoneSetup == true)
        {
            ApplySettings();
        }
    }

    /// <summary>Reads from file and sets the setting values. to apply these settings, call <see cref="ApplySettings"/></summary>
    public void ReadFromFile()
    {
        SetDefaults();//always do this, for missing values
        //read data from ini file, set variables in here
        if (FileAccess.FileExists(SETTINGS_FILE_PATH) == false)
        {
            HasDoneSetup = true;
            return;
        }

        FileAccess saveGame = FileAccess.Open(SETTINGS_FILE_PATH, FileAccess.ModeFlags.Read);
        GD.Print(saveGame.GetAsText());

        Dictionary<string, string> dict = new Dictionary<string, string>();
        //get all pairs
        string[] pairs = saveGame.GetAsText().Split("\n");

        foreach (string pair in pairs)
        {//put in dictionary if existing value
            string[] keyValue = pair.Trim().Split('=', 2);
            if (keyValue.Length == 2)
            {
                dict.Add(keyValue[0], keyValue[1]);
            }
        }

        SetFloat(dict, "Sensitivity", ref _sensitivity);
        SetFloat(dict, "MasterVolume", ref _masterVolume);
        SetFloat(dict, "SfxVolume", ref _sfxVolume);
        SetFloat(dict, "PlayerVolume", ref _playerVolume);
        SetFloat(dict, "AmbientVolume", ref _ambientVolume);
        SetWindowMode(dict, "WindowMode");
        SetBool(dict, "Ssao", ref _ssao);
        SetBool(dict, "Ssr", ref _ssr);
        SetBool(dict, "Ssil", ref _ssil);
        SetBool(dict, "Sdfgi", ref _sdfgi);
        SetBool(dict, "Bloom", ref _bloom);
        SetAntiAliasingMode(dict, "AntiAliasingMode");

        HasDoneSetup = true;
    }

    /// <summary>Writes all setting values to ini file</summary>
    public void SaveToFile()
    {
        //write data to ini file
        FileAccess settings = FileAccess.Open(SETTINGS_FILE_PATH, FileAccess.ModeFlags.Write);
        StringBuilder str = new StringBuilder();
        str.AppendLine("Sensitivity=" + _sensitivity);
        str.AppendLine("MasterVolume=" + _masterVolume);
        str.AppendLine("SfxVolume=" + _sfxVolume);
        str.AppendLine("PlayerVolume=" + _playerVolume);
        str.AppendLine("AmbientVolume=" + _ambientVolume);
        str.AppendLine("WindowMode=" + WindowModeIndex());
        str.AppendLine("Ssao=" + _ssao);
        str.AppendLine("Ssr=" + _ssr);
        str.AppendLine("Ssil=" + _ssil);
        str.AppendLine("Sdfgi=" + _sdfgi);
        str.AppendLine("Bloom=" + _bloom);
        str.AppendLine("AntiAliasingMode=" + (int)_antiAliasingMode);
        settings.StoreString(str.ToString());
        GD.Print("saved settings to: " + settings.GetPath());
        settings.Close();
    }

    /// <summary>Applies all the changes made to the setting values. to save these settings, call <see cref="SaveToFile"/></summary>
    public void ApplySettings()
    {
        //audio
        int busIndex = AudioServer.GetBusIndex("Master");
        SetVolumeDB(busIndex, _masterVolume);
        busIndex = AudioServer.GetBusIndex("Player");
        SetVolumeDB(busIndex, _playerVolume);
        busIndex = AudioServer.GetBusIndex("SFX");
        SetVolumeDB(busIndex, _sfxVolume);
        busIndex = AudioServer.GetBusIndex("Ambient");
        SetVolumeDB(busIndex, _ambientVolume);
        //graphics
        EnvironmentSettings.Instance?.AdjustSettings();
        SetAntiAliasing();
        SetWindowedMode();
        //gameplay
        if (Player.Instance != null)
        {
            Player.Instance.Sensitivity = _sensitivity;
        }
    }

    private void SetVolumeDB(int busIndex, float value)
    {
        AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(value));
    }

    private void SetAntiAliasing()
    {
        Viewport view = GetViewport();
        switch (_antiAliasingMode)
        {
            case AntiAliasingModeEnum.DISABLED:
            default:
                view.UseTaa = false;
                view.ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;
                view.Msaa3D = Viewport.Msaa.Disabled;
                break;
            case AntiAliasingModeEnum.FXAA:
                view.UseTaa = false;
                view.ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Max;
                view.Msaa3D = Viewport.Msaa.Disabled;
                break;
            case AntiAliasingModeEnum.MSAA2:
                view.UseTaa = false;
                view.ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;
                view.Msaa3D = Viewport.Msaa.Msaa2X;
                break;
            case AntiAliasingModeEnum.MSAA4:
                view.UseTaa = false;
                view.ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;
                view.Msaa3D = Viewport.Msaa.Msaa4X;
                break;
            case AntiAliasingModeEnum.MSAA8:
                view.UseTaa = false;
                view.ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;
                view.Msaa3D = Viewport.Msaa.Msaa8X;
                break;
            case AntiAliasingModeEnum.TAA:
                view.ScreenSpaceAA = Viewport.ScreenSpaceAAEnum.Disabled;
                view.Msaa3D = Viewport.Msaa.Disabled;
                view.UseTaa = true;
                break;
        }
    }

    private void SetWindowedMode()
    {
        DisplayServer.WindowSetMode(_windowMode, 0);
    }

    private void SetDefaults()
    {//all default values used in development
        _sensitivity = 0.5f;
        _masterVolume = 1;
        _sfxVolume = 1;
        _playerVolume = 1;
        _ambientVolume = 1;
        _windowMode = DisplayServer.WindowGetMode(0);
        _ssao = true;
        _ssr = true;
        _ssil = true;
        _sdfgi = true;
        _bloom = true;
        _antiAliasingMode = AntiAliasingModeEnum.TAA;
    }

    private void SetFloat(Dictionary<string, string> dict, string key, ref float val)
    {
        if (dict.ContainsKey(key))
        {
            if (float.TryParse(dict[key], out float res))
            {
                GD.Print($"parsed \"{key}\" with value: {val}");
                val = res;
            }
        }
    }

    private void SetBool(Dictionary<string, string> dict, string key, ref bool val)
    {
        if (dict.ContainsKey(key))
        {
            if (bool.TryParse(dict[key], out bool res))
            {
                GD.Print("parsed " + key);
                val = res;
            }
        }
    }

    private void SetWindowMode(Dictionary<string, string> dict, string key)
    {
        if (dict.ContainsKey(key))
        {
            if (int.TryParse(dict[key], out int res))
            {
                GD.Print("parsed " + key);
                switch (res)
                {
                    case 0://exclusive fullscreen
                        _windowMode = DisplayServer.WindowMode.ExclusiveFullscreen;
                        return;
                    case 1://windowed fullscreen
                        _windowMode = DisplayServer.WindowMode.Fullscreen;
                        return;
                    case 2://windowed
                        _windowMode = DisplayServer.WindowMode.Windowed;
                        return;
                }
            }
        }
    }

    private void SetAntiAliasingMode(Dictionary<string, string> dict, string key)
    {
        if (dict.ContainsKey(key))
        {
            if (int.TryParse(dict[key], out int res))
            {
                GD.Print("parsed " + key);
                _antiAliasingMode = (AntiAliasingModeEnum)res;
            }
        }
    }

    public int WindowModeIndex()
    {
        switch (WindowMode)
        {
            case DisplayServer.WindowMode.ExclusiveFullscreen://exclusive fullscreen
            default:
                return 0;
            case DisplayServer.WindowMode.Fullscreen://windowed fullscreen
                return 1;
            case DisplayServer.WindowMode.Windowed://windowed
                return 2;
        }
    }

    public float Sensitivity { get => _sensitivity; set => _sensitivity = value; }

    public float MasterVolume { get => _masterVolume; set => _masterVolume = value; }
    public float SfxVolume { get => _sfxVolume; set => _sfxVolume = value; }
    public float PlayerVolume { get => _playerVolume; set => _playerVolume = value; }
    public float AmbientVolume { get => _ambientVolume; set => _ambientVolume = value; }

    public DisplayServer.WindowMode WindowMode { get => _windowMode; set => _windowMode = value; }
    public bool Ssao { get => _ssao; set => _ssao = value; }
    public bool Ssr { get => _ssr; set => _ssr = value; }
    public bool Ssil { get => _ssil; set => _ssil = value; }
    public bool Sdfgi { get => _sdfgi; set => _sdfgi = value; }
    public bool Bloom { get => _bloom; set => _bloom = value; }
    public AntiAliasingModeEnum AntiAliasingMode { get => _antiAliasingMode; set => _antiAliasingMode = value; }
}
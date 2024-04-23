using Godot;
using System;

public partial class OptionsUI : Control
{
    public const string TREE = "/root/GameUI/Non-main menu/pauseMenu/Options";

    [Export] private Button GoBackButton, ApplyButton;
    [ExportGroup("Audio")]
    [Export] private HSlider MasterVolumeSlider;
    [Export] private Label MasterVolumeLabel;
    [Export] private HSlider SFXVolumeSlider;
    [Export] private Label SFXVolumeLabel;
    [Export] private HSlider PlayerVolumeSlider;
    [Export] private Label PlayerVolumeLabel;
    [Export] private HSlider AmbientVolumeSlider;
    [Export] private Label AmbientVolumeLabel;
    [ExportGroup("Graphics")]
    [Export] private OptionButton FullScreenOptions, AntiAliasingOptions;
    [Export] private CheckBox SSAOCheck, SSRCheck, SSILCheck, SDFGICheck, BloomCheck;
    [ExportGroup("Gameplay")]
    [Export] private HSlider SensitivitySlider;
    [Export] private Label SensitivityLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GoBackButton.Pressed += GoBackButton_Pressed;
        ApplyButton.Pressed += ApplyButton_Pressed;
        //audio
        MasterVolumeSlider.ValueChanged += MasterVolumeSlider_ValueChanged;
        SFXVolumeSlider.ValueChanged += SFXVolumeSlider_ValueChanged;
        PlayerVolumeSlider.ValueChanged += PlayerVolumeSlider_ValueChanged;
        AmbientVolumeSlider.ValueChanged += AmbientVolumeSlider_ValueChanged;
        //graphics
        FullScreenOptions.ItemSelected += FullScreenOptions_ItemSelected;
        AntiAliasingOptions.ItemSelected += AntiAliasingOptions_ItemSelected;
        SSAOCheck.Toggled += SSAOCheck_Toggled;
        SSRCheck.Toggled += SSRCheck_Toggled;
        SSILCheck.Toggled += SSILCheck_Toggled;
        SDFGICheck.Toggled += SDFGICheck_Toggled;
        BloomCheck.Toggled += BloomCheck_Toggled;
        //gameplay
        SensitivitySlider.ValueChanged += SensitivitySlider_ValueChanged;

        GD.Print("GameSettings.HasDoneSetup: " + GameSettings.Instance.HasDoneSetup);
        if (GameSettings.Instance.HasDoneSetup == true)//set all values in options
        {
            //audio
            MasterVolumeSlider.Value = GameSettings.Instance.MasterVolume * 100;
            SFXVolumeSlider.Value = GameSettings.Instance.SfxVolume * 100;
            PlayerVolumeSlider.Value = GameSettings.Instance.PlayerVolume * 100;
            AmbientVolumeSlider.Value = GameSettings.Instance.AmbientVolume * 100;
            //graphics
            FullScreenOptions.Select(GameSettings.Instance.WindowModeIndex());
            AntiAliasingOptions.Select((int)GameSettings.Instance.AntiAliasingMode);
            SSAOCheck.ButtonPressed = GameSettings.Instance.Ssao;
            SSRCheck.ButtonPressed = GameSettings.Instance.Ssr;
            SSILCheck.ButtonPressed = GameSettings.Instance.Ssil;
            SDFGICheck.ButtonPressed = GameSettings.Instance.Sdfgi;
            BloomCheck.ButtonPressed = GameSettings.Instance.Bloom;
            //gameplay
            SensitivitySlider.Value = GameSettings.Instance.Sensitivity;
        }
    }
    public override void _ExitTree()
    {
        GoBackButton.Pressed -= GoBackButton_Pressed;
        ApplyButton.Pressed -= ApplyButton_Pressed;
        //audio
        MasterVolumeSlider.ValueChanged -= MasterVolumeSlider_ValueChanged;
        SFXVolumeSlider.ValueChanged -= SFXVolumeSlider_ValueChanged;
        PlayerVolumeSlider.ValueChanged -= PlayerVolumeSlider_ValueChanged;
        AmbientVolumeSlider.ValueChanged -= AmbientVolumeSlider_ValueChanged;
        //graphics
        FullScreenOptions.ItemSelected -= FullScreenOptions_ItemSelected;
        AntiAliasingOptions.ItemSelected -= AntiAliasingOptions_ItemSelected;
        SSAOCheck.Toggled -= SSAOCheck_Toggled;
        SSRCheck.Toggled -= SSRCheck_Toggled;
        SSILCheck.Toggled -= SSILCheck_Toggled;
        SDFGICheck.Toggled -= SDFGICheck_Toggled;
        BloomCheck.Toggled -= BloomCheck_Toggled;
        //gameplay
        SensitivitySlider.ValueChanged -= SensitivitySlider_ValueChanged;
        base._ExitTree();
    }

    #region Buttons
    private void ApplyButton_Pressed()
    {
        GameSettings.Instance.ApplySettings();
        GameSettings.Instance.SaveToFile();
    }

    private void GoBackButton_Pressed()
    {
        this.Visible = false;
    }
    #endregion

    #region Gameplay
    private void SensitivitySlider_ValueChanged(double value)
    {
        SensitivityLabel.Text = value.ToString("0.00");
        GameSettings.Instance.Sensitivity = (float)value;
    }
    #endregion
    #region graphics
    private void BloomCheck_Toggled(bool toggledOn)
    {
        GameSettings.Instance.Bloom = toggledOn;
    }

    private void SDFGICheck_Toggled(bool toggledOn)
    {
        GameSettings.Instance.Sdfgi = toggledOn;
    }

    private void SSILCheck_Toggled(bool toggledOn)
    {
        GameSettings.Instance.Ssil = toggledOn;
    }

    private void SSRCheck_Toggled(bool toggledOn)
    {
        GameSettings.Instance.Ssr = toggledOn;
    }

    private void SSAOCheck_Toggled(bool toggledOn)
    {
        GameSettings.Instance.Ssao = toggledOn;
    }

    private void AntiAliasingOptions_ItemSelected(long index)
    {
        GameSettings.Instance.AntiAliasingMode = (GameSettings.AntiAliasingModeEnum)index;
    }

    private void FullScreenOptions_ItemSelected(long index)
    {
        switch (index)
        {
            case 0://exclusive fullscreen
                GameSettings.Instance.WindowMode = DisplayServer.WindowMode.ExclusiveFullscreen;
                return;
            case 1://windowed fullscreen
                GameSettings.Instance.WindowMode = DisplayServer.WindowMode.Fullscreen;
                return;
            case 2://windowed
                GameSettings.Instance.WindowMode = DisplayServer.WindowMode.Windowed;
                return;
        }
    }
    #endregion
    #region Audio settings
    private void PlayerVolumeSlider_ValueChanged(double value)
    {
        PlayerVolumeLabel.Text = ((int)value).ToString();
        GD.Print(value);
        GameSettings.Instance.PlayerVolume = (float)(value * 0.01);
    }

    private void SFXVolumeSlider_ValueChanged(double value)
    {
        SFXVolumeLabel.Text = ((int)value).ToString();
        GameSettings.Instance.SfxVolume = (float)(value * 0.01);
    }

    private void MasterVolumeSlider_ValueChanged(double value)
    {
        MasterVolumeLabel.Text = ((int)value).ToString();
        GameSettings.Instance.MasterVolume = (float)(value * 0.01);
    }
    private void AmbientVolumeSlider_ValueChanged(double value)
    {
        AmbientVolumeLabel.Text = ((int)value).ToString();
        GameSettings.Instance.AmbientVolume = (float)(value * 0.01);
    }
    #endregion
}

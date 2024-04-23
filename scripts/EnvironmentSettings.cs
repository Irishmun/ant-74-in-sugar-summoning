using Godot;
using System;

public partial class EnvironmentSettings : WorldEnvironment
{
    public static EnvironmentSettings Instance { get; private set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
        if (GameSettings.Instance.HasDoneSetup == true)
        {
            AdjustSettings();
        }
    }

    public void AdjustSettings()
    {
        Environment.SsaoEnabled = GameSettings.Instance.Ssao;
        Environment.SsrEnabled = GameSettings.Instance.Ssr;
        Environment.SsilEnabled = GameSettings.Instance.Ssil;
        Environment.SdfgiEnabled = GameSettings.Instance.Sdfgi;
        Environment.GlowEnabled = GameSettings.Instance.Bloom;
        GD.PrintRich("environment settings:\n" +
            $"ssao:{Environment.SsaoEnabled}\n" +
            $"ssr:{Environment.SsrEnabled}\n" +
            $"ssil:{Environment.SsilEnabled}\n" +
            $"sdgi:{Environment.SdfgiEnabled}\n" +
            $"glow:{Environment.GlowEnabled}");
    }
}

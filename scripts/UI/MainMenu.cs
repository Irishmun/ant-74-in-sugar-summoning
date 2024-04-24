using Godot;
using System;

public partial class MainMenu : Control
{
    public const string GameUINonMenuNode = "/root/GameUI/Non-main menu";

    [ExportGroup("Start")]
    [Export] private Button StartButton;
    [Export] private string StartScene = "d1_awakeningwood_01";
    [ExportGroup("Settings")]
    [Export] private Button SettingsButton;
    [Export] private Control MenuButtons;
    [Export] private OptionsUI OptionsMenu;
    [ExportGroup("Quit")]
    [Export] private Button QuitButton;

    private SceneFade _sceneChange;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sceneChange = GetNode<SceneFade>(SceneFade.TREE);
        OptionsMenu.Visible = false;
        MenuButtons.Visible = true;

        GetNode<PauseMenu>(PauseMenu.TREE).ProcessMode = ProcessModeEnum.Disabled;

        //set button events
        StartButton.Pressed += StartButton_Pressed;
        SettingsButton.Pressed += SettingsButton_Pressed;
        QuitButton.Pressed += QuitButton_Pressed;

        StartButton.GrabFocus();
    }
    #region Button Events
    private void StartButton_Pressed()
    {
        GD.Print("Starting game");
        GetNode(GameUINonMenuNode).ProcessMode = ProcessModeEnum.Inherit;
        GetNode<PauseMenu>(PauseMenu.TREE).ProcessMode = ProcessModeEnum.Inherit;
        _sceneChange.SceneName = StartScene;
        _sceneChange.FadeToScene();
    }
    private void SettingsButton_Pressed()
    {
        OptionsMenu.BecomeVisible();
    }
    private void QuitButton_Pressed()
    {
        GD.Print("Quitting...");
        GetTree().Quit();
    }
    #endregion

}

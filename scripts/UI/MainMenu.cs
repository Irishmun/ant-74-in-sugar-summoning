using Godot;
using System;

public partial class MainMenu : Control
{
    [ExportGroup("Start")]
    [Export] private Button StartButton;
    [Export] private string StartScene = "d1_awakeningwood_01";
    [ExportGroup("Settings")]
    [Export] private Button SettingsButton;
    [Export] private Control OptionsMenu, MenuButtons;
    [ExportGroup("Quit")]
    [Export] private Button QuitButton;

    private SceneChangeUtil _sceneChange;
    private const string GameUINonMenuNode = "/root/GameUI/Non-main menu";


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sceneChange = GetNode<SceneChangeUtil>(SceneChangeUtil.SCENE_CHANGE_NODE_TREE);
        OptionsMenu.Visible = false;
        MenuButtons.Visible = true;

        
        //set button events
        StartButton.Pressed += StartButton_Pressed;
        SettingsButton.Pressed += SettingsButton_Pressed;
        QuitButton.Pressed += QuitButton_Pressed;
    }
    #region Button Events
    private void StartButton_Pressed()
    {
        GD.Print("Starting game");
        GetNode(GameUINonMenuNode).ProcessMode = ProcessModeEnum.Inherit;
        ChangeScene(StartScene);
    }
    private void SettingsButton_Pressed()
    {
        GD.Print("TODO: implement");
    }
    private void QuitButton_Pressed()
    {
        GetTree().Quit();
        return;
    }
    #endregion
    protected void ChangeScene(string SceneName)
    {
        GD.Print("Scene name: " + SceneName);
        if (string.IsNullOrWhiteSpace(SceneName) == true)
        { return; }
        _sceneChange.ChangeSceneNextFrame(SceneName);
    }
}

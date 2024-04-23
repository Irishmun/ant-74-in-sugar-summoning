using Godot;
using System;

public partial class SceneChangeUtil : Node
{
    private const string SCENE_FOLDER_PATH = "res://scenes//";

    public static readonly string TREE = "/root/SceneChangeUtil";

    public string SceneEntityID { get; set; }

    private LoadingUI _loadingUI;

    public override void _Ready()
    {
        _loadingUI = GetNode<LoadingUI>(LoadingUI.TREE);
    }

    public void ChangeSceneNextFrame(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName) == true)
        { return; }
        _loadingUI.SetLoadingUIVisibility(true);
        CallDeferred("ChangeScene", sceneName);
    }

    private async void ChangeScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName) == true)
        { return; }
        //waiting two physics steps, enough for the load image to show without severe annoyance I'd say.
        await ToSignal(GetTree().CreateTimer(GetPhysicsProcessDeltaTime() * 2), "timeout");
        GetTree().ChangeSceneToFile($"{SCENE_FOLDER_PATH}{sceneName}.tscn");
        _loadingUI.SetLoadingUIVisibility(false);
    }
}

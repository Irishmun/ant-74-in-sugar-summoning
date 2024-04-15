using Godot;
using System;

public partial class SceneFade : Node
{
    [Export] private AnimationPlayer animator;


    private SceneChangeUtil _sceneChange;
    public static string SCENE_FADE_TREE = "/root/GameUI/ScreenFade";

    private bool _isFading = false;

    public override void _Ready()
    {
        _sceneChange = GetNode<SceneChangeUtil>(SceneChangeUtil.SCENE_CHANGE_NODE_TREE);
    }

    public void FadeToScene()
    {
        GD.PrintRich("[wave]Fading to black...[/wave]");
        _isFading = true;
        animator.Play("Load/FadeToBlack");
    }

    public void FadeFromScene()
    {
        GD.PrintRich("[wave]Fading from black...[/wave]");
        _isFading = true;
        animator.Play("Load/FadeFromBlack");
    }

    public void ChangeToScene()
    {
        _sceneChange.ChangeSceneNextFrame(SceneName);
    }

    public void SetFadingTrue()
    {
        _isFading = true;
    }

    public void SetFadingFalse()
    {
        _isFading = false;
    }

    public string SceneName { get; set; }
    public bool IsFading => _isFading;
}

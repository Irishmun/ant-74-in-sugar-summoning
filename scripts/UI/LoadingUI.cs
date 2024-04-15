using Godot;
using System;

public partial class LoadingUI : Control
{
    public static readonly string LOADING_UI_TREE = "/root/GameUI/Loading";
    [Export] private TextureRect _loadingImage;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _loadingImage.Visible = false;
    }

    public void SetLoadingUIVisibility(bool visible)
    {
        GD.Print("setting visibility of loading UI to " + visible);
        _loadingImage.Visible = visible;
    }

    public override void _EnterTree()
    {
        SetLoadingUIVisibility(false);
    }
}

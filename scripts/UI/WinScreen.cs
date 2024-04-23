using Godot;
using System;

public partial class WinScreen : Node
{
    [Export] Button QuitButton, MenuButton;
    [Export] Label timeLabel;
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        GetNode<HudUI>(HudUI.TREE).Visible = false;
        GetNode<SceneFade>(SceneFade.TREE).FadeFromScene();

        QuitButton.Pressed += QuitButton_Pressed;
        MenuButton.Pressed += MenuButton_Pressed;
        timeLabel.Text = "Time: " + GameTimer.Instance.TimeString();
    }

    private void MenuButton_Pressed()
    {
        GameTimer.Instance.ResetTimer();
        SceneFade fade = GetNode<SceneFade>(SceneFade.TREE);
        GetNode<DialogUI>(DialogUI.TREE).Init();
        fade.SceneName = "d1_awakeningwood_01";
        fade.FadeToScene();        
    }

    private void QuitButton_Pressed()
    {
        GetTree().Quit();
    }
}

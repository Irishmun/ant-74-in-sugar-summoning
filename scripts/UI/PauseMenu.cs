using Godot;
using System;

public partial class PauseMenu : Control
{
    public const string TREE = "/root/GameUI/Non-main menu/pauseMenu";

    [Export] private Button ResumeButton, OptionsButton, QuitButton;
    [Export] private Control MainPauseNode;
    [Export] private OptionsUI OptionsNode;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ResumeButton.Pressed += ResumeButton_Pressed;
        OptionsButton.Pressed += SettingsButton_Pressed;
        QuitButton.Pressed += QuitButton_Pressed;
        this.Visible = false;
        OptionsNode.Visible = false;
    }

    public override void _UnhandledInput(InputEvent e)
    {
        if (e.IsActionPressed("Pause"))
        {
            GD.Print("pause key pressed");
            if (OptionsNode.Visible == true)
            {//we are in options mode
                GD.Print("hiding options");
                OptionsNode.Visible = false;
                return;
            }
            if (this.Visible == true)
            {
                GD.Print("unpausing");
                Input.MouseMode = Input.MouseModeEnum.Captured;
                this.Visible = false;
                GetTree().Paused = false;
                GameTimer.Instance.StartTimer();
            }
            else
            {
                GD.Print("pausing");
                this.Visible = true;
                //MainPauseNode.Visible = true;
                OptionsNode.Visible = false;
                Input.MouseMode = Input.MouseModeEnum.Visible;
                GameTimer.Instance.StopTimer();
                GetTree().Paused = true;
            }
        }
    }

    private void ResumeButton_Pressed()
    {
        GetTree().Paused = false;
        this.Visible = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
        GameTimer.Instance.StartTimer();
    }

    private void QuitButton_Pressed()
    {
        GD.PrintRich("[wave]Quitting...[/wave]");
        GetTree().Quit();
    }

    private void SettingsButton_Pressed()
    {
        GD.PrintRich("[shake]not implemented[/shake]");
        OptionsNode.Visible = true;
        //MainPauseNode.Visible = false;
    }


}

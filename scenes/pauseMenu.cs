using Godot;
using System;

public partial class pauseMenu : Control
{
    [Export] private Button ResumeButton, OptionsButton, QuitButton;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ResumeButton.Pressed += ResumeButton_Pressed;
        OptionsButton.Pressed += SettingsButton_Pressed;
        QuitButton.Pressed += QuitButton_Pressed;
        this.Visible = false;
    }

    public override void _UnhandledInput(InputEvent e)
    {
        if (e.IsActionPressed("Pause"))
        {
            this.Visible = true;
            Input.MouseMode = Input.MouseModeEnum.Visible;
            GameTimer.Instance.StopTimer();
            GetTree().Paused = true;
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
    }


}

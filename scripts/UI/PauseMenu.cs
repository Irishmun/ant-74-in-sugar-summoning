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

        if (GetTree().Paused == true)
        {
            ShowHideCursorIfMouse(e);
        }
        if (e.IsActionPressed("Pause"))
        {
            GD.Print("pause key pressed");
            if (OptionsNode.Visible == true)
            {//we are in options mode
                GD.Print("hiding options");
                MainPauseNode.Visible = true;
                OptionsButton.GrabFocus();
                OptionsNode.Visible = false;
                return;
            }
            if (this.Visible == true)
            {
                GD.Print("unpausing");
                ShowHideCursorIfMouse(e, false);
                this.Visible = false;
                GetTree().Paused = false;
                GameTimer.Instance.StartTimer();
            }
            else
            {
                GD.Print("pausing");
                this.Visible = true;
                //MainPauseNode.Visible = true;
                ResumeButton.GrabFocus();
                OptionsNode.Visible = false;
                ShowHideCursorIfMouse(e);
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
        //GD.PrintRich("[shake]not implemented[/shake]");
        OptionsNode.BecomeVisible();
        OptionsButton.ReleaseFocus();
        MainPauseNode.Visible = false;
        //OptionsNode.Visible = true;
    }

    private void ShowHideCursorIfMouse(InputEvent e, bool show = true)
    {
        if (e is InputEventJoypadButton || e is InputEventJoypadMotion)
        {//controller, hide mouse
            Input.MouseMode = Input.MouseModeEnum.Captured;
            return;
        }
        if (show == true)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            return;
        }
        Input.MouseMode = Input.MouseModeEnum.Captured;
        return;
    }
}

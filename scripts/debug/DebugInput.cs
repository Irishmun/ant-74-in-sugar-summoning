using Godot;
using System;

public partial class DebugInput : Node
{
    //private SignalBus _signalBus;

    private bool _consoleVisible = false;
    public override void _UnhandledInput(InputEvent e)
    {
        if (e.IsActionPressed("Pause"))
        {
            GetTree().Quit();
            return;
        }
    }
}

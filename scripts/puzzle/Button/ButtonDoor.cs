using Godot;
using System;

public partial class ButtonDoor : ButtonInteractable
{
    [Export] private bool StartOpen = false;
    private bool _open = false;
    private Vector3 _startPos;

    public override void _Ready()
    {
        _open = StartOpen;
        _startPos = GlobalPosition;
    }
    public override void OnButtonPressed()
    {
        //open door
        if (_open == false)
        {
            OpenDoor(true);
        }
    }

    public override void OnButtonReleased()
    {
        //close door
        if (_open == true)
        {
            OpenDoor(false);
        }
    }

    private void OpenDoor(bool open)
    {
        _open = open;
        if (open == true)
        {
            GlobalPosition = _startPos + Vector3.Up * 2;
        }
        else
        {
            GlobalPosition = _startPos;
        }
    }
}

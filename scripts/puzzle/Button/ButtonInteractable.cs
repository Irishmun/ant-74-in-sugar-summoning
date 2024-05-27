using Godot;
using System;

public abstract partial class ButtonInteractable : Node3D
{
    public abstract void OnButtonPressed();

    public abstract void OnButtonReleased();
}

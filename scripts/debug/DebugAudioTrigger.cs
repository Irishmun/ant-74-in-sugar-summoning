using Godot;
using System;

public partial class DebugAudioTrigger : Area3D
{
    [Export] private AudioStreamPlayer3D audio;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        this.BodyEntered += DebugAudioTrigger_BodyEntered;
	}

    private void DebugAudioTrigger_BodyEntered(Node3D body)
    {
        audio.Play();
    }
}

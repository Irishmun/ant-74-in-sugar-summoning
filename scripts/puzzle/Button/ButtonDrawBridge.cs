using Godot;
using System;

public partial class ButtonDrawBridge : ButtonInteractable
{
    [Export] private float RotationAngle;

    private float _activeRotation, _startRotation;
    private bool _isRotated;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _activeRotation = Mathf.DegToRad(RotationAngle);
        _startRotation = Rotation.Z;
    }

    public override void OnButtonPressed()
    {
        if (_isRotated == false)
        {
            Rotation = new Vector3(Rotation.X, Rotation.Y, _activeRotation);
            _isRotated = true;
        }
    }

    public override void OnButtonReleased()
    {
        if (_isRotated == true)
        {
            Rotation = new Vector3(Rotation.X, Rotation.Y, _startRotation);
            _isRotated = false;
        }
    }
}

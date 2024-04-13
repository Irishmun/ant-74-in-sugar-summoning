using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [ExportGroup("Nodes")]
    [Export] private Node3D CamRoot, Cam;
    [Export] private AnimationPlayer animator;
    [Export] private Node3D MeshRoot;
    [Export] private CoinStack CoinStack;
    [ExportGroup("Movement")]
    [Export] private float MovementSpeed = 1, RotationSpeed = 1;
    [ExportGroup("Camera")]
    [Export] private float ControllerDeadZone = 0.1f;
    [Export] private float ControllerMultiplier = 30f;
    [Export] private float LookSensitivity = 1f;
    [Export] private Vector2 MaxVerticalLook = new Vector2(-45, 20);//X is down, Y is up


    private bool _usingController = false;
    private bool _isHoldingCoins = false;
    private Vector2 _camMovement, _inputDir;
    private Vector3 _direction;
    private Vector3 _velocity;
    private float _acceleration = 1, _speed;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        CoinStack.Player = this;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_usingController == true)
        {
            RotateCamera();
        }


        if (IsVisibleMovement() == true)
        {
            if (_isHoldingCoins == true)
            {
                animator.Play("ant/walk_hold");

            }
            else
            {
                animator.Play("ant/walk_Forward");
            }
        }
        else
        {
            if (_isHoldingCoins == true)
            {
                animator.Play("ant/idle_hold");

            }
            else
            {
                animator.Play("ant/idle");
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsVisibleMovement() == false)
        { return; }

        _direction = new Vector3(_inputDir.X, 0, _inputDir.Y).Normalized();
        _direction = _direction.Rotated(Vector3.Up, CamRoot.GlobalRotation.Y);

        _velocity = new Vector3(MovementSpeed * _direction.X, 0, MovementSpeed * _direction.Z);

        Velocity = _velocity;//Velocity.Lerp(_velocity, _acceleration * (float)delta);
        MoveAndSlide();

        float targetRotation = Mathf.Atan2(_velocity.X, _velocity.Z) - GlobalRotation.Y;
        float lerpRotation = Mathf.LerpAngle(MeshRoot.GlobalRotation.Y, targetRotation, RotationSpeed * (float)delta);
        MeshRoot.GlobalRotation = new Vector3(MeshRoot.GlobalRotation.X, lerpRotation, MeshRoot.GlobalRotation.Z);
    }

    public override void _Input(InputEvent @event)
    {
        _inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");
    }

    public override void _UnhandledInput(InputEvent e)
    {
        if (e is InputEventMouseMotion)
        {
            _usingController = false;
            InputEventMouseMotion m = (InputEventMouseMotion)e;
            _camMovement = m.Relative;
            RotateCamera();
            _camMovement = Vector2.Zero;
        }
        else if (e is InputEventJoypadMotion)
        {
            _usingController = true;

            InputEventJoypadMotion m = (InputEventJoypadMotion)e;
            if (m.Axis == JoyAxis.RightX)
            {
                _camMovement.X = Mathf.Abs(m.AxisValue) > ControllerDeadZone ? m.AxisValue * ControllerMultiplier : 0;
            }
            else if (m.Axis == JoyAxis.RightY)
            {
                _camMovement.Y = Mathf.Abs(m.AxisValue) > ControllerDeadZone ? m.AxisValue * ControllerMultiplier : 0;
            }
        }
    }

    private void RotateCamera()
    {
        //RotateY //rotate node this script is attached to on the Y axis
        CamRoot.RotateY(Mathf.DegToRad(-_camMovement.X * LookSensitivity));
        Cam.RotateX(Mathf.DegToRad(-_camMovement.Y * LookSensitivity));
        Cam.RotationDegrees = new Vector3(Mathf.Clamp(Cam.RotationDegrees.X, MaxVerticalLook.X, MaxVerticalLook.Y), Cam.RotationDegrees.Y, Cam.RotationDegrees.Z);

    }

    private bool IsVisibleMovement() => Mathf.Abs(_inputDir.X) > 0 || Mathf.Abs(_inputDir.Y) > 0;
    public bool IsHoldingCoins { get => _isHoldingCoins; set => _isHoldingCoins = value; }
}

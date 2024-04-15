using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [ExportGroup("Nodes")]
    [Export] private Node3D CamRoot, Cam;
    [Export] private AnimationPlayer animator;
    [Export] private Node3D MeshRoot;
    [Export] private CoinStack CoinStack;
    [Export] private RayCast3D FloorCast;
    [ExportGroup("Movement")]
    [Export] private float MovementSpeed = 1, RotationSpeed = 1;
    [Export] private float RunMultiplier = 2;
    [Export] private float DistanceBeforeFall = 0.126f;
    [Export] private float Mass = 10;
    [ExportGroup("Camera")]
    [Export] private float ControllerDeadZone = 0.1f;
    [Export] private float ControllerMultiplier = 30f;
    [Export] private float LookSensitivity = 1f;
    [Export] private Vector2 MaxVerticalLook = new Vector2(-45, 20);//X is down, Y is up



    private bool _usingController = false;
    private bool _isHoldingCoins = false;
    private bool _isRunning = false;
    private bool _wasOnFloor = false;
    private Vector2 _camMovement, _inputDir;
    private Vector3 _direction;
    private Vector3 _velocity;
    private float _acceleration = 1, _speed;
    private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    private float _lastFallingSpeed;
    private bool _mayDoStuff = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        CoinStack.Player = this;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        _isRunning = Input.IsActionPressed("Run");
        if (_usingController == true)
        {
            RotateCamera();
        }


        if (IsVisibleMovement() == true && _mayDoStuff == true)
        {
            animator.SpeedScale = _isRunning == true ? RunMultiplier : 1;
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
            animator.SpeedScale = 1;
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
        bool onFloor = IsOnFloor();
        if (_mayDoStuff == false)
        { return; }
        _speed = _isRunning == true ? MovementSpeed * RunMultiplier : MovementSpeed;

        _direction = new Vector3(_inputDir.X, 0, _inputDir.Y).Normalized();
        _direction = _direction.Rotated(Vector3.Up, CamRoot.GlobalRotation.Y);

        _velocity = new Vector3(_speed * _direction.X, 0, _speed * _direction.Z);

        if (!onFloor)
        {
            if (_wasOnFloor == true)
            {
                if (_velocity.Y <= 0)
                {
                    if (FloorCast.GetCollider() != null)
                    {
                        float floorSnap = FloorSnapLength;
                        FloorSnapLength = DistanceBeforeFall;
                        ApplyFloorSnap();
                        FloorSnapLength = floorSnap;
                    }
                }
            }
            else
            {
                _velocity.Y -= _gravity * Mass * (float)delta;
            }
        }

        Velocity = _velocity;//Velocity.Lerp(_velocity, _acceleration * (float)delta);
        MoveAndSlide();

        _wasOnFloor = onFloor;//cache current frame onfloor state for next frame
        _lastFallingSpeed = -_velocity.Y;

        if (IsVisibleMovement() == false)
        { return; }
        float targetRotation = Mathf.Atan2(_velocity.X, _velocity.Z) - GlobalRotation.Y;
        float lerpRotation = Mathf.LerpAngle(MeshRoot.GlobalRotation.Y, targetRotation, RotationSpeed * (float)delta);
        MeshRoot.GlobalRotation = new Vector3(MeshRoot.GlobalRotation.X, lerpRotation, MeshRoot.GlobalRotation.Z);
    }

    public override void _Input(InputEvent e)
    {
        _inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");
    }

    public override void _UnhandledInput(InputEvent e)
    {
        if (_mayDoStuff == false)
        { return; }
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

    public bool MayDoStuff { get => _mayDoStuff; set => _mayDoStuff = value; }
}

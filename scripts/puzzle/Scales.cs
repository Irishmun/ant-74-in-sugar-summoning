using Godot;
using System;

public partial class Scales : Node
{
    [Export] private Node3D ScaleTopNode;
    [ExportGroup("Left scale")]
    [Export] private Node3D LeftScaleNode;
    [Export] private Area3D LeftScaleArea;
    [ExportGroup("Right scale")]
    [Export] private Node3D RightScaleNode;
    [Export] private Area3D RightScaleArea;
    [ExportGroup("Settings")]
    [Export] private float MinWeightDifference = 0, MaxWeightDifference = 100;
    [Export] private float MaxHeight = 5;
    [Export] private bool LeftStartsTop = true;

    private float _leftWeight, _rightWeight, _weightDiff;
    private float _leftTargetHeight, _rightTargetHeight;
    private float _leftCurrentHeight, _rightCurrentHeight;
    private Node _sceneRoot;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sceneRoot = GetTree().CurrentScene;
        ScaleTopNode.Position = new Vector3(0, MaxHeight, 0);
        if (LeftStartsTop == true)
        {
            _leftTargetHeight = MaxHeight;
            _leftCurrentHeight = _leftTargetHeight;
            SetScaleHeight(LeftScaleNode, _leftCurrentHeight);
            _rightTargetHeight = 0;
        }
        else
        {
            _rightTargetHeight = MaxHeight;
            _rightCurrentHeight = _rightTargetHeight;
            SetScaleHeight(RightScaleNode, _rightCurrentHeight);
            _leftTargetHeight = 0;
        }

        //events
        LeftScaleArea.BodyEntered += LeftScaleArea_BodyEntered;
        LeftScaleArea.BodyExited += LeftScaleArea_BodyExited;

        RightScaleArea.BodyEntered += RightScaleArea_BodyEntered;
        RightScaleArea.BodyExited += RightScaleArea_BodyExited;
    }

    public override void _PhysicsProcess(double delta)
    {

        if (_weightDiff < 0)
        {//right goes down

        }
        else
        {//left goes down

        }
        float deltaF = (float)delta;
        _leftCurrentHeight = Mathf.Lerp(_leftCurrentHeight, _leftTargetHeight, deltaF);
        _rightCurrentHeight = Mathf.Lerp(_rightCurrentHeight, _rightTargetHeight, deltaF);
        SetScaleHeight(LeftScaleNode, _leftCurrentHeight);
        SetScaleHeight(RightScaleNode, _rightCurrentHeight);
    }

    private void LeftScaleArea_BodyExited(Node3D body)
    {

        GD.Print(body.Name + " Exited left");
        //body.Reparent(_sceneRoot);
        if (body is RigidBody3D)
        {
            _leftWeight -= ((RigidBody3D)body).Mass;
        }

    }

    private void LeftScaleArea_BodyEntered(Node3D body)
    {
        GD.Print(body.Name + " Entered left");
        //body.Reparent(LeftScaleNode);
        if (body is RigidBody3D)
        {
            _leftWeight += ((RigidBody3D)body).Mass;
        }
    }

    private void RightScaleArea_BodyExited(Node3D body)
    {
        GD.Print(body.Name + " Exited right");
        //body.Reparent(_sceneRoot);
        if (body is RigidBody3D)
        {
            _rightWeight -= ((RigidBody3D)body).Mass;
        }
    }

    private void RightScaleArea_BodyEntered(Node3D body)
    {
        GD.Print(body.Name + " Entered right");
        //body.Reparent(RightScaleNode);
        if (body is RigidBody3D)
        {
            _rightWeight += ((RigidBody3D)body).Mass;
        }
    }

    private void SetScaleHeight(Node3D scale, float height)
    {
        Vector3 pos = scale.Position;
        pos.Y = height;
        scale.Position = pos;
    }

    /// <summary>Gets the difference in weight. right is heavier if weight is negative</summary>
    private float WeightDiff => _leftWeight - _rightWeight;
}

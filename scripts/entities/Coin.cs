using Godot;
using System;

public partial class Coin : RigidBody3D
{
    [Export] public int CentValue = 100;
    [Export] string CoinName = "Euro";
    private float _height;
    [Export] private AudioStreamPlayer3D audio;
    [ExportGroup("Physics")]
    [Export] private float MinVelocityThreshold = 0.1f;
    [Export] private float TimeBeforeSleep = 1;

    private double _t = 0;

    public override void _Ready()
    {
        CylinderShape3D cyl = GetChild<CollisionShape3D>(1).Shape as CylinderShape3D;
        _height = cyl.Height;
        //GD.Print($"{CoinName} has height of: {_height}");
        this.BodyEntered += Coin_BodyEntered;
    }

    private void Coin_BodyEntered(Node body)
    {
        audio.Play();
    }

    //TODO: consider if this is needed (perhaps have it do a spherecast around it for coins to be woken up)
    public override void _PhysicsProcess(double delta)
    {
        if (Freeze == true)
        { return; }
        GD.Print($"{Name} Velocity: {LinearVelocity.Length()} ({_t})");

        //check if velocity is below threshold for more than specific time
        //(set time to 0 if velocity > threshold, start timer if velocity <= threshold)
        if (LinearVelocity.Length() > MinVelocityThreshold)
        {
            _t = 0;
            return;
        }

        _t += delta;
        //if time exceeded, disabled rigidbody process
        if (_t >= TimeBeforeSleep)
        {
            GD.Print(Name + " started sleeping");
            Freeze = true;
        }

    }

    public void WakeSurrounding()
    {
        //check for any coins nearby, wake them up to have it still update
        //spherecast, wake all objects that are of type Coin
    }

    public void WakeCoin()
    {
        _t = 0;
        Freeze = false;
        GD.Print(Name + " woken up");
    }

    public float Height => _height;
}

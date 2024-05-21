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
    [Export] private ShapeCast3D WakeShapeCast;

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
        //GD.Print($"{Name} Velocity: {LinearVelocity.Length()} ({_t})");

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
            //GD.Print(Name + " started sleeping");
            Freeze = true;
        }

    }

    public void WakeSurrounding(bool recursive = true)
    {
#if DEBUG
        int x = WakeShapeCast.GetCollisionCount();
        GD.Print(Name + "is touching " + x + " coins.");
#endif
        for (int i = 0; i < WakeShapeCast.GetCollisionCount(); i++)
        {
            Node3D obj = WakeShapeCast.GetCollider(i) as Node3D;
            GD.Print("Trying to wake object " + obj.Name);
            Coin coin = obj as Coin;
            if (coin == null)
            { continue; }
            if (coin.IsSleeping == false)
            { continue; }
            coin.WakeCoin(true);
        }
    }

    public void WakeCoin(bool wakeTouching = false)
    {
        _t = 0;
        Freeze = false;
        //GD.Print(Name + " woken up");
        if (wakeTouching == false)
        { return; }
        WakeSurrounding();
    }

    public float Height => _height;
    public bool IsSleeping => Freeze;
}

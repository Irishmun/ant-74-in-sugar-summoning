using Godot;
using System;

public partial class Coin : RigidBody3D
{
    [Export] public int CentValue = 100;
    [Export] string CoinName = "Euro";
    private float _height;
    [Export] private AudioStreamPlayer3D audio;

    public override void _Ready()
    {
        CylinderShape3D cyl = GetChild<CollisionShape3D>(1).Shape as CylinderShape3D;
        _height = cyl.Height;
        GD.Print($"{CoinName} has height of: {_height}");
        this.BodyEntered += Coin_BodyEntered;
    }

    private void Coin_BodyEntered(Node body)
    {
        audio.Play();
    }

    public void StackOnCoin(Coin parentCoin)
    {
        Position = new Vector3(0, parentCoin.Height, 0);
    }

    public float Height => _height;
}

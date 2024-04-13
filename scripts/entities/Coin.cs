using Godot;
using System;

public partial class Coin : RigidBody3D
{
    [Export] int CentValue = 100;
    [Export] string CoinName = "Euro";
    private float _height;


    public override void _Ready()
    {
        CylinderShape3D cyl = GetChild<CollisionShape3D>(1).Shape as CylinderShape3D;
        _height = cyl.Height;
        GD.Print($"{CoinName} has height of: {_height}");
    }

    public void StackOnCoin(Coin parentCoin)
    {
        Position = new Vector3(0, parentCoin.Height, 0);
    }
    public float Height => _height;
}

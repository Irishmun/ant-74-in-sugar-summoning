using Godot;
using System.Collections.Generic;

public partial class CoinStack : Area3D
{
    [Export] private Node3D Stack;
    [Export] private AudioStreamPlayer3D PickupStream;
    [Export] float MaxCarryWeight = 75;// rigidbody mass
    [Export] float MinCarryWeight = 20;//weight before penalty

    [Export] private CollisionShape3D StackCollider;

    private List<Coin> _coins = new List<Coin>();
    private Player _player;
    private List<Coin> _coinsInArea = new List<Coin>();
    private HudUI _hud;
    private float _stackWeight = 0, _stackHeight = 0, _startCollisionRadius = 0;
    private Vector3 _localColliderStart;

    public override void _Ready()
    {
        this.BodyEntered += CoinStack_BodyEntered;
        this.BodyExited += CoinStack_BodyExited;
        _hud = GetNode<HudUI>(HudUI.TREE);
        _localColliderStart = StackCollider.Position;
        _startCollisionRadius = (StackCollider.Shape as CylinderShape3D).Radius;
        UpdateStackCollider();
    }
    public override void _ExitTree()
    {
        this.BodyEntered -= CoinStack_BodyEntered;
        this.BodyExited -= CoinStack_BodyExited;
    }
    /*
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        //move coins on stack
    }
    */
    private void CoinStack_BodyEntered(Node3D body)
    {
        //GD.Print(body.Name + " entered");
        Coin c = body as Coin;
        if (c != null)
        {
            _coinsInArea.Add((Coin)body);
        }
    }
    private void CoinStack_BodyExited(Node3D body)
    {
        //GD.Print(body.Name + " exited");
        Coin c = body as Coin;
        if (c != null)
        {
            _coinsInArea.Remove((Coin)body);
        }
    }

    public override void _Input(InputEvent e)
    {
        if (_player.MayDoStuff == false)
        { return; }
        if (e.IsActionPressed("Use"))
        {
            if (_coinsInArea.Count <= 0)
            { return; }
            AddCoinToStack(_coinsInArea[_coinsInArea.Count - 1]);
            PickupStream.Play();
            UpdateHud();
            return;
        }
        if (e.IsActionPressed("Drop"))
        {
            RemoveTopCoinFromStack();
            UpdateHud();
            return;
        }
    }

    public void AddCoinToStack(Coin coin)
    {
        _stackWeight += coin.Mass;
        _stackHeight += coin.Height;
        _coins.Add(coin);
        UpdatePlayerSpeed();
        UpdateStackCollider();
        coin.ProcessMode = ProcessModeEnum.Disabled;
        coin.Rotation = Vector3.Zero;
        if (_coins.Count == 1)
        {
            //coin.ProcessMode = ProcessModeEnum.Disabled;
            coin.Reparent(Stack, false);
            coin.Position = Vector3.Zero;
            //coin.Rotation = Vector3.Zero;
            MakePlayerHoldCoins();
            return;
        }
        Coin last = _coins[_coins.Count - 2];
        coin.Reparent(last, false);
        coin.Position = new Vector3(0, last.Height, 0);
        //_coins.Add(coin);
    }

    public void RemoveTopCoinFromStack()
    {//TODO: replace coin position with somewhere either in front of player or behind player
        if (_coins.Count == 0)
        { return; }
        Coin coin = _coins[_coins.Count - 1];
        GD.Print("removing coin from stack of size " + _coins.Count);

        _stackWeight -= coin.Mass;
        _stackHeight -= coin.Height;
        _coins.Remove(coin);

        UpdatePlayerSpeed();
        UpdateStackCollider();
        //reparent coin back to scene, set position to in front of player
        coin.Reparent(GetTree().CurrentScene, true);
        coin.GlobalPosition += GetForward(coin);
        GD.Print($"coin new position: {coin.GlobalPosition}");
        coin.ProcessMode = ProcessModeEnum.Pausable;

        if (_coins.Count == 0)
        {
            MakePlayerHoldCoins();
        }
    }

    private void MakePlayerHoldCoins()
    {
        GD.Print($"player holding {_coins.Count} coins");
        _player.IsHoldingCoins = _coins.Count > 0;
    }

    private void UpdatePlayerSpeed()
    {
        GD.Print("player carrying " + _stackWeight + "kg in coins");
        float remapped = _player.RunSpeedValue;
        if (_stackWeight < MinCarryWeight)//no need to calculate below threshold
        {
            remapped = _player.RunSpeedValue;
        }
        else if (_stackWeight > MaxCarryWeight)//no need to calculate beyond limit
        {
            remapped = _player.WalkSpeedValue;
        }
        else
        {
            //remap method
            remapped = (_stackWeight - MinCarryWeight) / (MaxCarryWeight - MinCarryWeight) * (_player.WalkSpeedValue - _player.RunSpeedValue) + _player.RunSpeedValue;
            remapped = Mathf.Clamp(remapped, _player.WalkSpeedValue, _player.RunSpeedValue);
        }
        GD.Print("setting player speed to: " + remapped);
        _player.ChangeMovementSpeed(remapped);
    }

    private void UpdateStackCollider()
    {//TODO: add radius handling as well, for "widestradius"
        //update collider height
        CylinderShape3D stackShape = StackCollider.Shape as CylinderShape3D;
        _stackHeight = _stackHeight < 0 ? 0 : _stackHeight;
        stackShape.Height = _stackHeight;
        if (_coins.Count <= 0)
        {
            stackShape.Radius = 0;
        }
        else
        {
            stackShape.Radius = _startCollisionRadius;
        }
        //update collider position
        GD.Print($"Stack height:{_stackHeight}, setting position of collider to: {(_localColliderStart.Y + (_stackHeight * 0.5f))}");
        StackCollider.Position = new Vector3(0, _localColliderStart.Y + (_stackHeight * 0.5f), 0);
    }

    private Vector3 GetForward(Node3D node)
    {
        return node.GlobalTransform.Basis.Z;
    }

    private void UpdateHud()
    {
        _hud.HeldCoins = _coins;
        _hud.UpdateLabel();
    }
    public Player Player { get => _player; set => _player = value; }
}

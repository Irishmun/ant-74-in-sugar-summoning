using Godot;
using System.Collections.Generic;

public partial class CoinStack : Area3D
{
    [Export] private Node3D Stack;

    private List<Coin> _coins = new List<Coin>();
    private Player _player;
    private List<Coin> _coinsInArea = new List<Coin>();

    public override void _Ready()
    {
        this.BodyEntered += CoinStack_BodyEntered;
        this.BodyExited += CoinStack_BodyExited;
    }
    public override void _ExitTree()
    {
        this.BodyEntered -= CoinStack_BodyEntered;
        this.BodyExited -= CoinStack_BodyExited;
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        //move coins on stack
    }

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
        if (e.IsActionPressed("Use"))
        {
            if (_coinsInArea.Count <= 0)
            { return; }
            AddCoinToStack(_coinsInArea[_coinsInArea.Count - 1]);
            return;
        }
        if (e.IsActionPressed("Drop"))
        {
            RemoveTopCoinFromStack();
            return;
        }
    }

    public void AddCoinToStack(Coin coin)
    {
        if (_coins.Count == 0)
        {
            coin.ProcessMode = ProcessModeEnum.Disabled;
            coin.Reparent(Stack, false);
            coin.Position = Vector3.Zero;
            coin.Rotation = Vector3.Zero;
            _coins.Add(coin);
            MakePlayerHoldCoins();
            return;
        }
        Coin last = _coins[_coins.Count - 1];
        coin.ProcessMode = ProcessModeEnum.Disabled;
        coin.Reparent(last, false);
        coin.StackOnCoin(last);
        coin.Rotation = Vector3.Zero;
        _coins.Add(coin);

    }

    public void RemoveTopCoinFromStack()
    {//TODO: replace coin position with somewhere either in front of player or behind player
        if (_coins.Count == 0)
        { return; }
        Coin coin = _coins[_coins.Count - 1];
        GD.Print("removing coin from stack of size " + _coins.Count);
        if (_coins.Count == 1)
        {
            coin.Reparent(GetTree().CurrentScene, true);
            GD.Print($"coin old position: {coin.GlobalPosition}");

            coin.GlobalPosition += GetForward(coin);
            GD.Print($"coin new position: {coin.GlobalPosition}");
            coin.ProcessMode = ProcessModeEnum.Pausable;
            _coins.Remove(coin);
            MakePlayerHoldCoins();
            return;
        }
        coin.Reparent(GetTree().CurrentScene, true);
        coin.GlobalPosition += GetForward(coin);
        coin.ProcessMode = ProcessModeEnum.Pausable;
        _coins.Remove(coin);
        //remove top coin from pile, returning it's parent to the scene

    }

    private void MakePlayerHoldCoins()
    {
        GD.Print($"player holding {_coins.Count} coins");
        _player.IsHoldingCoins = _coins.Count > 0;
    }

    public Player Player { get => _player; set => _player = value; }

    private Vector3 GetForward(Node3D node)
    {
        return node.GlobalTransform.Basis.Z;
    }
}

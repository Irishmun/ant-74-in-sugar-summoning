using Godot;
using System;
using System.Collections.Generic;

public partial class CoinObjective : Area3D
{
    [Export] private int CoinWinCount = 11;
    [Export] private int WinValue = 20000;//cents
    [Export] private string WinScene = "d1_awakeningwood_02";

    private List<Coin> _coins = new List<Coin>();
    private HudUI _hud;
    private float _value = 0;
    private const int CentsInEuro = 100;

    public override void _Ready()
    {
        this.BodyEntered += CoinStack_BodyEntered;
        this.BodyExited += CoinStack_BodyExited;
        _hud = GetNode<HudUI>(HudUI.TREE);
    }

    public override void _ExitTree()
    {
        this.BodyEntered -= CoinStack_BodyEntered;
        this.BodyExited -= CoinStack_BodyExited;
    }

    private void CoinStack_BodyExited(Node3D body)
    {
        Coin c = body as Coin;
        if (c != null)
        {
            _coins.Remove((Coin)body);
            _value -= c.CentValue;
        }
        UpdateHud();
    }

    private void CoinStack_BodyEntered(Node3D body)
    {
        GD.Print(body.Name + " has entered");
        Coin c = body as Coin;
        if (c != null)
        {
            GD.Print("Coin entered");
            _coins.Add((Coin)body);
            _value += c.CentValue;
            CheckWin();
        }
        UpdateHud();
    }

    private void UpdateHud()
    {

        _hud.ScoredCoins = _coins;
        _hud.Value = CalcValue();
        _hud.UpdateLabel();
    }

    private float CalcValue()
    {
        //GD.Print($"updating value with {_ScoredCoins.Count} coins");
        float value = 0;
        for (int i = 0; i < _coins.Count; i++)
        {
            value += _coins[i].CentValue / CentsInEuro;
        }
        return value;
    }

    private void CheckWin()
    {
        if (_value >= WinValue || _coins.Count >= CoinWinCount)
        {
            GameTimer.Instance.StopTimer();
            GD.PrintRich("[rainbow][wave]You Win![/wave][/rainbow]");
            GD.Print($"final time: {GameTimer.Instance.Time}({GameTimer.Instance.RawTime})");
            Player.Instance.MayDoStuff = false;
            SceneFade fade = GetNode<SceneFade>(SceneFade.TREE);
            fade.SceneName = WinScene;
            fade.FadeToScene();
        }
    }
}

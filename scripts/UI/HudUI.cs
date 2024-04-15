using Godot;
using System;
using System.Collections.Generic;

public partial class HudUI : Control
{
    public static string HUD_UI_TREE = "/root/GameUI/Non-main menu/hud";

    [Export] private Label scoreLabel;
    [Export] private CanvasItem WinImage;

    private List<Coin> _ScoredCoins = new List<Coin>();
    private List<Coin> _heldCoins = new List<Coin>();
    private float _value = 0;
    public override void _Ready()
    {
        UpdateLabel();
        WinImage.Visible = false;
    }


    public void ShowWin()
    {
        WinImage.Visible = true;
    }

    public void UpdateLabel()
    {
        scoreLabel.Text = $"Held Coins: {_heldCoins.Count}\nScored Coins: {_ScoredCoins.Count}\nValue: ${_value.ToString("0.00")}";
    }

    public List<Coin> ScoredCoins { get => _ScoredCoins; set => _ScoredCoins = value; }
    public List<Coin> HeldCoins { get => _heldCoins; set => _heldCoins = value; }
    public float Value { get => _value; set => _value = value; }
}

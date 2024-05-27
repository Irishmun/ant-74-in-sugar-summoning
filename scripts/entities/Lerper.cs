using Godot;
using System;

public partial class Lerper : Node3D
{
    [Export] private Node3D StartPos, EndPos;
    [Export] private int LerpMode = 0;

    private Vector3 _start, _end;

    private float speed = 0.5f;
    private float _startDelay = 5f, _t = 0;
    private double _currentDelay = 0;
    private bool _forwards = true;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _start = StartPos.Position;
        _end = EndPos.Position;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        _currentDelay += delta;
        if (_currentDelay < _startDelay)
        { return; }

        if (_forwards == true)
        {
            _t += (float)delta * speed;
        }
        else
        {
            _t -= (float)delta * speed;
        }
        _t = Mathf.Clamp(_t, 0, 1);
        //lerping methods
        switch (LerpMode)
        {
            case 0://a to b (actual linear)
                Position = _start.Lerp(_end, _t);
                break;
            case 1://pos to b (t) (exponential speed increase)
                if (_forwards == true)
                {
                    Position = Position.Lerp(_end, _t);
                    break;
                }
                Position = Position.Lerp(_start, 1 - _t);
                break;
            case 2://pos to b (delta only) (exponential speed decrease)
                if (_forwards == true)
                {
                    Position = Position.Lerp(_end, (float)delta);
                    break;
                }
                Position = Position.Lerp(_start, (float)delta);
                break;
        }

        if (_forwards && _t == 1)
        {
            _forwards = false;
            return;
        }
        if (!_forwards && _t == 0)
        {
            _forwards = true;
            return;
        }
    }
}

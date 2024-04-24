using Godot;
using System;

public partial class GameTimer : Node
{
    public static GameTimer Instance;

    private double _time;
    private bool _timerRunning = false;

    public override void _Ready()
    {
        if (Instance.IsValid() == false)
        {
            Instance = this;
            ResetTimer();
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_timerRunning == false)
        { return; }
        _time += delta;
    }

    public void StartTimer(bool reset = false)
    {
        StopTimer();
        if (reset == true)
        {
            ResetTimer();
        }
        _timerRunning = true;
    }

    public void StopTimer()
    {
        _timerRunning = false;
    }

    public void ResetTimer()
    {
        _time = 0;
    }

    public string TimeString()
    {
        TimeSpan time = TimeSpan.FromSeconds(_time);
        return $"{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")}.{time.Milliseconds.ToString("000")}";
    }

    public TimeSpan Time => TimeSpan.FromSeconds(_time);
    public double RawTime => _time;
}

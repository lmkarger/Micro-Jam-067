using Godot;
using System;

public partial class DayNightModulate : CanvasModulate
{
    [Export]
    public Color DayModulate;

    [Export]
    public Color NightModulate;

    public override void _Ready()
    {
        base._Ready();

        UpdateModulate();
        GameManager.Instance.GameTicked += () => UpdateModulate();
    }

    private void UpdateModulate()
    {
        bool isDay = GameManager.Instance.IsDay();
        Modulate = isDay ? DayModulate : NightModulate;
    }
}

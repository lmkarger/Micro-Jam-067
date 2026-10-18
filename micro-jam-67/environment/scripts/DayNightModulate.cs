using Godot;
using System;

public partial class DayNightModulate : CanvasModulate
{
    private const float tweenDuration = 0.3f;
    private const Tween.TransitionType tweenTrans = Tween.TransitionType.Sine;

    [Export]
    public Color DayModulate;

    [Export]
    public Color NightModulate;

    public override void _Ready()
    {
        base._Ready();

        Color = GetTargetColor();
        UpdateModulate();
        GameManager.Instance.GameTicked += () => UpdateModulate();
    }

    private void UpdateModulate()
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "color", GetTargetColor(), tweenDuration)
            .SetTrans(tweenTrans);
    }

    private Color GetTargetColor()
    {
        bool isDay = GameManager.Instance.IsDay();
        return isDay ? DayModulate : NightModulate;
    }
}

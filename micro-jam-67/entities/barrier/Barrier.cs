using Godot;
using System;
using System.Linq;

[GlobalClass]
public partial class Barrier : GridEntity
{
    [Export]
    public bool onDuringDay = true;

    public bool IsToggled { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        GameManager.Instance.GameTicked += OnTimeAdvanced;
        GameManager.Instance.PlayerPositionChanged += (Vector2I pos) => CheckForPlayer();
        OnTimeAdvanced();
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameTicked -= OnTimeAdvanced;
            GameManager.Instance.PlayerPositionChanged -= (Vector2I pos) => CheckForPlayer();
        }
    }

    private void OnTimeAdvanced()
    {
        if (!IsInstanceValid(this)) return;

        bool isDay = GameManager.Instance.IsDay();
        IsToggled = isDay == onDuringDay;
        Color targetModulate = IsToggled ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.3f);

        Tween tween = GetTree().CreateTween()
            .SetTrans(Tween.TransitionType.Expo)
            .SetEase(Tween.EaseType.Out);

        tween.TweenProperty(this, "modulate", targetModulate, 0.4f);

        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        if (!IsInstanceValid(this)) return;
        if (!IsToggled) return;
        if (GameManager.Instance.PlayerPositions.Count == 0) return;

        Vector2I playerPosition = GameManager.Instance.PlayerPositions.Last();
        if (playerPosition != GridPosition) return;

        GameManager.Instance.EndGame(false);
    }
}

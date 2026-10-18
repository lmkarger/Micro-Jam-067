using Godot;
using System;

public partial class Ui : CanvasLayer
{
    [Export]
    private TextureRect _bloodOverlay;

    [Export]
    private Button _retryButton;

    public override void _Ready()
    {
        base._Ready();

        GameManager.Instance.GameEnded += (bool win) => OnGameEnd(win);
        _retryButton.Pressed += () => GameManager.Instance.ResetGame();
    }

    private void OnGameEnd(bool win)
    {
        if (win){
            OnWin();
        }
        else {
            OnDeath();
        }
    }

    private void OnWin()
    {
        GD.Print("sigma");
        if (!IsInstanceValid(this)) return;
        if (IsInstanceValid(_retryButton)) _retryButton.Show();
    }

    private void OnDeath()
    {
        if (!IsInstanceValid(this)) return;
        if (IsInstanceValid(_retryButton)) _retryButton.Show();

        var tween = GetTree().CreateTween()
            .SetTrans(Tween.TransitionType.Sine);
        tween.TweenProperty(
            _bloodOverlay,
            "modulate",
            new Color(1, 1, 1, 1),
            0.3f
        );
    }
}

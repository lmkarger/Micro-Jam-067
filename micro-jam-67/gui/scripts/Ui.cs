using Godot;
using System;

public partial class Ui : CanvasLayer
{
    [Export]
    private TextureRect _bloodOverlay;

    [Export]
    private Button _retryButton;

    [Export]
    private Button _nextButton;

    public override void _Ready()
    {
        base._Ready();

        GameManager.Instance.GameEnded += OnGameEnd;

        _retryButton.Pressed += GameManager.Instance.ResetGame;
        _nextButton.Pressed += GameManager.Instance.SignalNextLevel;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (IsInstanceValid(_retryButton))
        {
            _retryButton.Pressed -= GameManager.Instance.ResetGame;
        }

        if (IsInstanceValid(_nextButton))
        {
            _nextButton.Pressed -= GameManager.Instance.SignalNextLevel;
        }
    }

    private void OnGameEnd(bool win)
    {
        if (!IsInstanceValid(this)) return;

        if (win){
            OnWin();
        }
        else {
            OnDeath();
        }
    }

    private void OnWin()
    {
        if (IsInstanceValid(_nextButton)) _nextButton.Show();
    }

    private void OnDeath()
    {
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

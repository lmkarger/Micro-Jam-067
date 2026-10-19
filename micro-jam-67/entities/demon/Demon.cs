using Godot;
using System;

public partial class Demon : GridEntity
{
    [Export]
    private AnimationPlayer animationPlayer;

    [Export]
    private AnimatedSprite2D sprite;

    public override void _Ready()
    {
        base._Ready();

        animationPlayer.Play("idle");
        GameManager.Instance.PlayerPositionChanged += (Vector2I newPlayerPosition) => playerMoved(newPlayerPosition);
        GameManager.Instance.GameTicked += () => UpdateAppearance();
        UpdateAppearance();
    }

    /// <summary>
    /// Updates the appearance of the demon's sprite to match whether it is
    /// day or not.
    /// </summary>
    private void UpdateAppearance()
    {
        if (!IsInstanceValid(this)) return;
        
        bool isDay = GameManager.Instance.IsDay();
        sprite.Frame = isDay ? 1 : 0;
    }

    private void playerMoved(Vector2I newPlayerPosition)
    {
        if (!IsInstanceValid(this)){
            return;
        }
        bool isOnPlayer = newPlayerPosition == GridPosition;
        if (isOnPlayer)
        {
            GameManager.Instance.EndGame(true);
        }
    }
}

using Godot;
using System;

[GlobalClass]
public partial class Bullet : GridEntity
{
    /// <summary>
    /// The direction the bullet moves each game tick.
    /// </summary>
    [Export]
    public Vector2I Direction = Vector2I.Right;

    private Sprite2D sprite;

    public override void _Ready()
    {
        base._Ready();
        GameManager.Instance.GameTicked += () => Move();

        sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
        UpdateSpriteDirection();
    }

    private void UpdateSpriteDirection()
    {
        if (sprite is null) return;

        sprite.Rotation = ((Vector2)Direction).Angle();
    }

    private void Move()
    {
        if (!IsInstanceValid(this)) return;

        if (!ValidateMove(GridPosition + Direction))
        {
            QueueFree();
            return;
        }

        base.Move(Direction);
    }
}

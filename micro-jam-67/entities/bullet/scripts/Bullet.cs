using Godot;
using System;
using System.Net.Http.Headers;

[GlobalClass]
public partial class Bullet : GridEntity
{
    /// <summary>
    /// The direction the bullet moves each game tick.
    /// </summary>
    [Export]
    public Vector2I Direction = Vector2I.Right;

    /// <summary>
    /// Whether or not the bullet is able to pass through obstacles.
    /// </summary>
    [Export]
    public bool PassThroughObstacles = false;

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

    public override bool ValidateMove(Vector2I coordinate)
    {
        bool hasObstacle = Obstacles.GetCellSourceId(coordinate) != -1;
        bool hasFloor = Floor.GetCellSourceId(coordinate) != -1;

        if (hasObstacle && !PassThroughObstacles)
        {
            return false;
        }

        return hasFloor;
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

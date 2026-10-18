using Godot;
using System;
using System.Linq;
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
    private AnimationPlayer animationPlayer;
    private bool _disabled = false;

    public override void _Ready()
    {
        base._Ready();
        GameManager.Instance.GameTicked += Move;

        sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
        UpdateSpriteDirection();

        animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");

        if (GameManager.Instance.PlayerPositions.Count > 0 && GridPosition == GameManager.Instance.PlayerPositions.Last())
        {
            GameManager.Instance.EndGame(false);
            Destroy();
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameTicked -= Move;
        }
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
        if (_disabled) return;

        if (!ValidateMove(GridPosition + Direction))
        {
            Destroy();
            return;
        }

        Vector2I previousPos = GridPosition;
        Vector2I currentPlayerPos = GameManager.Instance.PlayerPositions.Last();

        base.Move(Direction);

        bool isOnPlayer = currentPlayerPos == GridPosition;
        bool passedThroughPlayer = false;

        if (GameManager.Instance.PlayerPositions.Count >= 2)
        {
            Vector2I previousPlayerPos = GameManager.Instance.PlayerPositions[GameManager.Instance.PlayerPositions.Count - 2];
            passedThroughPlayer = (GridPosition == previousPlayerPos) && (previousPos == currentPlayerPos);
        }

        if (isOnPlayer || passedThroughPlayer)
        {
            GameManager.Instance.EndGame(false);
            Destroy();
        }
    }

    private void Destroy()
    {
        _disabled = true;

        if (animationPlayer == null)
        {
            QueueFree();
            return;
        }

        animationPlayer.Play("destroy");
    }
}

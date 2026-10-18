using Godot;

public partial class Character : GridEntity
{
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        base._Ready();

        GameManager.Instance.PlayerPositions.Add(GridPosition);

        GameManager.Instance.GameEnded += () => OnDeath();
        animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");

        GameManager.Instance.StartGame();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (GameManager.Instance.GameOver) return;

        player = true;
        canMoveOnObstacle = false;
        Vector2I movement = Vector2I.Zero;

        if (Input.IsActionJustPressed("move_up"))
            movement = Vector2I.Up;
        else if (Input.IsActionJustPressed("move_down"))
            movement = Vector2I.Down;
        else if (Input.IsActionJustPressed("move_left"))
            movement = Vector2I.Left;
        else if (Input.IsActionJustPressed("move_right"))
            movement = Vector2I.Right;

        if (movement != Vector2I.Zero) Move(movement);
    }

    public override bool Move(Vector2I direction)
    {
        if (base.Move(direction))
        {
            GameManager.Instance.PlayerMoved(GridPosition);

            return true;
        }

        return false;
    }

    private void OnDeath()
    {
        if (!IsInstanceValid(animationPlayer)) return;

        animationPlayer.Play("kill");
    }
}

using Godot;

public partial class Character : GridEntity
{
    private AnimationPlayer animationPlayer;
    private AudioStreamPlayer clockSounds;

    public override void _Ready()
    {
        base._Ready();

        GameManager.Instance.PlayerPositions.Add(GridPosition);

        GameManager.Instance.GameEnded += (bool win) => OnDeath(win);
        animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
        clockSounds = GetNodeOrNull<AudioStreamPlayer>("ClockSound");
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
            if (IsInstanceValid(clockSounds))
            {
                clockSounds.Play();
            }

            return true;
        }

        return false;
    }

    private void OnDeath(bool win)
    {
        if (!IsInstanceValid(animationPlayer) || win) return;

        animationPlayer.Play("kill");
    }
}

using Godot;

public partial class Character : GridEntity
{

	public override void _Process(double delta)
    {	
		base._Process(delta);
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

        if (movement != Vector2I.Zero)
            Move(movement);
    }
}

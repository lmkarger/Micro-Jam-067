using Godot;
using System;

public partial class Bullet : GridEntity
{
	// Called when the node enters the scene tree for the first time.
	private Vector2I direction;
	public Bullet()
    {
    }

	public void CreateBullet(Vector2I position, Vector2I dir){
		Sprite2D sprite = new Sprite2D();
		sprite.Texture = GD.Load<Texture2D>("res://assets/church_bullet.png");
		sprite.Centered = false;
		AddChild(sprite);
		direction = dir;
		GridPosition = position;
		canMoveOnObstacle = false;
		player = false;
		UpdatePosition();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void Move()
	{
		base.Move(direction);
	}
	public Vector2I getDirection(){
		return direction;
	}
}

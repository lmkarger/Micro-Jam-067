using Godot;
using System;

public partial class GridEntity : Node2D
{
    // The factor to smooth. Higher = snappier, lower = slower
    private const double smoothingFactor = 20d;

	public Vector2I GridPosition = Vector2I.Zero;
	protected bool canMoveOnObstacle;
	private TileMapLayer Obstacles;
	private TileMapLayer Floor;
	private Vector2I RealPos; 
	private const int TileSize = 16;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready(){
		Obstacles = GetTree().CurrentScene.GetNode<TileMapLayer>("%Obstacles");
		Floor = GetTree().CurrentScene.GetNode<TileMapLayer>("%Floor");
		GD.Print($"Obstacles: {Obstacles}");
    	GD.Print($"Floor: {Floor}");
		UpdatePosition();
	}

	public void Move(Vector2I direction){
		Vector2I finalPos = GridPosition + direction;
		if (ValidateMove(finalPos)){
			GridPosition += direction;
			UpdatePosition();
		}
	}

	public bool ValidateMove(Vector2I coordinate){
		if (!canMoveOnObstacle){
			if (Obstacles.GetCellSourceId(coordinate) != -1){
				return false;
			}
			if (Floor.GetCellSourceId(coordinate) == -1){
				return false;
			}
		}
		return true;
	}

	public void UpdatePosition(){
		RealPos = GridPosition * TileSize;
	}
	public override void _Process(double delta){
        float step = 1f - (float)Math.Exp(-delta * smoothingFactor);
		Position = Position.Lerp(RealPos, step); 
	}
}

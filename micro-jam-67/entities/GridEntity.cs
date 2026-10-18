using Godot;
using System;

[GlobalClass]
public partial class GridEntity : Node2D
{
    // The factor to smooth. Higher = snappier, lower = slower
    private const double smoothingFactor = 20d;

	[Export]
	public Vector2I GridPosition = Vector2I.Zero;
	protected bool canMoveOnObstacle;
	protected bool player;
	private TileMapLayer Obstacles;
	private TileMapLayer Floor;
	private Vector2I RealPos;
    private const int TileSize = 16;

    public override void _Ready()
    {
        Obstacles = GetTree().CurrentScene.GetNode<TileMapLayer>("%Obstacles");
        Floor = GetTree().CurrentScene.GetNode<TileMapLayer>("%Floor");

        UpdatePosition();
        Position = RealPos; // Instantly update position upon start
    }
    
    public override void _Process(double delta){
        float step = 1f - (float)Math.Exp(-delta * smoothingFactor);
		Position = Position.Lerp(RealPos, step); 
	}

	public void Move(Vector2I direction){
		Vector2I finalPos = GridPosition + direction;
		if (ValidateMove(finalPos)){
			GD.Print("movement!");
			GridPosition += direction;
			UpdatePosition();
			if (player){
				GameManager.AdvanceTime(1);
			}
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

    public void UpdatePosition()
    {
        RealPos = GridPosition * TileSize;
    }
    
}

using Godot;

public partial class GridEntity : Node2D
{
	public Vector2I GridPosition = Vector2I.Zero;
	protected bool canMoveOnObstacle;
	private TileMapLayer Obstacles;
	private TileMapLayer Floor;
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
		Position = GridPosition * TileSize;
	}
}

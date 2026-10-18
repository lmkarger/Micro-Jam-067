using Godot;
using System;

[GlobalClass]
public partial class GridEntity : Node2D
{
    /// <summary>
    /// Whether the GridEntity should snap to the closest position upon spawn.
    /// </summary>
    [Export]
    public bool SnapToClosestPosition = true;

    [Export]
    public Vector2I GridPosition = Vector2I.Zero;

	protected bool canMoveOnObstacle;
    protected bool player;
	protected TileMapLayer Obstacles;
    protected TileMapLayer Floor;
    
    private const int TileSize = 16;
    /// <summary>
    /// The factor to smooth. Higher = snappier, lower = slower
    /// </summary>
    private const double smoothingFactor = 20d;

    public override void _Ready()
    {
        Obstacles = GetTree().CurrentScene.GetNode<TileMapLayer>("%Obstacles");
        Floor = GetTree().CurrentScene.GetNode<TileMapLayer>("%Floor");

        if (SnapToClosestPosition)
        {
            Vector2 approxGridPos = (GlobalPosition - new Vector2I(TileSize / 2, TileSize / 2)) / TileSize;
            GridPosition = new Vector2I(
                (int)Math.Round(approxGridPos.X),
                (int)Math.Round(approxGridPos.Y)
            );
        }

        GlobalPosition = GetTargetPosition(); // Instantly update position upon start
    }
    
    public override void _Process(double delta) {
        float step = 1f - (float)Math.Exp(-delta * smoothingFactor);
		GlobalPosition = GlobalPosition.Lerp(GetTargetPosition(), step); 
	}

	public void Move(Vector2I direction) {
		Vector2I finalPos = GridPosition + direction;
		if (ValidateMove(finalPos)){
			GridPosition += direction;
            
			if (player) {
				GameManager.Instance.AdvanceTime(1);
			}
		}
	}

    public virtual bool ValidateMove(Vector2I coordinate)
    {
        if (!canMoveOnObstacle)
        {
            if (Obstacles.GetCellSourceId(coordinate) != -1)
            {
                return false;
            }
            if (Floor.GetCellSourceId(coordinate) == -1)
            {
                return false;
            }
        }
        return true;
    }
    
    private Vector2 GetTargetPosition()
    {
        return GridPosition * TileSize + new Vector2I(TileSize / 2, TileSize / 2);
    }
}

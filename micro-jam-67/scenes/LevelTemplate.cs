using Godot;
using System;

public partial class LevelTemplate : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public TileMapLayer Obstacles;

    [Export]
    public TileMapLayer Floor;
}

using Godot;
using System;

[GlobalClass]
public partial class Level : Node2D
{
    /// <summary>
    /// The number of ticks the game initializes with when the level begins.
    /// </summary>
    [Export]
    public int StartingTicks = 10;

    /// <summary>
    /// The path/uuid of the next level scene to go to.
    /// </summary>
    [Export]
    public String NextLevelPath = "";


    public override void _Ready()
    {
        base._Ready();
        GameManager.Instance.AdvanceTime(StartingTicks);
    }

}

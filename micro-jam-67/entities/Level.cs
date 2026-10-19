using Godot;
using System;

[GlobalClass]
public partial class Level : Node2D
{
    /// <summary>
    /// The path/uuid of the next level scene to go to.
    /// </summary>
    [Export]
    public String NextLevelPath = "";

    /// <summary>
    /// The number of ticks the game initializes with when the level begins.
    /// </summary>
    [Export]
    public int StartingTicks = 10;

    [ExportGroup("Day Night Cycle")]
    [Export(PropertyHint.Range, "1,10,1,or_greater")]
    public int NightDuration = 8;

    [Export(PropertyHint.Range, "1,10,1,or_greater")]
    public int DayDuration = 2;


    public override void _Ready()
    {
        base._Ready();
        GameManager.Instance.AdvanceTime(StartingTicks);
        GameManager.Instance.NextLevelSelected += OnNextLevelSelected;

        GameManager.Instance.NightDuration = NightDuration;
        GameManager.Instance.DayDuration = DayDuration;
        GameManager.Instance.EmitSignal(GameManager.SignalName.GameBegan);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.NextLevelSelected -= OnNextLevelSelected;
        }
    }

    private void OnNextLevelSelected()
    {
        if (NextLevelPath == "") return;

        GameManager.Instance.ChangeLevel(NextLevelPath);
    }
}

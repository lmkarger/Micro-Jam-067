using Godot;
using System;
using System.Linq;

[GlobalClass]
public partial class Clock : GridEntity
{
    public override void _Ready()
    {
        base._Ready();

        GameManager.Instance.PlayerPositionChanged += (Vector2I position) => CheckPlayerCollision();
        CheckPlayerCollision();
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerPositionChanged -= (Vector2I position) => CheckPlayerCollision();
        }
    }

    private void CheckPlayerCollision()
    {
        if (!IsInstanceValid(this)) return;
        if (GameManager.Instance.PlayerPositions.Count == 0) return;
        if (GridPosition != GameManager.Instance.PlayerPositions.Last()) return;

        GameManager.Instance.AdvanceTime(1);
        QueueFree();
    }
}

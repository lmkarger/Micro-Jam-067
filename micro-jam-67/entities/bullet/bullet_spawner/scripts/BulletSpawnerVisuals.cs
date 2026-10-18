using Godot;
using System;

public partial class BulletSpawnerVisuals : Node2D
{
    private Label _phaseLabel;

    public override void _Ready()
    {
        base._Ready();
        _phaseLabel = GetNode<Label>("PhaseLabel");
    }

    public void DisplayPhase(int phase)
    {
        if (!IsInstanceValid(this)) return;

        if (_phaseLabel == null) return;

        _phaseLabel.Text = phase == 0 ? "" : phase.ToString();
    }
}

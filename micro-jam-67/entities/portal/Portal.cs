using Godot;
using System;

[GlobalClass]
public partial class Portal : GridEntity
{
    private static CompressedTexture2D arrowTexture = GD.Load<CompressedTexture2D>("uid://bf3rsxt1aeibq");

    [Export]
    public Portal link;

    public override void _Ready()
    {
        base._Ready();

        GameManager.Instance.PlayerPositionChanged += OnPlayerMoved;

        CreateArrow();
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerPositionChanged -= OnPlayerMoved;
        }
    }

    private void CreateArrow()
    {
        if (!IsInstanceValid(this)) return;
        if (!IsInstanceValid(link)) return;

        Sprite2D sprite = new();
        sprite.Texture = arrowTexture;
        sprite.Modulate = new Color(0.24f, 0.2f, 0.5f, 0.5f);
        AddChild(sprite);

        sprite.GlobalPosition = (GlobalPosition + link.GlobalPosition) / 2f;
        sprite.Rotation = GlobalPosition.AngleToPoint(link.GlobalPosition);

        float dist = GlobalPosition.DistanceTo(link.GlobalPosition) / 16f;
        sprite.Scale = new Vector2(Math.Max(1f, dist - 2), 0.5f);
    }

    private void OnPlayerMoved(Vector2I position)
    {
        if (!IsInstanceValid(this)) return;
        if (!IsInstanceValid(link)) return;
        if (position != GridPosition) return;

        GameManager.Instance.EmitSignal(GameManager.SignalName.PortalTouched, link.GridPosition);
    }
}

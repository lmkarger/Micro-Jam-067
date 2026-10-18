using Godot;
using System;

[GlobalClass]
public partial class BulletSpawner : GridEntity
{
    /// <summary>
    /// The scene of the bullet which this [BulletSpawner] should spawn.
    /// </summary>
    [Export]
    public PackedScene BulletScene = GD.Load<PackedScene>("uid://eetk1gkylpkv");

    /// <summary>
    /// How many ticks it takes to spawn one bullet.
    /// </summary>
    [Export]
    public uint SpawnCycle = 1;

    /// <summary>
    /// The starting phase for the spawn cycle. When this reaches SpawnCycle, a bullet is spawned.
    /// </summary>
    [Export]
    public uint Phase = 0;

    /// <summary>
    /// The direction to spawn the bullet facing.
    /// </summary>
    [Export]
    public Vector2I SpawnDirection = Vector2I.Right;

    public override void _Ready()
    {
        base._Ready();
        GameManager.Instance.GameTicked += () => OnGameTick();
    }

    private void OnGameTick()
    {
        Phase++;
        if (Phase < SpawnCycle) return;

        Phase = 0;
        SpawnBullet();
    }

    private void SpawnBullet()
    {
        var bullet = BulletScene.Instantiate<Bullet>();
        bullet.GridPosition = GridPosition;
        bullet.SnapToClosestPosition = false;
        bullet.Direction = SpawnDirection;

        AddChild(bullet);
    }
}

using Godot;
using System;

[GlobalClass]
public partial class BulletSpawner : GridEntity
{
    private static PackedScene _bulletSpawnerVisualsScene = GD.Load<PackedScene>("uid://cyrms4jhblxel");

    private BulletSpawnerVisuals _visuals;

    /// <summary>
    /// The scene of the bullet which this [BulletSpawner] should spawn.
    /// </summary>
    [Export]
    public PackedScene BulletScene = GD.Load<PackedScene>("uid://eetk1gkylpkv");

    [Export]
    public bool OnlySpawnDuringDay = false;

    [Export]
    public bool OnlySpawnDuringNight = false;

    /// <summary>
    /// How many ticks it takes to spawn one bullet.
    /// </summary>
    [Export]
    public int SpawnCycle = 1;

    /// <summary>
    /// The starting phase for the spawn cycle. When this reaches SpawnCycle, a bullet is spawned.
    /// </summary>
    [Export]
    public int Phase = 1;

    /// <summary>
    /// The direction to spawn the bullet facing.
    /// </summary>
    [Export]
    public Vector2I SpawnDirection = Vector2I.Right;

    public override void _Ready()
    {
        base._Ready();
        GameManager.Instance.GameTicked += () => OnGameTick();

        _visuals = _bulletSpawnerVisualsScene.Instantiate<BulletSpawnerVisuals>();
        AddChild(_visuals);
        _visuals.DisplayPhase(Phase);
    }

    private void OnGameTick()
    {
        Phase--;

        if (Phase <= 0)
        {
            Phase = SpawnCycle;
            var isDay = GameManager.Instance.IsDay();
            
            if (OnlySpawnDuringDay && isDay || OnlySpawnDuringNight && !isDay || !OnlySpawnDuringDay && !OnlySpawnDuringNight)
            {
                SpawnBullet();
            }
        }

        if (_visuals != null) _visuals.DisplayPhase(Phase);
    }

    private void SpawnBullet()
    {
        if (!IsInstanceValid(this)) return;

        var bullet = BulletScene.Instantiate<Bullet>();
        bullet.GridPosition = GridPosition;
        bullet.SnapToClosestPosition = false;
        bullet.Direction = SpawnDirection;

        AddChild(bullet);
    }
}

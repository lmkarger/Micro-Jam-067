using Godot;
using System;

public partial class MusicManager : Node
{
    private PackedScene _music = GD.Load<PackedScene>("uid://dq2k5l02ls0if");

    public override void _Ready()
    {
        base._Ready();

        var music = _music.Instantiate<AudioStreamPlayer>();
        AddChild(music);
    }
}

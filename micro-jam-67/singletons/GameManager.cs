using Godot;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;

public partial class GameManager : Node
{
    public const int NightDuration = 8;
    public const int DayDuration = 2;

    /// <summary>
    /// The event fired when the game tick is updated (upon player movement).
    /// </summary>
    [Signal]
    public delegate void GameTickedEventHandler();

    [Signal]
    public delegate void PlayerPositionChangedEventHandler(Vector2I newPlayerPosition);

    public static GameManager Instance { get; private set; }

    private int _gameTime;
    public List<Vector2I> PlayerPositions = new();

    public int GameTime
    {
        get { return _gameTime; }
        private set { _gameTime = value % (NightDuration + DayDuration); }
    }

    public int GamePhase = 1;

    public override void _Ready()
    {
        GameTime = 0;
        Instance = this;

        GetTree().CreateTimer(0.5).Timeout += () => AdvanceTime(10);
    }

    public bool IsDay()
    {
        return GameTime >= NightDuration;
    }

    public void PlayerMoved(Vector2I newPosition)
    {
        PlayerPositions.Add(newPosition);
        AdvanceTime(1);
        EmitSignal(SignalName.PlayerPositionChanged, newPosition);
    }
    
    public void AdvanceTime(int amount)
    {
        GameTime += amount;

        for (int i = 0; i < amount; i++)
        {
            EmitSignal(SignalName.GameTicked);
        }
    }

	public void AdvancePhase(){
		GamePhase++;
	}

	public void ResetGameTime(){
		GameTime = 0;
	}

	public void ResetGamePhase(){
		GamePhase = 0;
	}
	
}

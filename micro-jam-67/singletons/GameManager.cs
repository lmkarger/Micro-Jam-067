using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class GameManager : Node
{
    public int NightDuration = 8;
    public int DayDuration = 2;

    /// <summary>
    /// The event fired when the game tick is updated (upon player movement).
    /// </summary>
    [Signal]
    public delegate void GameTickedEventHandler();

    [Signal]
    public delegate void PlayerPositionChangedEventHandler(Vector2I newPlayerPosition);

    [Signal]
    public delegate void GameEndedEventHandler(bool win);

    [Signal]
    public delegate void NextLevelSelectedEventHandler();

    [Signal]
    public delegate void PortalTouchedEventHandler(Vector2I teleportPosition);

    [Signal]
    public delegate void GameBeganEventHandler();

    public static GameManager Instance { get; private set; }

    private int _gameTime;
    public List<Vector2I> PlayerPositions = new();

    public bool GameOver { get; private set; }

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
    }

    public bool IsDay()
    {
        return GameTime >= NightDuration;
    }

    public void EndGame(bool win)
    {
        if (GameOver) return;

        GameOver = true;
        EmitSignal(SignalName.GameEnded, win);
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

    public void AdvancePhase()
    {
        GamePhase++;
    }
    
    public void ResetState()
    {
        GameTime = 0;
        GamePhase = 0;
        PlayerPositions.Clear();
        GameOver = false;
    }

    public void ResetGame()
    {
        ResetState();
        GetTree().ReloadCurrentScene();
    }

    public void SignalNextLevel()
    {
        EmitSignal(SignalName.NextLevelSelected);
    }
    
    public void ChangeLevel(string levelPath)
    {
        GetTree().ChangeSceneToFile(levelPath);
        ResetState();
    }
}

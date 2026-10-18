using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class GameManager : Node
{
    /// <summary>
    /// The event fired when the game tick is updated (upon player movement).
    /// </summary>
    [Signal]
    public delegate void GameTickedEventHandler();

    public static GameManager Instance { get; private set; }

    public static int GameTime = 0;
	public static int GamePhase = 1;

	public override void _Ready(){
        Instance = this;
	}

	public void AdvanceTime(int amount){
		GameTime += amount;
        EmitSignal(SignalName.GameTicked);
	}

	public static void AdvancePhase(){
		GamePhase++;
	}

	public static void ResetGameTime(){
		GameTime = 0;
	}

	public static void ResetGamePhase(){
		GamePhase = 0;
	}
	
}

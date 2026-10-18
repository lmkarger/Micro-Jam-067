using Godot;
using System;

public partial class GameManager : Node
{

	public static int GameTime = 0;
	public static int GamePhase = 1;

	public override void _Ready(){
		CallDeferred(nameof(InitializeGame));
	}

	private void InitializeGame()
	{
    BulletManager.Instance.GenerateBullets(GamePhase);
	}

	public static void AdvanceTime(int amount){
		GameTime += amount;
		BulletManager.Instance.MoveBullets();
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

using Godot;
using System;
using System.Linq;

public partial class BulletManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public static BulletManager Instance { get; private set; }
	private Bullet[] bulletArr = Array.Empty<Bullet>();

	public override void _Ready()
    {
        Instance = this;
    }
    
	public void GenerateBullets(int phase){
		if (phase == 1){
			Bullet bullet = new Bullet();
			AddChild(bullet);
			bullet.CreateBullet(new Vector2I(-10, 1), Vector2I.Right);
			GD.Print("create bullets");
            bulletArr = bulletArr
                .Append(bullet)
                .ToArray();
		}
	}

	public void MoveBullets(){
		for (int i = 0; i < bulletArr.Length; i++){
			GD.Print(bulletArr[i].getDirection());
			bulletArr[i].Move(bulletArr[i].getDirection());
		}
	}
}

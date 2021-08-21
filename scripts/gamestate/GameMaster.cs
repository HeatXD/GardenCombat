using System;
using Godot;
using AF = Abacus.Fixed64Precision;
public class GameMaster : Node2D
{
	private GameState GS;
	private AF.Fixed64 Time;
	private AF.Fixed64 DeltaTime;
	private DynamicFont DrawFont;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var localID = GetNode("/root/NetworkGlobals").Get("player_id");
		this.GS = new GameState(Maps.GetLevel1(), 2, Convert.ToByte(localID));
		this.Time = 0.0;
		this.DeltaTime = 1.0 / 60.0;
		this.DrawFont = new DynamicFont();

		DrawFont.FontData = ResourceLoader.Load("res://assets/fonts/Roboto-Bold.ttf") as DynamicFontData;
		DrawFont.Size = 16;
	}
	public override void _Process(float delta)
	{
		Time += delta;
		if (Time >= DeltaTime)
		{
			GS.Update(DeltaTime);
			Time -= DeltaTime;
			//Draw Game State
			Update();
		}
	}
	public override void _Draw()
	{
		DrawGameState();
	}

	private void DrawGameState()
	{
		DrawGameTiles();
		DrawPlayers();
		DrawTools();
	}

	private void DrawTools()
	{
		GD.Print("Draw Tools");
	}

	private void DrawPlayers()
	{
		for (int i = 0; i < GS.Players.Length; i++)
		{
			Vector2 playerPos = new Vector2(GS.Players[i].Position.X.ToSingle(), GS.Players[i].Position.Y.ToSingle());
			DrawCircle(playerPos, 30, Colors.Black);
			DrawString(DrawFont, playerPos, Convert.ToString(GS.Players[i].ID), Colors.White);
		}
	}

	private void DrawGameTiles()
	{
		for (int i = 0; i < GS.GameMapData.Height; i++)
		{
			for (int j = 0; j < GS.GameMapData.Width; j++)
			{
				switch (GS.GameMap[i, j].TileType)
				{
					case MapTileType.DIRT:
						DrawCircle(new Vector2((j + 1) * 100, (i + 1) * 100), 50, Colors.SandyBrown);
						break;
					case MapTileType.GRASS:
						DrawCircle(new Vector2((j + 1) * 100, (i + 1) * 100), 50, Colors.LightGreen);
						break;
					case MapTileType.WATER:
						DrawCircle(new Vector2((j + 1) * 100, (i + 1) * 100), 50, Colors.SkyBlue);
						break;
					case MapTileType.SEEDS:
						DrawCircle(new Vector2((j + 1) * 100, (i + 1) * 100), 50, Colors.Red);
						break;
					case MapTileType.FENCE:
						DrawCircle(new Vector2((j + 1) * 100, (i + 1) * 100), 50, Colors.Yellow);
						break;
					case MapTileType.PLANT_L:
						DrawCircle(new Vector2((j + 1) * 100, (i + 1) * 100), 50, Colors.PaleGreen);
						break;
					case MapTileType.PLANT_M:
						DrawCircle(new Vector2((j + 1) * 100, (i + 1) * 100), 50, Colors.LawnGreen);
						break;
					case MapTileType.PLANT_S:
						DrawCircle(new Vector2((j + 1) * 100, (i + 1) * 100), 50, Colors.ForestGreen);
						break;
					default: break;
				}
			}
		}
	}
}

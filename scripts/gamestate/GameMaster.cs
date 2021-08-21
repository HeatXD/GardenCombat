using Godot;
using AF = Abacus.Fixed64Precision;
public class GameMaster : Node2D
{
	private GameState gs;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AF.Vector2 v = new AF.Vector2(11.42, 16.44);
		GD.Print("Hello!");
		GD.Print(v);

		gs = new GameState(Maps.GetLevel1(), 2);
	}
	public override void _Draw()
	{
		//DrawPolyline(, new Vector2(50, 50)), Colors.Red);
		//DrawCircle(new Vector2(50, 50), 60, Colors.White);
		DrawGameState();
	}

	private void DrawGameState()
	{
		for (int i = 0; i < gs.GameMapData.Height; i++)
		{
			for (int j = 0; j < gs.GameMapData.Width; j++)
			{
				switch (gs.GameMap[i, j].TileType)
				{
					case MapTileType.DIRT:
						DrawCircle(new Vector2((i + 1) * 100, (j + 1) * 100), 50, Colors.SandyBrown);
						break;
					case MapTileType.GRASS:
						DrawCircle(new Vector2((i + 1) * 100, (j + 1) * 100), 50, Colors.LightGreen);
						break;
					case MapTileType.WATER:
						DrawCircle(new Vector2((i + 1) * 100, (j + 1) * 100), 50, Colors.SkyBlue);
						break;
					case MapTileType.SEEDS:
						DrawCircle(new Vector2((i + 1) * 100, (j + 1) * 100), 50, Colors.Red);
						break;
					default: break;
				}
			}
		}
	}
}

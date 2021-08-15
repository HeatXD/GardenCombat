using Godot;
using AF = Abacus.Fixed64Precision;
public class GameMaster : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AF.Vector2 v = new AF.Vector2(11.42,16.44);
		GD.Print("Hello!");
		GD.Print(v);

		GameState test = new GameState(Maps.GetLevel1(), 2);
	}
}

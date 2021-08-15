using AF = Abacus.Fixed64Precision;
public struct Player
{
	sbyte ID { get; set;} // ID of the player corresponds to the player's index in the array. an id shouldnt be negative
	AF.Vector2 Position { get; set; }
	AF.Vector2 Velocity { get; set; }
	AF.Vector2 Acceleration { get; set; }
	PlayerInput GameInput { get; set; }

	public void Tick(AF.Fixed64 dt){
		_processMotion(dt);
	}
	private void _processMotion(AF.Fixed64 dt){
		this.Velocity += this.Acceleration * dt;
		this.Position += this.Velocity * dt;
	}
}
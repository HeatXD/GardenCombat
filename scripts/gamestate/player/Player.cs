using Godot;
using AF = Abacus.Fixed64Precision;
public class Player
{
	public byte ID { get; set; } // ID of the player corresponds to the player's index in the array. an id shouldnt be negative
	public AF.Vector2 Position { get; set; }
	public AF.Vector2 Velocity { get; set; }
	public AF.Vector2 Acceleration { get; set; }
	public PlayerInput GameInput { get; set; }
	public int MoveSpeed { get; set; }
	public Player(byte ID)
	{
		this.ID = ID;
		this.Position = new AF.Vector2();
		this.Velocity = new AF.Vector2();
		this.Acceleration = new AF.Vector2();
		this.GameInput = new PlayerInput();
		this.MoveSpeed = 50;
	}
	public Player(Player p)
	{
		this.ID = p.ID;
		this.Position = p.Position;
		this.Velocity = p.Velocity;
		this.Acceleration = p.Acceleration;
		this.GameInput = p.GameInput;
		this.MoveSpeed = p.MoveSpeed;
	}
	public void Update(AF.Fixed64 dt, byte localID)
	{
		GetInput(localID);
		UseInput();
		ProcessMotion(dt);
		ResolveMapCollisions();
	}
	private void ResolveMapCollisions()
	{
		//GD.Print("Map Collisions");
	}
	private void ProcessMotion(AF.Fixed64 dt)
	{
		Velocity += Acceleration * dt;
		Position += Velocity * dt;
	}

	private void UseInput()
	{
		MovePlayer();
	}

	private void MovePlayer()
	{
		Velocity *= 0.9;

		var dir = new AF.Vector2();

		if (GameInput.IsKeyboardBitSet(0))
			dir += new AF.Vector2(0, -1);
		if (GameInput.IsKeyboardBitSet(1))
			dir += new AF.Vector2(0, 1);
		if (GameInput.IsKeyboardBitSet(2))
			dir += new AF.Vector2(-1, 0);
		if (GameInput.IsKeyboardBitSet(3))
			dir += new AF.Vector2(1, 0);

		Velocity += dir * MoveSpeed;
	}

	private void GetInput(byte localID)
	{
		GameInput.InputState = 0;
		if (ID == localID)
		{
			if (Input.IsActionPressed("move_up"))
				GameInput.SetKeyboardBit(0, true);
			if (Input.IsActionPressed("move_down"))
				GameInput.SetKeyboardBit(1, true);
			if (Input.IsActionPressed("move_left"))
				GameInput.SetKeyboardBit(2, true);
			if (Input.IsActionPressed("move_right"))
				GameInput.SetKeyboardBit(3, true);
			if (Input.IsActionPressed("use_item_1"))
				GameInput.SetKeyboardBit(4, true);
			if (Input.IsActionPressed("use_item_2"))
				GameInput.SetKeyboardBit(5, true);
			if (Input.IsActionPressed("use_item_3"))
				GameInput.SetKeyboardBit(6, true);
		}
		GD.Print(GameInput.InputState);
	}
}

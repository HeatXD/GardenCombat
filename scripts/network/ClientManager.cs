using Godot;
using GC = Godot.Collections;

public class ClientManager : Node
{
	public GameWebRTCClient Client;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Client = new GameWebRTCClient();
		AddChild(Client);
		Client.ConnectToWebSocket("ws://localhost:6755");
		Client.Connect("GameNetCTS", this, nameof(OnServerConnect));
		Client.Connect("GameNetRPK", this, nameof(OnPlayerRegister));
	}

	public void OnPlayerRegister(GC.Dictionary info)
	{
		GD.Print("Player Registered!");
	}

	public void OnServerConnect()
	{
		Client.SendCommand(Client.CreateCommand("###!RP", "HeatXDBot"));
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Client.RTCMP.Poll();
	}
	private void _on_Create_pressed()
	{
		Client.SendCommand(Client.CreateCommand("###!CR", "null"));
	}
	private void _on_Join_pressed()
	{
		Client.SendCommand(Client.CreateCommand("###!JR", GetParent().GetNode<TextEdit>("TextBox").Text));
	}
}


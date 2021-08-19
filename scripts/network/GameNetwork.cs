using Godot;
using System;
using GC = Godot.Collections;

public class GameNetwork : Node
{
	[Signal] public delegate void GameNetConnected(GC.Dictionary info); // Network Connected to Signaling Server
	[Signal] public delegate void GameNetClosed(GC.Dictionary info); // Network Disconnected from the Signaling Server
	[Signal] public delegate void GameNetRPK(GC.Dictionary info); // Register Player Ok
	[Signal] public delegate void GameNetRPB(GC.Dictionary info); // Register Player Bad
	[Signal] public delegate void GameNetCRK(GC.Dictionary info); // Create Room  OK 
	[Signal] public delegate void GameNetCRB(GC.Dictionary info); // Create Room  BAD  
	[Signal] public delegate void GameNetJRK(GC.Dictionary info); // Join Room OK 
	[Signal] public delegate void GameNetJRB(GC.Dictionary info); // Join Room  Bad 
	[Signal] public delegate void GameNetSRK(GC.Dictionary info); // Seal Room OK
	[Signal] public delegate void GameNetSRB(GC.Dictionary info); // Seal Room BAD
	[Signal] public delegate void GameNetRMK(GC.Dictionary info); // Room Message OK  
	[Signal] public delegate void GameNetRMB(GC.Dictionary info); // Room Message BAD  
	[Signal] public delegate void GameNetICB(GC.Dictionary info); // Issued Command BAD   
	[Signal] public delegate void GameNetPTO(GC.Dictionary info); // Player Timed Out 

	public const int ServerID = 1;
	public WebSocketClient Client;
	public string WebSocketURL;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.WebSocketURL = "ws://localhost:6755";
		this.Client = new WebSocketClient();

		Client.Connect("connection_error", this, nameof(OnClosed));
		Client.Connect("connection_closed", this, nameof(OnClosed));
		Client.Connect("connection_established", this, nameof(OnConnected));
		Client.Connect("data_received", this, nameof(OnData));

		var err = Client.ConnectToUrl(WebSocketURL);
		if (err != Error.Ok)
		{
			GD.Print("Unable to connect!");
			SetProcess(false);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Client.Poll();
	}

	public void OnConnected(string proto)
	{
		GD.Print("Connected to the server!");
		//Client.GetPeer(ServerID).PutPacket("Hello from client ");
		var msg = CreateCommand("###!RP", "HeatXD");
		SendCommand(msg);
		// /Client.GetPeer(ServerID).PutPacket("###!CR".ToUTF8());
	}

	public void OnClosed(bool wasClean)
	{
		GD.Print("Connection to the server closed! Connection Closed Cleanly? : " + wasClean);
		SetProcess(false);
	}

	public void OnData()
	{
		var data = Client.GetPeer(ServerID).GetPacket().GetStringFromUTF8();
		var message = JSON.Parse(data).Result;
		GC.Dictionary parsed = message as GC.Dictionary;
		HandleCommand(parsed);
		//GD.Print(parsed["command"]);
	}

	public void HandleCommand(GC.Dictionary message)
	{
		string CMD = Convert.ToString(message["command"]).Substring(4, 3);
		//GD.Print(CMD);
		switch (CMD)
		{
			case "RPK":
				GD.Print("RPK: " + Convert.ToString(message["data"]));
				EmitSignal(nameof(GameNetRPK),message);
				break;
			case "RPB":
				EmitSignal(nameof(GameNetRPB),message);
				break;
			case "CRK":
				EmitSignal(nameof(GameNetCRK),message);
				break;
			case "CRB":
				EmitSignal(nameof(GameNetCRB),message);
				break;
			case "JRK":
				EmitSignal(nameof(GameNetJRK),message);
				break;
			case "JRB":
				EmitSignal(nameof(GameNetJRB),message);
				break;
			case "SRK":
				EmitSignal(nameof(GameNetSRK),message);
				break;
			case "SRB":
				EmitSignal(nameof(GameNetSRB),message);
				break;
			case "RMK":
				EmitSignal(nameof(GameNetRMK),message);
				break;
			case "RMB":
				EmitSignal(nameof(GameNetRMB),message);
				break;
			case "ICB":
				EmitSignal(nameof(GameNetICB),message);
				break;
			case "PTO":
				EmitSignal(nameof(GameNetPTO),message);
				break;
			default:
				GD.Print("Unknown Server Command");
				break;
		}
	}

	public GC.Dictionary CreateCommand<T>(string command, T data)
	{
		GC.Dictionary msg = new GC.Dictionary();
		msg.Add("command", command);
		msg.Add("data", data);
		return msg;
	}

	public void SendCommand(GC.Dictionary message){
		Client.GetPeer(ServerID).PutPacket(JSON.Print(message).ToUTF8());
	}
}

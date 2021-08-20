/*
	In These Commands <> are playerholders
	PLAYER COMMANDS                                   MESSAGE DATA
	----------------------------------------------------------------------------
	Register Player     \  ###!RP   \                 <PlayerName>
	Create Room         \  ###!CR   \                    'null'
	Join Room           \  ###!JR   \                  <RoomUUID>
	Seal Room           \  ###!SR   \                    'null'
	Send Message        \  ###!SM   \                  <Message>
	Send Offer          \  ###!OO   \      Json{id:<RecipientID>,rtc:<WebRTCData>}
	Send Answer         \  ###!AA   \      Json{id:<RecipientID>,rtc:<WebRTCData>}
	Send Candidate      \  ###!CC   \      Json{id:<RecipientID>,rtc:<WebRTCData>}
	----------------------------------------------------------------------------
	SERVER RESPONSES
	----------------------------------------------------------------------------
	Register Player OK      \ ###!RPK   \             <PlayerID>
	Register Player BAD     \ ###!RPB   \              <Reason>
	Create Room  OK         \ ###!CRK   \               <Room>
	Create Room  BAD        \ ###!CRB   \              <Reason>
	Join Room  OK           \ ###!JRK   \               <Room>    
	Join Room  BAD          \ ###!JRB   \              <Reason> 
	Connected To Room       \ ###!CTR   \             <PlayerID>
	Room Peer Connected     \ ###!RPC   \             <PlayerID>
	Room Peer Disconnected  \ ###!RPD   \             <PlayerID>
	Seal Room OK            \ ###!SRK   \               'null'
	Seal Room BAD           \ ###!SRB   \              <Reason>
	Room Message OK         \ ###!RMK   \              <Message>
	Room Message BAD        \ ###!RMB   \              <Reason>
	Issued Command BAD      \ ###!ICB   \              <Reason>
	Player Timed Out        \ ###!PTO   \              <Reason>
	Recieve Offer           \ ###!OOO   \   Json{id:<SenderID>,rtc:<WebRTCData>}
	Recieve Answer          \ ###!AAA   \   Json{id:<SenderID,rtc:<WebRTCData>}
	Recieve Candidate       \ ###!CCC   \   Json{id:<SenderID>,rtc:<WebRTCData>}

*/
using Godot;
using System;
using GC = Godot.Collections;

public class GameNetwork : Node
{
	[Signal] public delegate void GameNetCTS(); // Network Connected to Signaling Server
	[Signal] public delegate void GameNetError(); // Network Error Disconnected from the Signaling Server
	[Signal] public delegate void GameNetClosed(bool wasClean); // Network Disconnected from the Signaling Server
	[Signal] public delegate void GameNetCTR(GC.Dictionary info); // Network Connected to a Room
	[Signal] public delegate void GameNetRPK(GC.Dictionary info); // Register Player Ok
	[Signal] public delegate void GameNetRPB(GC.Dictionary info); // Register Player Bad
	[Signal] public delegate void GameNetCRK(GC.Dictionary info); // Create Room  OK 
	[Signal] public delegate void GameNetCRB(GC.Dictionary info); // Create Room  BAD  
	[Signal] public delegate void GameNetJRK(GC.Dictionary info); // Join Room  OK 
	[Signal] public delegate void GameNetJRB(GC.Dictionary info); // Create Room  BAD  
	[Signal] public delegate void GameNetRPC(GC.Dictionary info); // Room Peer Connected   
	[Signal] public delegate void GameNetRPD(GC.Dictionary info); // Room Peer Disconnected 
	[Signal] public delegate void GameNetSRK(GC.Dictionary info); // Seal Room OK
	[Signal] public delegate void GameNetSRB(GC.Dictionary info); // Seal Room BAD
	[Signal] public delegate void GameNetRMK(GC.Dictionary info); // Room Message OK  
	[Signal] public delegate void GameNetRMB(GC.Dictionary info); // Room Message BAD  
	[Signal] public delegate void GameNetICB(GC.Dictionary info); // Issued Command BAD   
	[Signal] public delegate void GameNetPTO(GC.Dictionary info); // Player Timed Out 
	[Signal] public delegate void GameNetOOO(GC.Dictionary info); // Offer Received
	[Signal] public delegate void GameNetAAA(GC.Dictionary info); // Answer Received
	[Signal] public delegate void GameNetCCC(GC.Dictionary info); // Candidate Received

	public const int ServerID = 1;
	public WebSocketClient Client;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Client = new WebSocketClient();
		Client.Connect("connection_error", this, nameof(OnError));
		Client.Connect("connection_closed", this, nameof(OnClosed));
		Client.Connect("connection_established", this, nameof(OnConnected));
		Client.Connect("data_received", this, nameof(OnData));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var status = Client.GetConnectionStatus();
		if (status == WebSocketClient.ConnectionStatus.Connecting || status == WebSocketClient.ConnectionStatus.Connected)
		{
			Client.Poll();
		}
	}
	public void ConnectToWebSocket(string url)
	{
		if (Client.GetConnectionStatus() == WebSocketClient.ConnectionStatus.Connected)
		{
			Client.DisconnectFromHost(1000, "Disconnect By Client");
		}

		var err = Client.ConnectToUrl(url);
		if (err != Error.Ok)
		{
			GD.Print("Unable to connect To Server");
		}
	}
	public void OnError()
	{
		EmitSignal(nameof(GameNetError));
	}

	public void OnConnected(string proto)
	{
		Client.GetPeer(ServerID).SetWriteMode(WebSocketPeer.WriteMode.Text);
		EmitSignal(nameof(GameNetCTS));
	}

	public void OnClosed(bool wasClean = false) => EmitSignal(nameof(GameNetClosed), wasClean);

	public void OnData()
	{
		var data = Client.GetPeer(ServerID).GetPacket().GetStringFromUTF8();
		var message = JSON.Parse(data).Result;
		GC.Dictionary parsed = message as GC.Dictionary;
		HandleCommand(parsed);
	}

	public void HandleCommand(GC.Dictionary message)
	{
		GD.Print(Convert.ToString(message["command"]));
		string CMD = Convert.ToString(message["command"]).Substring(4, 3);
		switch (CMD)
		{
			case "CTR":
				EmitSignal(nameof(GameNetCTR), message);
				break;
			case "RPK":
				EmitSignal(nameof(GameNetRPK), message);
				break;
			case "RPB":
				EmitSignal(nameof(GameNetRPB), message);
				break;
			case "CRK":
				EmitSignal(nameof(GameNetCRK), message);
				break;
			case "CRB":
				EmitSignal(nameof(GameNetCRB), message);
				break;
			case "JRK":
				EmitSignal(nameof(GameNetJRK), message);
				break;
			case "JRB":
				EmitSignal(nameof(GameNetJRB), message);
				break;
			case "RPC":
				EmitSignal(nameof(GameNetRPC), message);
				break;
			case "RPD":
				EmitSignal(nameof(GameNetRPD), message);
				break;
			case "SRK":
				EmitSignal(nameof(GameNetSRK), message);
				break;
			case "SRB":
				EmitSignal(nameof(GameNetSRB), message);
				break;
			case "RMK":
				EmitSignal(nameof(GameNetRMK), message);
				break;
			case "RMB":
				EmitSignal(nameof(GameNetRMB), message);
				break;
			case "ICB":
				EmitSignal(nameof(GameNetICB), message);
				break;
			case "PTO":
				EmitSignal(nameof(GameNetPTO), message);
				break;
			case "OOO":
				GD.PrintErr("OOO Recieved");
				EmitSignal(nameof(GameNetOOO), message);
				break;
			case "AAA":
				GD.PrintErr("AAA Recieved");
				EmitSignal(nameof(GameNetAAA), message);
				break;
			case "CCC":
				GD.Print("CCC Recieved");
				EmitSignal(nameof(GameNetCCC), message);
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

	public void SendCommand(GC.Dictionary message)
	{
		if (Client.GetConnectionStatus() == WebSocketClient.ConnectionStatus.Connected)
		{
			Client.GetPeer(ServerID).PutPacket(JSON.Print(message).ToUTF8());
		}
		else
		{
			GD.PrintErr("Not Connected To Signaling Server");
		}
	}

	public void SendOffer(int id, string offer)
	{
		var data = new GC.Dictionary();
		data.Add("id", id);
		data.Add("offer", offer);
		var msg = CreateCommand("###!OO", data);
		GD.PrintErr("Send Offer");
		SendCommand(msg);
	}
	public void SendAnswer(int id, string answer)
	{
		var data = new GC.Dictionary();
		data.Add("id", id);
		data.Add("answer", answer);
		var msg = CreateCommand("###!AA", data);
		GD.PrintErr("Send Answer");
		SendCommand(msg);
	}
	public void SendCandidate(int id, string mid, int index, string sdpNname)
	{
		var data = new GC.Dictionary();
		data.Add("id", id);
		data.Add("mid", mid);
		data.Add("index", index);
		data.Add("sdp", sdpNname);
		var msg = CreateCommand("###!CC", data);
		GD.PrintErr("Send Candidate");
		SendCommand(msg);
	}
}

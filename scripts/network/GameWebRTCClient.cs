using System;
using Godot;
using GC = Godot.Collections;

public class GameWebRTCClient : GameNetwork
{
	public WebRTCMultiplayer RTCMP;

	public override void _Ready()
	{
		base._Ready();

		this.RTCMP = new WebRTCMultiplayer();

		this.Connect("GameNetCTS", this, nameof(OnServerConnect));
		this.Connect("GameNetCTR", this, nameof(OnRoomConnect));
		this.Connect("GameNetClosed", this, nameof(OnDisconnect));
		this.Connect("GameNetError", this, nameof(OnServerError));

		this.Connect("GameNetOOO", this, nameof(OnOfferReceived));
		this.Connect("GameNetAAA", this, nameof(OnAnswerReceived));
		this.Connect("GameNetCCC", this, nameof(OnCandidateReceived));

		this.Connect("GameNetRPK", this, nameof(OnRegisterOk));
		this.Connect("GameNetRPB", this, nameof(OnRegisterBad));
		this.Connect("GameNetCRK", this, nameof(OnCreateRoomOk));
		this.Connect("GameNetCRB", this, nameof(OnCreateRoomBad));

		this.Connect("GameNetJRK", this, nameof(OnRoomJoinedOk));
		this.Connect("GameNetJRB", this, nameof(OnRoomJoinedBad));
		this.Connect("GameNetRPC", this, nameof(OnRoomPeerConnected));
		this.Connect("GameNetRPD", this, nameof(OnRoomPeerDisconnected));

		this.Connect("GameNetSRK", this, nameof(OnRoomSealedOk));
		this.Connect("GameNetSRB", this, nameof(OnRoomSealedBad));
		this.Connect("GameNetRMK", this, nameof(OnRoomMessageOk));
		this.Connect("GameNetRMB", this, nameof(OnRoomMessageBad));

		this.Connect("GameNetICB", this, nameof(OnInvalidCommand));
		this.Connect("GameNetPTO", this, nameof(OnPlayerTimedOut));
	}

	public void OnServerError()
	{
		GD.Print("Error Connecting to server");
	}

	public void OnPlayerTimedOut(GC.Dictionary info)
	{
		GD.Print("Player Timed Out. Reason: " + Convert.ToString(info["data"]));
	}

	public void OnInvalidCommand(GC.Dictionary info)
	{
		GD.PrintErr("Invalid Command. Reason: " + Convert.ToString(info["data"]));
	}

	public void OnRoomMessageBad(GC.Dictionary info)
	{
		GD.PrintErr("Room Message Error. Reason: " + Convert.ToString(info["data"]));
	}

	public void OnRoomMessageOk(GC.Dictionary info)
	{
		GD.Print("Room Message Ok. Received: " + Convert.ToString(info["data"]));
	}

	public void OnRoomSealedBad(GC.Dictionary info)
	{
		GD.PrintErr("Room Seal Error. Reason: " + Convert.ToString(info["data"]));
	}

	public void OnRoomSealedOk(GC.Dictionary info)
	{
		GD.Print("Room Seal Ok. Received: " + Convert.ToString(info["data"]));
	}

	public void OnRoomPeerDisconnected(GC.Dictionary info)
	{
		int id = Convert.ToInt32(info["data"]);
		if (RTCMP.HasPeer(id))
		{
			GD.Print("Peer: " + id + " disconnected");
			RTCMP.RemovePeer(id);
		}
	}

	public void OnRoomPeerConnected(GC.Dictionary info)
	{
		int id = Convert.ToInt32(info["data"]);
		GD.Print("Peer Connected: " + id);
		CreatePeer(id);
	}

	public void OnRoomJoinedBad(GC.Dictionary info)
	{
		GD.PrintErr("Room Join Error. Reason: " + Convert.ToString(info["data"]));
	}

	public void OnRoomJoinedOk(GC.Dictionary info)
	{
		GD.Print("Room Join Ok. Received: " + Convert.ToString(info["data"]));
	}

	public void OnCreateRoomBad(GC.Dictionary info)
	{
		GD.PrintErr("Create Room Error. Reason: " + Convert.ToString(info["data"]));
	}

	public void OnCreateRoomOk(GC.Dictionary info)
	{
		GD.Print("Room Created Ok. Received: " + Convert.ToString(info["data"]));
	}

	public void OnRegisterBad(GC.Dictionary info)
	{
		GD.PrintErr("Player Register Error. Reason: " + Convert.ToString(info["data"]));
	}

	public void OnRegisterOk(GC.Dictionary info)
	{
		GD.Print("Player Register Ok. Received: " + Convert.ToInt32(info["data"]));
	}

	public void OnServerConnect()
	{
		GD.Print("Client Connected To Signaling Server!");
	}

	public void OnDisconnect(bool wasClean)
	{
		if (wasClean)
			GD.Print("Good Server Disconnect");
		else
			GD.PrintErr("Bad Server Disconnect");
	}
	public void OnRoomConnect(GC.Dictionary info)
	{
		int id = Convert.ToInt32(info["data"]);
		GD.Print("Connected to room with ID: " + id);
		var err = RTCMP.Initialize(id, true);
		if (err != Error.Ok)
		{
			GD.PrintErr("Couldn't init RTCMP");
		}
	}
	public void OnOfferReceived(GC.Dictionary info)
	{
		GC.Dictionary data = info["data"] as GC.Dictionary;
		int id = Convert.ToInt32(data["id"]); ;
		string offer = Convert.ToString(data["offer"]);
		GD.Print("Got offer: " + id);
		if (RTCMP.HasPeer(id))
		{
			GD.PrintErr("OFFER");
			GD.PrintErr(RTCMP.GetPeers());
			WebRTCPeerConnection conn = (WebRTCPeerConnection)RTCMP.GetPeer(id)["connection"];
			conn.SetRemoteDescription("offer", offer);
		}
	}
	public void OnAnswerReceived(GC.Dictionary info)
	{
		GC.Dictionary data = info["data"] as GC.Dictionary;
		int id = Convert.ToInt32(data["id"]);
		string answer = Convert.ToString(data["answer"]);
		GD.Print("Got answer: " + id);
		if (RTCMP.HasPeer(id))
		{
			GD.PrintErr("ANSWER");
			WebRTCPeerConnection conn = (WebRTCPeerConnection)RTCMP.GetPeer(id)["connection"];
			conn.SetRemoteDescription("answer", answer);
		}
	}
	public void OnCandidateReceived(GC.Dictionary info)
	{
		GC.Dictionary data = info["data"] as GC.Dictionary;
		int id = Convert.ToInt32(data["id"]);
		string mid = Convert.ToString(data["mid"]);
		int index = Convert.ToInt32(data["index"]);
		string sdp = Convert.ToString(data["sdp"]);
		GD.Print("Got Candidate");
		if (RTCMP.HasPeer(id))
		{
			GD.PrintErr("CANDIDATE");
			WebRTCPeerConnection conn = (WebRTCPeerConnection)RTCMP.GetPeer(id)["connection"];
			conn.AddIceCandidate(mid, index, sdp);
		}
	}
	public void CreatePeer(int id)
	{
		var peer = new WebRTCPeerConnection();
		var settings = new GC.Dictionary();
		var urls = new GC.Dictionary();
		var stuns = new string[1];
		var param = new GC.Array();

		stuns[0] = "stun:stun.l.google.com:19302";
		urls.Add("urls", stuns);
		var stunUrls = new GC.Array();
		stunUrls.Add(urls);
		GD.Print(JSON.Print(stunUrls));
		settings.Add("iceServers", JSON.Print(stunUrls));
		GD.Print(JSON.Print(settings));
		param.Add(id);

		var err = peer.Initialize(settings);

		if (err != Error.Ok)
		{
			GD.PrintErr("Couldn't Initialize Peer");
			return;
		}

		peer.Connect("session_description_created", this, nameof(OnOfferCreated), param);
		peer.Connect("ice_candidate_created", this, nameof(OnIceCreated), param);

		err = RTCMP.AddPeer(peer, id);
		GD.PrintErr(RTCMP.GetPeers());

		if (err != Error.Ok)
		{
			GD.PrintErr("Couldn't Add Peer To RTCMP");
			return;
		}

		if (id > RTCMP.GetUniqueId())
		{
			GD.Print("ok");
			peer.CreateOffer();
		}
	}
	public void OnOfferCreated(string type, string sdp, int id)
	{
		if (!RTCMP.HasPeer(id))
		{
			GD.Print("RTCMP Already has peer");
			return;
		}

		GC.Dictionary info = RTCMP.GetPeer(id);
		WebRTCPeerConnection conn = (WebRTCPeerConnection)info["connection"];
		GD.Print(conn);
		conn.SetLocalDescription(type, sdp);

		if (type == "offer")
		{
			GD.Print("created: " + type);
			SendOffer(id, sdp);
		}
		else
		{
			GD.Print("created: " + type);
			SendAnswer(id, sdp);
		}
	}
	public void OnIceCreated(string mid, int index, string sdpName, int id) => SendCandidate(id, mid, index, sdpName);
}

using Godot;
using System;
using AF = Abacus.Fixed64Precision;

public struct GameState
{	
	public int Frame;
	public TileState[,] GameMap;
	public Maps.MapData GameMapData;
	public Player[] Players;
	public byte LocalID;
	public GameState(Maps.MapData mapData, byte playerCount, byte localID)
	{
		//create 2d array with specified width and height
		this.GameMap = new TileState[mapData.Width, mapData.Height];
		this.GameMapData = mapData;
		this.Players = new Player[playerCount];
		this.LocalID = localID;
		this.Frame = 0;

		for (byte i = 0; i < Players.Length; i++)
		{
			this.Players[i] = new Player(i);
		}

		this.GameMap = _generateMap(mapData);
	}

	public void Update(AF.Fixed64 dt)
	{
		_updateState(dt);
	}

	private void _updateState(AF.Fixed64 dt)
	{
		for (int i = 0; i < Players.Length; i++)
		{
			Players[i].Update(dt, LocalID);
		}
	}

	private TileState[,] _generateMap(Maps.MapData mapData)
	{
		TileState[,] map = new TileState[mapData.Width, mapData.Height];
		char[] charMap = mapData.MapString.ToCharArray();

		int mapIndex = 0;
		for (int i = 0; i < mapData.Height; i++)
		{
			for (int j = 0; j < mapData.Width; j++)
			{
				char currentChar = charMap[mapIndex];
				MapTileType tileType = _getTileType(currentChar);
				map[i, j] = new TileState(tileType, false);

				//GD.Print(map[i, j].ToString());
				mapIndex++;
			}
		}
		return map;
	}

	private MapTileType _getTileType(char character)
	{
		int convertedChar = Convert.ToInt32(new string(character, 1));
		return (MapTileType)convertedChar;
	}
}


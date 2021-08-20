using Godot;
using System;
using AF = Abacus.Fixed64Precision;

public struct GameState
{
    public TileState[,] GameMap;
    public Player[] Players;
    public GameState(Maps.MapData mapData, byte playerCount)
    {
        //create 2d array with specified width and height
        this.GameMap = new TileState[mapData.Width, mapData.Height];
        this.Players = new Player[playerCount];

        for (sbyte i = 0; i < Players.Length; i++)
        {
            this.Players[i].ID = i;
        }

        this.GameMap = _generateMap(mapData);
    }

    public void MainLoop(AF.Fixed64 dt)
    {
        _updateState(dt);
        _drawState(dt);
    }

    private void _updateState(AF.Fixed64 dt)
    {
        for (int i = 0; i < Players.Length; i++)
        {
			Players[i].GetInput();
			Players[i].Update(dt);
        }
    }

    private void _drawState(AF.Fixed64 dt)
    {
        throw new NotImplementedException();
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

                GD.Print(map[i, j].ToString());
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


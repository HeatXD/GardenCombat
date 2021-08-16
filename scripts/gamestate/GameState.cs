using Godot;
using System;

public struct GameState
{
    public TileState[,] GameMap;
    public Player[] Players;
    public GameState(Maps.MapData mapData, int playerCount)
    {
        //create 2d array with specified width and height
        this.GameMap = new TileState[mapData.Width, mapData.Height];
        this.Players = new Player[playerCount];

        this.GameMap = _generateMap(mapData);
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


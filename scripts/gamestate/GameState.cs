public struct GameState
{
	TileState[,] GameMap;
	Player[] Players;
	public GameState(int width, int height, int playerCount)
	{
		//create 2d array with specified width and height
		this.GameMap = new TileState[width, height];
		this.Players = new Player[playerCount];
	}
}


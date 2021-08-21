public static class Maps
{
	public struct MapData
	{
		public int Width;
		public int Height;
		public string MapString;

		public MapData(int width, int height, string mapString)
		{
			this.Width = width;
			this.Height = height;
			this.MapString = mapString;
		}
	}
	public static MapData GetLevel1()
	{
		return new MapData(5, 5, "0123456701234567561000000");
	}
}

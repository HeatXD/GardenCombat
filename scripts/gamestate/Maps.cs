public static class Maps
{
    public struct MapData
    {
        public int Width;
        public int Height;
        public string MapString;

        public MapData(int width, int height, string mapString)
        {
            Width = width;
            Height = height;
            MapString = mapString;
        }
    }
    public static MapData GetLevel1()
    {
        return new MapData(5, 5, "0000001110011100561000000");
    }
}
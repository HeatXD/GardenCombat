public struct TileState
{
    MapTileType TileType { get; set; }
    sbyte OwnedBy { get; set; }

    public TileState(MapTileType tileType, bool isStatic)
    {
        if (isStatic) // Static tiles cannot be own by a player
            this.OwnedBy = -2; //  -2 tiles cannot be owned by a player
        else
            this.OwnedBy = -1; // -1 indicates noone owns it yet 
        this.TileType = tileType;
    }
}
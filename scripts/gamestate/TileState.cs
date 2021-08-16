using System;

public struct TileState
{
    public MapTileType TileType { get; set; }
    public sbyte OwnedBy { get; set; }

    public TileState(MapTileType tileType, bool isStatic)
    {
        this.TileType = tileType;

        if (isStatic || TileType == MapTileType.WATER || TileType == MapTileType.SEEDS || TileType == MapTileType.FENCE) // Static tiles cannot be own by a player
            this.OwnedBy = -2; //  -2 tiles cannot be owned by a player
        else
            this.OwnedBy = -1; // -1 indicates noone owns it yet 
    }

    public override string ToString()
    {
        return String.Format("{0}, {1}", TileType, OwnedBy);
    }
}
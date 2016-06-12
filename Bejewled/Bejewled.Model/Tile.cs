namespace Bejewled.Model
{
    using Bejewled.Model.Enums;
    using Bejewled.Model.Interfaces;

    public class Tile : ITile
    {
        public Tile(TileType tileType, TilePosition position)
        {
            this.TileType = tileType;
            this.Position = position;
        }

        public TilePosition Position { get; set; }

        public TileType TileType { get; private set; }
    }
}
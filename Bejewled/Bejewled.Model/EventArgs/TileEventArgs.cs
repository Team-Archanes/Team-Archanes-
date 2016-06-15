namespace Bejewled.Model.EventArgs
{
    using System;

    public class TileEventArgs : EventArgs
    {
        public TileEventArgs(int firstTileX, int firstTileY, int secondTileX, int secondTileY)
        {
            this.FirstTileX = firstTileX;
            this.FirstTileY = firstTileY;
            this.SecondTileX = firstTileX;
            this.SecondTileY = firstTileY;
        }

        public int FirstTileX { get; set; }

        public int FirstTileY { get; set; }

        public int SecondTileX { get; set; }

        public int SecondTileY { get; set; }
    }
}


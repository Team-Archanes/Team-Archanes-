namespace Bejewled.Model.EventArgs
{
    using System;

    public class TileEventArgs : EventArgs
    {
        public TileEventArgs(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }
    }
}


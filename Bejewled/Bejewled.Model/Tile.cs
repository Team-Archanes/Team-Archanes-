namespace Bejewled.Model
{
    using Bejewled.Model.Interfaces;

    public class Tile : ITile
    {
        private int width;

        private int height;

        public Tile(int width, int height)
        {
            
        }

        public int Width
        {
            get
            {
                return this.width;
            }
            private set
            {
                //todo: add validation
                this.width = value;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }
            private set
            {
                //todo: add validation
                this.height = value;
            }
        }
    }
}
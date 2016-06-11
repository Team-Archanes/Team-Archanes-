namespace Bejewled.Model
{
    using Bejewled.Model.Interfaces;

    public class GameBoard : IGameBoard
    {
        private const int NumberOfRows = 8;

        private const int NumberOfColumn = 8;

        private Tile[,] gameBoard = new Tile[NumberOfRows, NumberOfColumn];

        public GameBoard()
        {
            
        }

        public void InitializeGameBoard()
        {
            //todo: add logic for first load of game board
        }

        public void CheckForMatch()
        {
            //todo: add logic for checking for match i.e at least tree equal tiles
            this.RemoveMatchedTiles();
        }

        private void MoveDownTiles()
        {
            // todo: add logic for movig down tiles which replaces the matched one
        }

        private void RemoveMatchedTiles()
        {
            //todo: logic for removing matched tiles
            this.MoveDownTiles();
        }
    }
}
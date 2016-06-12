namespace Bejewled.Model
{
    using System;

    using Bejewled.Model.Interfaces;

    public class GameBoard : IGameBoard
    {
        private const int NumberOfColumn = 8;

        private const int NumberOfRows = 8;

        private readonly ITile[,] gameBoard;

        private readonly TileGenerator tileGenerator;

        public GameBoard()
        {
            this.gameBoard = new ITile[NumberOfRows, NumberOfColumn];
            this.tileGenerator = new TileGenerator();
        }

        public void CheckForValidMove(ITile firstClickedTile, ITile secondClickedTile)
        {
            // todo: checks if move is valid
            // if move is valid we call SwapTiles
            this.SwapTiles(firstClickedTile, secondClickedTile);
        }

        public ITile[,] InitializeGameBoard()
        {
            for (var row = 0; row < this.gameBoard.GetLength(0); row++)
            {
                for (var column = 0; column < this.gameBoard.GetLength(1); column++)
                {
                    var tile = this.tileGenerator.CreateRandomTile(row, column);
                    if (row < 2 && column >= 2)
                    {
                        while (tile.TileType.Equals(this.gameBoard[row, column - 1].TileType)
                               && tile.TileType.Equals(this.gameBoard[row, column - 2].TileType))
                        {
                            tile = this.tileGenerator.CreateRandomTile(row, column);
                        }
                    }

                    if (row >= 2 && column < 2)
                    {
                        while (tile.TileType.Equals(this.gameBoard[row - 1, column].TileType)
                               && tile.TileType.Equals(this.gameBoard[row - 2, column].TileType))
                        {
                            tile = this.tileGenerator.CreateRandomTile(row, column);
                        }
                    }

                    if (row >= 2 && column >= 2)
                    {
                        while ((tile.TileType.Equals(this.gameBoard[row - 1, column].TileType)
                                && tile.TileType.Equals(this.gameBoard[row - 2, column].TileType))
                               || (tile.TileType.Equals(this.gameBoard[row, column - 1].TileType)
                                   && tile.TileType.Equals(this.gameBoard[row, column - 2].TileType)))
                        {
                            tile = this.tileGenerator.CreateRandomTile(row, column);
                        }
                    }

                    this.gameBoard[row, column] = tile;
                }
            }
            return this.gameBoard;
        }

        private void CheckForMatch()
        {
            // todo: add logic for checking for match i.e at least tree equal tiles
            this.RemoveMatchedTiles();
            throw new NotImplementedException();
        }

        private void MoveDownTiles()
        {
            // todo: add logic for movig down tiles which replaces the matched one
            throw new NotImplementedException();
        }

        private void RemoveMatchedTiles()
        {
            // todo: logic for removing matched tiles
            this.MoveDownTiles();
            throw new NotImplementedException();
        }

        private void SwapTiles(ITile firstClickedTile, ITile secondClickedTile)
        {
            // todo: add logic for swaping tiles
            this.CheckForMatch();
            throw new NotImplementedException();
        }
    }
}
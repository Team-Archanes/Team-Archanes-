namespace Bejewled.Model
{
    using System;
    using System.Collections.Generic;
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

        // Checks if move is valid
        public void CheckForValidMove(ITile firstClickedTile, ITile secondClickedTile)
        {
            int differenceX = Math.Abs(firstClickedTile.Position.X - secondClickedTile.Position.X);
            int differenceY = Math.Abs(firstClickedTile.Position.Y - secondClickedTile.Position.Y);

            // Checking if tiles are next to each other either by X or by Y // ATanev
            if (differenceX + differenceY == 1)
            {
                this.SwapTiles(firstClickedTile, secondClickedTile);
            }

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
            List<ITile[]> allTileMatches = GetAllTileMatches();

            // todo: add logic for adding points for each match, depending on the count

            // todo: add logic for removing the found matches
            this.RemoveMatchedTiles();

            throw new NotImplementedException();
        }

        // Getting all horizontal and vertical matches on the GameBoard // ATanev
        private List<ITile[]> GetAllTileMatches()
        {
            List<ITile[]> allTileMatches = new List<ITile[]>();

            for (int row = 0; row < gameBoard.GetLength(0); row++)
            {
                var tempStackOfTiles = new Stack<ITile>();
                tempStackOfTiles.Push(gameBoard[row, 0]);

                for (int col = 1; col < gameBoard.GetLength(1); col++)
                {
                    if (gameBoard[row, col].Equals(tempStackOfTiles.Peek()))
                    {
                        tempStackOfTiles.Push(gameBoard[row, col]);
                    }
                    else
                    {
                        if (tempStackOfTiles.Count >= 3)
                        {
                            allTileMatches.Add(tempStackOfTiles.ToArray());
                        }
                        tempStackOfTiles.Clear();
                        tempStackOfTiles.Push(gameBoard[row, col]);
                    }

                    if (tempStackOfTiles.Count >= 3)
                    {
                        allTileMatches.Add(tempStackOfTiles.ToArray());
                    }
                }
            }

            for (int col = 0; col < gameBoard.GetLength(1); col++)
            {
                var tempStackOfTiles = new Stack<ITile>();
                tempStackOfTiles.Push(gameBoard[0, col]);

                for (int row = 1; row < gameBoard.GetLength(0); row++)
                {
                    if (gameBoard[row, col].Equals(tempStackOfTiles.Peek()))
                    {
                        tempStackOfTiles.Push(gameBoard[row, col]);
                    }
                    else
                    {
                        if (tempStackOfTiles.Count >= 3)
                        {
                            allTileMatches.Add(tempStackOfTiles.ToArray());
                        }
                        tempStackOfTiles.Clear();
                        tempStackOfTiles.Push(gameBoard[row, col]);
                    }

                    if (tempStackOfTiles.Count >= 3)
                    {
                        allTileMatches.Add(tempStackOfTiles.ToArray());
                    }
                }
            }

            return allTileMatches;
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

        // Logic for swaping tiles // ATanev
        private void SwapTiles(ITile firstClickedTile, ITile secondClickedTile)
        {
            var tempPositionHolder = firstClickedTile.Position;
            firstClickedTile.Position = secondClickedTile.Position;
            secondClickedTile.Position = tempPositionHolder;

            this.CheckForMatch();
        }

        public IEnumerable<ITile> GetHint(GameBoard gameBoard)
        {
            // List that stores all the matches we find 
            List<ITile> matches = new List<ITile>();


            for (int row = 0; row < this.gameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < this.gameBoard.GetLength(1); col++)
                {
                    ITile currentTile = this.gameBoard[row, col];
                    ITile rightNeighbourTile = this.gameBoard[row, col + 1];
                    ITile downNeighbourTile = this.gameBoard[row + 1, col];

                    bool twoInRowMatch = currentTile.TileType.Equals(rightNeighbourTile.TileType);
                    bool twoInColumnMatch = currentTile.TileType.Equals(downNeighbourTile.TileType);

                    // Six horizontal cases for potential match
                    // 1. Check for: * & * & & * * * case
                    if (col > 1 && twoInRowMatch &&
                        currentTile.TileType.Equals(this.gameBoard[row, col - 2].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row, col - 2]);
                    }

                    // 2. Check for: * & & * & * * * case
                    if (col < NumberOfColumn - 4 && twoInRowMatch &&
                        currentTile.TileType.Equals(this.gameBoard[row, col + 3].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row, col + 3]);
                    }

                    // 3. Check for case:
                    // * & * * * *  
                    // * * & & * * 
                    if (col > 0 && row > 0 && twoInRowMatch &&
                        currentTile.TileType.Equals(this.gameBoard[row - 1, col - 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row - 1, col - 1]);
                    }

                    // 4. Check for case:
                    // * * * * & *  
                    // * * & & * * 
                    if (col < NumberOfColumn - 2 && row > 0 && twoInRowMatch
                        && currentTile.TileType.Equals(this.gameBoard[row - 1, col + 2].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row - 1, col + 2]);
                    }

                    // 5. Check for case:
                    // * * & & * *  
                    // * & * * * *
                    if (row < NumberOfRows - 2 && col > 0 && twoInRowMatch
                        && currentTile.TileType.Equals(this.gameBoard[row + 1, col - 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row + 1, col - 1]);
                    }

                    // 6. Check for case:
                    // * * & & * *  
                    // * * * * & * 
                    if (row < NumberOfRows - 2 && col < NumberOfColumn - 2 && twoInRowMatch
                        && currentTile.TileType.Equals(this.gameBoard[row + 1, col + 2].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row + 1, col + 2]);
                    }

                    // Six vertical cases for potential match

                    // * * & * *  
                    // * * & * * 
                    // * * * * *  --- vertical case 1 
                    // * * & * *
                    if (row < NumberOfRows - 3 && twoInColumnMatch &&
                        currentTile.TileType.Equals(this.gameBoard[row + 3, col].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row + 3, col]);
                    }

                    // * * & * * 
                    // * * * * *
                    // * * & * *  --- vertical case 2
                    // * * & * * 
                    if (row > 1 && twoInColumnMatch &&
                        currentTile.TileType.Equals(this.gameBoard[row - 2, col].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row - 2, col]);
                    }

                    // * & * *
                    // * * & *   --- vertical case 3
                    // * * & *
                    if (row > 0 && col > 0 && twoInColumnMatch &&
                        currentTile.TileType.Equals(this.gameBoard[row - 1, col - 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row - 1, col - 1]);
                    }

                    // * * & *
                    // * & * *   --- vertical case 4
                    // * & * * 
                    if (row > 0 && col < NumberOfColumn - 2 && twoInColumnMatch &&
                        currentTile.TileType.Equals(this.gameBoard[row - 1, col + 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row - 1, col + 1]);
                    }

                    // * * & *
                    // * * & *   --- vertical case 5
                    // * & * *
                    if (col > 0 && row < NumberOfRows - 2 && twoInColumnMatch &&
                        currentTile.TileType.Equals(this.gameBoard[row + 2, col - 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row + 2, col - 1]);
                    }

                    // * & * *
                    // * & * *   --- vertical case 6
                    // * * & *
                    if (row < NumberOfRows - 2 && col < NumberOfColumn - 1 && twoInColumnMatch &&
                        currentTile.TileType.Equals(this.gameBoard[row + 2, col + 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row + 2, col + 1]);
                    }
                }
            }

            if (matches.Count >= 3)
            {
                Random rand = new Random();
                yield return matches[rand.Next(matches.Count - 1)];
            }
            else
            {
                yield return null;
            }
        }
    }
}
using System.Threading;
using Bejewled.Model.EventArgs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bejewled.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bejewled.Model.Enums;
    using Bejewled.Model.Interfaces;

    public class GameBoard
    {
        private Rectangle clickableArea = new Rectangle(240, 40, 525, 525);
        private Point fistClickedTileCoordinates;
        private bool isFirstClick;
        private MouseState mouseState;
        private MouseState prevMouseState = Mouse.GetState();

        private readonly int NumberOfColumn;

        private readonly int NumberOfRows;

        private int[,] tiles;

        private Texture2D[] textureTiles;

        private readonly List<Tile> firstTileList;

        private readonly Tile[,] gameBoard;

        private readonly List<Tile> secondTileList;

        private readonly TileGenerator tileGenerator;


        public GameBoard(int numberOfRows, int numberOfColumn, Texture2D[] textureTiles)
        {
            this.NumberOfRows = numberOfRows;
            this.NumberOfColumn = numberOfColumn;

            this.tiles = new int[numberOfRows, numberOfColumn];
            this.gameBoard = new Tile[numberOfRows, numberOfColumn];
            this.textureTiles = textureTiles;

            this.firstTileList = new List<Tile>();
            this.secondTileList = new List<Tile>();
            this.tileGenerator = new TileGenerator();

            this.fistClickedTileCoordinates = new Point(0, 0);
            this.isFirstClick = true;
        }

        public void Update(GameTime gameTime)
        {
            this.mouseState = Mouse.GetState();
            this.DetectGameBoardClick();
        }

        public void DetectGameBoardClick()
        {
            if (this.mouseState.LeftButton == ButtonState.Pressed
                && this.prevMouseState.LeftButton == ButtonState.Released)
            {
                // We now know the left mouse button is down and it wasn't down last frame
                // so we've detected a click
                // Now find the position 
                var mousePos = new Point(this.mouseState.X, this.mouseState.Y);
                if (this.clickableArea.Contains(mousePos))
                {
                    var indexY = (int)Math.Floor((double)(this.mouseState.X - 240) / 65);
                    var indexX = (int)Math.Floor((double)(this.mouseState.Y - 40) / 65);
                    if (this.isFirstClick)
                    {
                        this.fistClickedTileCoordinates = new Point(indexX, indexY);
                        this.isFirstClick = false;
                    }
                    else
                    {
                        var firstTilePosition = new TilePosition();
                        firstTilePosition.X = fistClickedTileCoordinates.X;
                        firstTilePosition.Y = fistClickedTileCoordinates.Y;
                        Tile firstTile = new Tile(this.gameBoard[fistClickedTileCoordinates.X, fistClickedTileCoordinates.Y].TileType, firstTilePosition);

                        var secondTilePosition = new TilePosition();
                        secondTilePosition.X = indexX;
                        secondTilePosition.Y = indexY;
                        Tile secondTile = new Tile(this.gameBoard[indexX, indexY].TileType, secondTilePosition);

                        this.CheckForValidMove(firstTile, secondTile);

                        this.tiles = this.GenerateNumericGameBoard();
                        //this.view.DrawGameBoard();

                        this.isFirstClick = true;
                    }
                }
            }

            // Store the mouse state so that we can compare it next frame
            // with the then current mouse state
            this.prevMouseState = this.mouseState;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
           // spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            float x = 50;
            for (var i = 0; i < this.tiles.GetLength(0); i++)
            {
                float y = 250;
                for (var j = 0; j < this.tiles.GetLength(1); j++)
                {
                    spriteBatch.Draw(
                        textureTiles[this.tiles[i, j]],
                        new Vector2(y, x),
                        null,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        0.5f,
                        SpriteEffects.None,
                        0);
                    y += 65;
                }

                x += 65;
            }
            spriteBatch.End();
        }

        public void InitializeGameBoard()
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

           // this.AvaliableMoves();
           this.tiles = this.GenerateNumericGameBoard();
        }

        public void AvaliableMoves()
        {
            this.HorizontalCheck();
            this.CheckVertical();
        }

        private void CheckForMatch()
        {
            var allTileMatches = this.GetAllTileMatches();

            this.RemoveMatchedTiles(allTileMatches);

            this.MoveDownTiles();

            this.GenerateTilesOnEmptySpots();
        }

        // Checks if move is valid
        public void CheckForValidMove(Tile firstClickedTile, Tile secondClickedTile)
        {
            var differenceX = Math.Abs(firstClickedTile.Position.X - secondClickedTile.Position.X);
            var differenceY = Math.Abs(firstClickedTile.Position.Y - secondClickedTile.Position.Y);

            if (differenceX + differenceY == 1)
            {
                this.SwapTiles(firstClickedTile, secondClickedTile);

                var allTileMatches = this.GetAllTileMatches();
                if (allTileMatches.Count == 0)
                {
                    this.SwapTiles(firstClickedTile, secondClickedTile);
                }
                else
                {
                    this.RemoveMatchedTiles(allTileMatches);

                    this.MoveDownTiles();

                    this.GenerateTilesOnEmptySpots();
                }
            }
        }

        public IEnumerable<ITile> GetHint()
        {
            // List that stores all the matches we find 
            var matches = new List<ITile>();

            for (var row = 0; row < this.gameBoard.GetLength(0); row++)
            {
                for (var col = 0; col < this.gameBoard.GetLength(1); col++)
                {
                    var currentTile = this.gameBoard[row, col];
                    var rightNeighbourTile = this.gameBoard[row, col + 1];
                    var downNeighbourTile = this.gameBoard[row + 1, col];

                    var twoInRowMatch = currentTile.TileType.Equals(rightNeighbourTile.TileType);
                    var twoInColumnMatch = currentTile.TileType.Equals(downNeighbourTile.TileType);

                    // Six horizontal cases for potential match
                    // 1. Check for: * & * & & * * * case
                    if (col > 1 && twoInRowMatch && currentTile.TileType.Equals(this.gameBoard[row, col - 2].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row, col - 2]);
                    }

                    // 2. Check for: * & & * & * * * case
                    if (col < NumberOfColumn - 4 && twoInRowMatch
                        && currentTile.TileType.Equals(this.gameBoard[row, col + 3].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(rightNeighbourTile);
                        matches.Add(this.gameBoard[row, col + 3]);
                    }

                    // 3. Check for case:
                    // * & * * * *  
                    // * * & & * * 
                    if (col > 0 && row > 0 && twoInRowMatch
                        && currentTile.TileType.Equals(this.gameBoard[row - 1, col - 1].TileType))
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
                    if (row < NumberOfRows - 3 && twoInColumnMatch
                        && currentTile.TileType.Equals(this.gameBoard[row + 3, col].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row + 3, col]);
                    }

                    // * * & * * 
                    // * * * * *
                    // * * & * *  --- vertical case 2
                    // * * & * * 
                    if (row > 1 && twoInColumnMatch
                        && currentTile.TileType.Equals(this.gameBoard[row - 2, col].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row - 2, col]);
                    }

                    // * & * *
                    // * * & *   --- vertical case 3
                    // * * & *
                    if (row > 0 && col > 0 && twoInColumnMatch
                        && currentTile.TileType.Equals(this.gameBoard[row - 1, col - 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row - 1, col - 1]);
                    }

                    // * * & *
                    // * & * *   --- vertical case 4
                    // * & * * 
                    if (row > 0 && col < NumberOfColumn - 2 && twoInColumnMatch
                        && currentTile.TileType.Equals(this.gameBoard[row - 1, col + 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row - 1, col + 1]);
                    }

                    // * * & *
                    // * * & *   --- vertical case 5
                    // * & * *
                    if (col > 0 && row < NumberOfRows - 2 && twoInColumnMatch
                        && currentTile.TileType.Equals(this.gameBoard[row + 2, col - 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row + 2, col - 1]);
                    }

                    // * & * *
                    // * & * *   --- vertical case 6
                    // * * & *
                    if (row < NumberOfRows - 2 && col < NumberOfColumn - 1 && twoInColumnMatch
                        && currentTile.TileType.Equals(this.gameBoard[row + 2, col + 1].TileType))
                    {
                        matches.Add(currentTile);
                        matches.Add(downNeighbourTile);
                        matches.Add(this.gameBoard[row + 2, col + 1]);
                    }
                }
            }

            if (matches.Count >= 3)
            {
                var rand = new Random();
                yield return matches[rand.Next(matches.Count - 1)];
            }
            else
            {
                yield return null;
            }
        }

        private void CheckVertical()
        {
            for (var row = 0; row < this.gameBoard.GetLength(0) - 2; row++)
            {
                for (var column = 0; column < this.gameBoard.GetLength(1) - 1; column++)
                {
                    if (this.gameBoard[row, column].TileType == this.gameBoard[row + 1, column].TileType)
                    {
                        if (column - 1 >= 0)
                        {
                            if (this.gameBoard[row + 1, column].TileType == this.gameBoard[row + 2, column - 1].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row + 2, column]);
                                this.secondTileList.Add(this.gameBoard[row + 2, column - 1]);
                            }
                        }

                        if (column + 1 < this.gameBoard.GetLength(0))
                        {
                            if (this.gameBoard[row + 1, column].TileType == this.gameBoard[row + 2, column + 1].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row + 2, column]);
                                this.secondTileList.Add(this.gameBoard[row + 2, column + 1]);
                            }
                        }

                        if (row + 3 < this.gameBoard.GetLength(1))
                        {
                            if (this.gameBoard[row + 1, column].TileType == this.gameBoard[row + 3, column].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row + 2, column]);
                                this.secondTileList.Add(this.gameBoard[row + 3, column]);
                            }
                        }
                    }
                    else if (this.gameBoard[row, column].TileType == this.gameBoard[row + 2, column].TileType)
                    {
                        if (column + 1 < this.gameBoard.GetLength(0))
                        {
                            if (this.gameBoard[row + 1, column + 1].TileType == this.gameBoard[row, column].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row + 1, column + 1]);
                                this.secondTileList.Add(this.gameBoard[row + 1, column]);
                            }
                        }

                        if (column - 1 >= 0)
                        {
                            if (this.gameBoard[row + 1, column - 1].TileType == this.gameBoard[row, column].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row + 1, column - 1]);
                                this.secondTileList.Add(this.gameBoard[row + 1, column]);
                            }
                        }
                    }
                }
            }
        }

        public int[,] GenerateNumericGameBoard()
        {
            var otherGameBoard = new int[NumberOfRows, NumberOfColumn];
            for (var i = 0; i < this.gameBoard.GetLength(0); i++)
            {
                for (var j = 0; j < this.gameBoard.GetLength(1); j++)
                {
                    otherGameBoard[i, j] = (int)this.gameBoard[i, j].TileType;
                }
            }

            return otherGameBoard;
        }

        // On all empty spots of the gameboard we generate new tiles // ATanev
        private void GenerateTilesOnEmptySpots()
        {
            var rand = new Random();

            for (var row = 0; row < this.gameBoard.GetLength(0); row++)
            {
                for (var col = 0; col < this.gameBoard.GetLength(1); col++)
                {
                    if (this.gameBoard[row, col].TileType == TileType.Empty)
                    {
                        //  var newTile = this.tileGenerator.CreateRandomTile(row, col);
                        // this.gameBoard[row, col] = newTile;
                        var tileNum = rand.Next(7);

                        switch (tileNum)
                        {
                            case 0:
                                this.gameBoard[row, col] = new Tile(TileType.Red, new TilePosition { X = row, Y = col });
                                break;
                            case 1:
                                this.gameBoard[row, col] = new Tile(TileType.Green, new TilePosition { X = row, Y = col });
                                break;
                            case 2:
                                this.gameBoard[row, col] = new Tile(TileType.Blue, new TilePosition { X = row, Y = col });
                                break;
                            case 3:
                                this.gameBoard[row, col] = new Tile(TileType.RainBow, new TilePosition { X = row, Y = col });
                                break;
                            case 4:
                                this.gameBoard[row, col] = new Tile(TileType.Purple, new TilePosition { X = row, Y = col });
                                break;
                            case 5:
                                this.gameBoard[row, col] = new Tile(TileType.White, new TilePosition { X = row, Y = col });
                                break;
                            case 6:
                                this.gameBoard[row, col] = new Tile(TileType.Yellow, new TilePosition { X = row, Y = col });
                                break;
                        }
                    }
                }
            }

            var allTileMatches = this.GetAllTileMatches();
            
            if (allTileMatches.Count != 0)
            {
                this.RemoveMatchedTiles(allTileMatches);

                this.MoveDownTiles();

                this.GenerateTilesOnEmptySpots();
            }
        }

        // Getting all horizontal and vertical matches on the GameBoard // ATanev
        private List<ITile[]> GetAllTileMatches()
        {
            var allTileMatches = new List<ITile[]>();

            for (var row = 0; row < this.gameBoard.GetLength(0); row++)
            {
                var tempStackOfTiles = new Stack<ITile>();
                // tempStackOfTiles.Push(this.gameBoard[row, 0]);
                tempStackOfTiles.Push(new Tile(gameBoard[row, 0].TileType, gameBoard[row, 0].Position));

                for (var col = 1; col < this.gameBoard.GetLength(1); col++)
                {
                    if (this.gameBoard[row, col].TileType.Equals(tempStackOfTiles.Peek().TileType))
                    {
                        tempStackOfTiles.Push(this.gameBoard[row, col]);
                        // tempStackOfTiles.Push(new Tile(gameBoard[row, col].TileType, new TilePosition(row, col)));
                    }
                    else
                    {
                        if (tempStackOfTiles.Count >= 3)
                        {
                            allTileMatches.Add(tempStackOfTiles.ToArray());
                        }

                        tempStackOfTiles.Clear();
                        tempStackOfTiles.Push(this.gameBoard[row, col]);
                        //tempStackOfTiles.Push(new Tile(gameBoard[row, col].TileType, gameBoard[row, col].Position));
                    }
                }

                if (tempStackOfTiles.Count >= 3)
                {
                    allTileMatches.Add(tempStackOfTiles.ToArray());
                }
            }

            for (var col = 0; col < this.gameBoard.GetLength(1); col++)
            {
                var tempStackOfTiles = new Stack<ITile>();
                tempStackOfTiles.Push(this.gameBoard[0, col]);

                for (var row = 1; row < this.gameBoard.GetLength(0); row++)
                {
                    if (this.gameBoard[row, col].TileType.Equals(tempStackOfTiles.Peek().TileType))
                    {
                        tempStackOfTiles.Push(this.gameBoard[row, col]);
                    }
                    else
                    {
                        if (tempStackOfTiles.Count >= 3)
                        {
                            allTileMatches.Add(tempStackOfTiles.ToArray());
                        }

                        tempStackOfTiles.Clear();
                        tempStackOfTiles.Push(this.gameBoard[row, col]);
                    }
                }

                if (tempStackOfTiles.Count >= 3)
                {
                    allTileMatches.Add(tempStackOfTiles.ToArray());
                }
            }

            return allTileMatches.Distinct().ToList();
        }

        private void HorizontalCheck()
        {
            for (var row = 0; row < this.gameBoard.GetLength(0) - 1; row++)
            {
                for (var column = 0; column < this.gameBoard.GetLength(1) - 2; column++)
                {
                    if (this.gameBoard[row, column].TileType == this.gameBoard[row, column + 1].TileType)
                    {
                        if (row - 1 >= 0)
                        {
                            if (this.gameBoard[row, column + 1].TileType == this.gameBoard[row - 1, column + 2].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row, column + 2]);
                                this.secondTileList.Add(this.gameBoard[row - 1, column + 2]);
                            }
                        }

                        if (row + 1 < this.gameBoard.GetLength(0))
                        {
                            if (this.gameBoard[row, column + 1].TileType == this.gameBoard[row + 1, column + 2].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row, column + 2]);
                                this.secondTileList.Add(this.gameBoard[row + 1, column + 2]);
                            }
                        }

                        if (column + 3 < this.gameBoard.GetLength(1))
                        {
                            if (this.gameBoard[row, column + 1].TileType == this.gameBoard[row, column + 3].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row, column + 2]);
                                this.secondTileList.Add(this.gameBoard[row, column + 3]);
                            }
                        }
                    }
                    else if (this.gameBoard[row, column].TileType == this.gameBoard[row, column + 2].TileType)
                    {
                        if (row + 1 < this.gameBoard.GetLength(0))
                        {
                            if (this.gameBoard[row + 1, column + 1].TileType == this.gameBoard[row, column].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row + 1, column + 1]);
                                this.secondTileList.Add(this.gameBoard[row, column + 1]);
                            }
                        }

                        if (row - 1 >= 0)
                        {
                            if (this.gameBoard[row - 1, column + 1].TileType == this.gameBoard[row, column].TileType)
                            {
                                this.firstTileList.Add(this.gameBoard[row - 1, column + 1]);
                                this.secondTileList.Add(this.gameBoard[row, column + 1]);
                            }
                        }
                    }
                }
            }
        }

        // Moving everything down, if possible // ATanev
        private void MoveDownTiles()
        {
            // Moving tiles down until there are no changes // ATanev
            var thereIsChange = true;
            while (thereIsChange)
            {
                thereIsChange = false;

                for (var row = 0; row < this.gameBoard.GetLength(0) - 1; row++)
                {
                    for (var col = 0; col < this.gameBoard.GetLength(1); col++)
                    {
                        if (this.gameBoard[row, col].TileType != TileType.Empty
                            && this.gameBoard[row + 1, col].TileType == TileType.Empty)
                        {
                            this.SwapTiles(this.gameBoard[row, col], this.gameBoard[row + 1, col]);
                            thereIsChange = true;
                        }
                    }
                }
            }
        }

        // Removing matched Tiles by replacing them with an Empty one // ATanev
        private void RemoveMatchedTiles(List<ITile[]> matchesToRemove)
        {
            GlobalScore.globalScore += matchesToRemove.Count;

            foreach (var match in matchesToRemove)
            {
                foreach (var tile in match)
                {
                    if (this.gameBoard[tile.Position.X, tile.Position.Y].TileType != TileType.Empty)
                    {
                        this.gameBoard[tile.Position.X, tile.Position.Y] = new Tile(
                            TileType.Empty,
                            this.gameBoard[tile.Position.X, tile.Position.Y].Position);
                    }
                }
            }
        }

        private void SwapTiles(Tile firstClickedTile, Tile secondClickedTile)
        {
            this.gameBoard[firstClickedTile.Position.X, firstClickedTile.Position.Y] = secondClickedTile;
            this.gameBoard[secondClickedTile.Position.X, secondClickedTile.Position.Y] = firstClickedTile;

            int x = firstClickedTile.Position.X;
            int y = firstClickedTile.Position.Y;

            firstClickedTile.Position.X = secondClickedTile.Position.X;
            firstClickedTile.Position.Y = secondClickedTile.Position.Y;

            secondClickedTile.Position.X = x;
            secondClickedTile.Position.Y = y;

           // int temp = this.tiles[firstClickedTile.Position.X, firstClickedTile.Position.Y];
           // this.tiles[firstClickedTile.Position.X, firstClickedTile.Position.Y] = this.tiles[secondClickedTile.Position.X, secondClickedTile.Position.Y];
           // this.tiles[secondClickedTile.Position.X, secondClickedTile.Position.Y] = temp;

            this.CheckForMatch();
        }

        private void RemoveTiles(bool isVertical, ITile firstClickedTile)
        {
            if (isVertical)
            {

            }
        }

        private int CheckForVerticalMatch(ITile firstClickedTile)
        {
            int matchesLenght = 1;
            for (int i = firstClickedTile.Position.Y; i < this.gameBoard.GetLength(0); i++)
            {
                if (this.gameBoard[i, firstClickedTile.Position.Y].TileType == firstClickedTile.TileType)
                {
                    matchesLenght += 1;
                }
            }
            for (int i = firstClickedTile.Position.Y - 1; i >= 0; i--)
            {
                if (this.gameBoard[i, firstClickedTile.Position.Y].TileType == firstClickedTile.TileType)
                {
                    matchesLenght += 1;
                }
            }
            return matchesLenght;
        }

        private int CheckForHorizontalMatch(ITile firstClickedTile)
        {
            int matchesLenght = 1;
            for (int i = firstClickedTile.Position.X; i < this.gameBoard.GetLength(1); i++)
            {
                if (this.gameBoard[firstClickedTile.Position.X, i].TileType == firstClickedTile.TileType)
                {
                    matchesLenght += 1;
                }
            }
            for (int i = firstClickedTile.Position.X - 1; i >= 0; i--)
            {
                if (this.gameBoard[firstClickedTile.Position.X, i].TileType == firstClickedTile.TileType)
                {
                    matchesLenght += 1;
                }
            }
            return matchesLenght;
        }
    }
}
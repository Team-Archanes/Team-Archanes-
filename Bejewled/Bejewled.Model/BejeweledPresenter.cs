namespace Bejewled.Model
{
    using System;

    using Bejewled.Model.EventArgs;
    using Bejewled.Model.Interfaces;

    public class BejeweledPresenter
    {
        private readonly IGameBoard gameBoard;

        private IView view;

        public BejeweledPresenter(IView view, IGameBoard gameBoard)
        {
            this.gameBoard = gameBoard;
            this.view = view;
            this.view.OnLoad += this.GameLoaded;
            this.view.OnTileClicked += TileClicked;
        }

        private void TileClicked(object sender, TileEventArgs tileEventArgs)
        {
            this.gameBoard
        }

        private void GameLoaded(object sender, System.EventArgs eventArgs)
        {
            this.view.Tiles = this.gameBoard.InitializeGameBoard();
        }
    }
}
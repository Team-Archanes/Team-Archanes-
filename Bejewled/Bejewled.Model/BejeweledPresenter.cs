namespace Bejewled.Model
{
    using System;

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
        }

        private void GameLoaded(object sender, EventArgs eventArgs)
        {
            this.view.Tiles = this.gameBoard.InitializeGameBoard();
        }
    }
}
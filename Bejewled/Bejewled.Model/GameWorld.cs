using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bejewled.Model
{
    public class GameWorld
    {
        private GameBoard gameBoard;
        private Score score;

        private readonly Texture2D[] textureTiles = new Texture2D[8];
        private Texture2D grid;
        private Texture2D hintButton;

        private SpriteFont scoreFont;

        public GameWorld(ContentManager Content)
        {
            LoadContent(Content);

            this.gameBoard = new GameBoard(8, 8, textureTiles);
            this.gameBoard.InitializeGameBoard();

            this.score = new Score(Content);
        }

        private void LoadContent(ContentManager Content)
        {
            this.textureTiles[0] = Content.Load<Texture2D>(@"redgemTrans");
            this.textureTiles[1] = Content.Load<Texture2D>(@"greengemTrans");
            this.textureTiles[2] = Content.Load<Texture2D>(@"bluegemTrans");
            this.textureTiles[3] = Content.Load<Texture2D>(@"yellowgemTrans");
            this.textureTiles[4] = Content.Load<Texture2D>(@"purplegemTrans");
            this.textureTiles[5] = Content.Load<Texture2D>(@"whitegemTrans");
            this.textureTiles[6] = Content.Load<Texture2D>(@"rainbowTrans");
            this.textureTiles[7] = Content.Load<Texture2D>(@"emptyTrans");

            this.grid = Content.Load<Texture2D>(@"boardFinal");
        }

        public void Update(GameTime gameTime)
        {
            this.gameBoard.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(this.grid, Vector2.Zero, Color.White);
            spriteBatch.End();

            this.gameBoard.Draw(gameTime, spriteBatch);
            this.score.Draw(gameTime, spriteBatch);
        }
    }
}

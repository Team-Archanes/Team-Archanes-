using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bejewled.Model
{
    using Bejewled.Model.Interfaces;

    public class Score
    {
        private int playerScore;

        private Texture2D hintButton;
        private SpriteFont scoreFont;

        public Score(ContentManager Content)
        {
            this.scoreFont = Content.Load<SpriteFont>("scoreFont");
            this.hintButton = Content.Load<Texture2D>(@"hintButton");
        }

        public void Reset()
        {
            this.Reset();
            playerScore = 0;
        }

        public int PlayerScore
        {
            get { return playerScore; }
            set { playerScore = value; }
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //var scale = 0.5f;
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.DrawString(
                scoreFont,
                "Score: " + GlobalScore.globalScore,
                new Vector2(30, 120),
                Color.GreenYellow);
            spriteBatch.Draw(hintButton, new Vector2(60, 430), null, Color.White);
            spriteBatch.End();
        }
    }
}
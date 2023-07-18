using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LinkInBomberland
{
    /// <summary>
    /// this scene will show high score ranking
    /// </summary>
    public class HighScoreScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private NormalString msgString;
        private NormalString highScoreString;
        private ScoreManager sm;

        public HighScoreScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            Vector2 position1 = new Vector2(450, 100);

            tex = game.Content.Load<Texture2D>("Images/link1");
            sm = new ScoreManager();

            msgString = new NormalString(game, spriteBatch, game.Content.Load<SpriteFont>("Fonts/Regular"),
                position1, Color.Red, "This is your Top 10 score record!");
            this.Components.Add(msgString);

            UpdateTopTenString(game, spriteBatch);
        }

        public void UpdateTopTenString(Game game, SpriteBatch spriteBatch)
        {
            this.Components.Remove(highScoreString);
            Vector2 position2 = new Vector2(450, 150);
            String topTen = sm.showHighScoreScene();
            highScoreString = new NormalString(game, spriteBatch, game.Content.Load<SpriteFont>("Fonts/Regular"),
                position2, Color.Blue, topTen);
            this.Components.Add(highScoreString);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(tex, new Vector2(0, 0), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}


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
    /// this scene will show author name and created date
    /// </summary>
    public class CreditScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private NormalString helpString;

        public CreditScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            Vector2 position = new Vector2(250, 400);
            tex = game.Content.Load<Texture2D>("Images/helpImage");

            helpString = new NormalString(game, spriteBatch, game.Content.Load<SpriteFont>("Fonts/Regular"),
                position, Color.Black, "This is game is made by Taekmin Jeong \n2020-12-06");
            this.Components.Add(helpString);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(tex, new Vector2(150, 40), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}


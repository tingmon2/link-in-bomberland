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
    /// this class shows game rule, play image and control information
    /// </summary>
    public class HelpScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private NormalString helpString;

        public HelpScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            Vector2 position = new Vector2(250, 400);
            tex = game.Content.Load<Texture2D>("Images/helpImage");

            helpString = new NormalString(game, spriteBatch, game.Content.Load<SpriteFont>("Fonts/Regular"), 
                position, Color.Black, "Help link to hide from bombardment!!\nSpace key: mute/unmute background music\nUp arrow key: move up\nDown arrow key: move down\n" +
                "Left arrow key: move left\nRight arrow key: move right");
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

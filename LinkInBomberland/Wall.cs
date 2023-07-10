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
    /// Wall class is obstacle in the game screen
    /// </summary>
    public class Wall : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 position;
        private Rectangle destRect;

        public Texture2D Tex { get => tex; set => tex = value; }
        public Vector2 Position { get => position; set => position = value; }

        public Wall(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = position;
        }

        /// <summary>
        /// specify size of the wall rectangle and return
        /// </summary>
        /// <returns>Rectagle with given size</returns>
        public Rectangle getBound()
        {
            return new Rectangle((int)position.X, (int)position.Y,
                tex.Width - 8, tex.Height - 8);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            destRect = new Rectangle((int)position.X, (int)position.Y, tex.Width, tex.Height);
            spriteBatch.Draw(tex, destRect, Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

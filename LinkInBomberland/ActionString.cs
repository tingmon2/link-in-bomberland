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
    /// this class has strings only populate when action scene is visible
    /// </summary>
    public class ActionString : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private string message = "";
        private Vector2 position;
        private Color color;
        private Link link;
        private string score;

        public SpriteFont Font { get => font; set => font = value; }
        public string Message { get => message; set => message = value; }
        public Color Color { get => color; set => color = value; }

        public ActionString(Game game,
            SpriteBatch spriteBatch,
            SpriteFont font,
            Vector2 position,
            Color color,
            Link link) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.position = position;
            this.color = color;
            this.link = link;

            if(color == Color.Red)
            {
                hide();
            }
            else
            {
                show();
            }
        }

        public void hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }

        public void show()
        {
            this.Enabled = true;
            this.Visible = true;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, message, position, color);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if(position == Vector2.Zero)
            {
                // shows score on the top-left screen 
                message = ActionScene.GameTimer.ToString();
            }
            else
            {
                // game over string
                score = ActionScene.GameTimer.ToString();
                message = "Link has been attacked!\nGame Over!\nYour score is " + score + "\nPress ESC button";
            }
            base.Update(gameTime);
        }
    }
}

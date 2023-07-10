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
    /// this class will draw explosion animation
    /// </summary>
    public class Explosion : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 position;
        private Vector2 dimesion; // 스프라이트 시트에서 각 프레임당 조각낼 사각형 사이즈 
        private List<Rectangle> frames;
        private int frameIndex = -1;
        private int delay; // 얼마나 애니메이션이 빠르게 바뀔지를 결정
        private int delayCounter;
        private int direction = 0;

        public Vector2 Position { get => position; set => position = value; }
        public int Direction { get => direction; set => direction = value; }

        private const int ROW = 5;
        private const int COL = 5;

        public Explosion(Game game,
         SpriteBatch spriteBatch,
         Texture2D tex,
         Vector2 position,
         int delay) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = position;
            this.delay = delay;

            dimesion = new Vector2(tex.Width / COL, tex.Height / ROW); // 전체 스프라이트 시트에서 한 프레임 조각의 크기
            hide();
            createFrames();
        }

        public void hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }

        public void start()
        {
            this.Enabled = true;
            this.Visible = true;
            frameIndex = -1;
        }

        /// <summary>
        /// separate frames from explosion sprite sheet
        /// </summary>
        private void createFrames()
        {
            frames = new List<Rectangle>();
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    int x = j * (int)dimesion.X;
                    int y = i * (int)dimesion.Y;

                    Rectangle r = new Rectangle(x, y, (int)dimesion.X, (int)dimesion.Y);
                    frames.Add(r);
                }
            }
        }

        /// <summary>
        /// specify size of the explosion rectangle and return
        /// </summary>
        /// <returns>Rectagle with given size</returns>
        public Rectangle getBound()
        {
            return new Rectangle((int)position.X, (int)position.Y,
                (int)dimesion.X - 8, (int)dimesion.Y - 8);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (frameIndex >= 0)
            {
                spriteBatch.Draw(tex, Position, frames[frameIndex], Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            delayCounter++;
            if (delayCounter > delay)
            {
                frameIndex++;
                if (frameIndex > ROW * COL - 1)
                {
                    frameIndex = -1;
                    hide();
                }

                delayCounter = 0;
            }
            base.Update(gameTime);
        }
    }
}

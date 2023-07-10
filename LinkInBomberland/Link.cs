using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LinkInBomberland
{
    /// <summary>
    /// this class draws animation of link. also manages its movement and life
    /// </summary>
    public class Link : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 position;
        private Vector2 dimension;
        private Vector2 speedX;
        private Vector2 speedY;
        private List<Rectangle> frames;
        private Rectangle destRect;
        private int frameIndex = 0;
        private int frameCounter = 0;
        private int delay;
        private int delayCounter;
        private KeyboardState ks;
        private bool banLeft = false;
        private bool banRight = false;
        private bool banUp = false;
        private bool banDown = false;
        private bool linkAlive = true;
        private SoundEffect walkSound;
        private int walkCounter = 0;

        private const int ROW = 8;
        private const int COL = 10;
        private const int FRAMELENTH = 10;
        private const int WALL_WIDTH = 70;
        private const int WALK_INTERVAL = 15;


        public Vector2 Position { get => position; set => position = value; }
        public bool BanLeft { get => banLeft; set => banLeft = value; }
        public bool BanRight { get => banRight; set => banRight = value; }
        public bool BanUp { get => banUp; set => banUp = value; }
        public bool BanDown { get => banDown; set => banDown = value; }
        public bool LinkAlive { get => linkAlive; set => linkAlive = value; }

        public Link(Game game, 
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position,
            int delay,
            SoundEffect walkSound) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = position;
            this.delay = delay;
            this.speedX = new Vector2(3.5f, 0);
            this.speedY = new Vector2(0, 3.5f);
            this.walkSound = walkSound;

            dimension = new Vector2(tex.Width / COL, tex.Height / ROW);

            CreateFrames();
            Start();
        }

        /// <summary>
        /// execute this to show link
        /// </summary>
        public void Start()
        {
            this.Enabled = true;
            this.Visible = true;
        }

        /// <summary>
        /// separate image by size of dimension and add it on frame list
        /// to use it for animation effect
        /// </summary>
        private void CreateFrames()
        {
            frames = new List<Rectangle>();
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    int x = j * (int)dimension.X;
                    int y = i * (int)dimension.Y;

                    if (y > 400)
                    {
                        Rectangle r = new Rectangle(x, y, (int)dimension.X, (int)dimension.Y);
                        frames.Add(r);
                    }

                }
            }
        }

        /// <summary>
        /// specify size of the link rectangle and return
        /// </summary>
        /// <returns>Rectagle with given size</returns>
        public Rectangle getBound()
        {
            return new Rectangle((int)position.X, (int)position.Y, frames[frameIndex].Width / 2 - 4, frames[frameIndex].Height / 2 - 4);
        }

        private void IndexUpdate(int index)
        {
            // if frame reaches end, initialize to first frame
            if (frameIndex % FRAMELENTH == 0)
            {
                frameIndex = index;
            }
            frameIndex++;
            frameCounter++;
            // prevent animation shows any other direction image
            // if link is moving left, shows only frames for left moving
            if (frameCounter >= FRAMELENTH)
            {
                frameIndex = index;
                frameCounter = 0;
            }
            delayCounter = 0;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            destRect = new Rectangle((int)position.X, (int)position.Y, 
                frames[frameIndex].Width / 2, frames[frameIndex].Height / 2);

            spriteBatch.Draw(tex, destRect, frames[frameIndex], Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            ks = Keyboard.GetState();
            Vector2 lastPosition = position;

            if (ks.IsKeyDown(Keys.Up))
            {
                // moving up is banned. maintain position
                if(BanUp)
                {
                    position = lastPosition;
                    walkCounter++;
                }
                else
                {
                    position -= speedY;
                    walkCounter++;
                }

                // link is at the edge of top. maintain position
                if (position.Y < 0 + WALL_WIDTH)
                {
                    position.Y = WALL_WIDTH;
                    walkCounter++;
                }

                delayCounter++;
                // update frame index when delay counter is bigger than delay
                if (delayCounter > delay)
                {
                    IndexUpdate(20);
                }

            }
            else if (ks.IsKeyDown(Keys.Down))
            {
                if (BanDown)
                {
                    position = lastPosition;
                    walkCounter++;
                }
                else
                {
                    position += speedY;
                    walkCounter++;
                }

                if (position.Y > 560 - frames[frameIndex].Height / 2 - WALL_WIDTH)
                {
                    position.Y = 560 - frames[frameIndex].Height / 2 - WALL_WIDTH;
                    walkCounter++;
                }
                delayCounter++;
                if (delayCounter > delay)
                {
                    IndexUpdate(0);
                }

            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                if (BanLeft)
                {
                    position = lastPosition;
                    walkCounter++;
                }
                else
                {
                    position -= speedX;
                    walkCounter++;
                }

                if (position.X < 0 + WALL_WIDTH)
                {
                    position.X = WALL_WIDTH;
                    walkCounter++;
                }
                delayCounter++;
                if (delayCounter > delay)
                {
                    IndexUpdate(10);
                }
            }
            else if (ks.IsKeyDown(Keys.Right))
            {
                if (BanRight)
                {
                    position = lastPosition;
                    walkCounter++;
                }
                else
                {
                    position += speedX;
                    walkCounter++;
                }

                if (position.X > 840 - frames[frameIndex].Width / 2 - WALL_WIDTH)
                {
                    position.X = 840 - frames[frameIndex].Width / 2 - WALL_WIDTH;
                    walkCounter++;
                }

                delayCounter++;
                if (delayCounter > delay)
                {
                    IndexUpdate(30);
                }
            }
            // play walking sound effect
            if(walkCounter % WALK_INTERVAL == 0 && walkCounter > 0)
            {
                walkCounter = 0;
                walkSound.Play();
            }

            base.Update(gameTime);
        }
    }
}

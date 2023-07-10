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
    /// this class draws bomb. each bomb will have 17 explosions
    /// </summary>
    public class Bomb : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 position;
        private Rectangle destRect;
        private int delay;
        private int delayCounter;
        private Color bombColor = Color.White;
        private List<Vector2> positionList;
        private Explosion explosion;
        private List<Explosion> explosionList;
        private List<Vector2> wallPositionList;
        int counter = 1;

        private const int EXPLOSION_COUNT = 17;
        private const int LEFT = 1;
        private const int RIGHT = 2;
        private const int UP = 3;
        private const int DOWN = 4;
        private const int EDGE_LEFT = 70;
        private const int EDGE_RIGHT = 700;
        private const int EDGE_TOP = 70;
        private const int EDGE_DOWN = 440;

        public Vector2 Position { get => position; set => position = value; }
        public Texture2D Tex { get => tex; set => tex = value; }
        public List<Explosion> ExplosionList { get => explosionList; set => explosionList = value; }
        public Color BombColor { get => bombColor; set => bombColor = value; }

        public Bomb(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position,
            int delay,
            List<Vector2> wallPositionList) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = position;
            this.delay = delay;
            this.wallPositionList = wallPositionList;

            // store positions for future explosion
            positionList = new List<Vector2>();

            for (int i = 0; i < EXPLOSION_COUNT; i++)
            {
                Vector2 tempPosition = Vector2.Zero;
                //center
                if(i == 0)
                {
                    tempPosition = this.position;
                }
                //left
                if (i > 0 && i < 5)
                {
                    tempPosition = new Vector2(this.position.X - counter * tex.Width, this.position.Y);
                    counter++;
                }
                //right
                if (i >= 5 && i < 9)
                {
                    tempPosition = new Vector2(this.position.X + counter * tex.Width, this.position.Y);
                    counter++;
                }
                //up
                if (i >= 9 && i < 13)
                {
                    tempPosition = new Vector2(this.position.X, this.position.Y - counter*tex.Height);
                    counter++;
                }
                //down
                if (i >= 13 && i < 17)
                {
                    tempPosition = new Vector2(this.position.X, this.position.Y + counter * tex.Height);
                    counter++;
                }
                if(counter == 5)
                {
                    counter = 1;
                }
                positionList.Add(tempPosition);

            }

            explosionList = new List<Explosion>();
            // position list is used to make explosion list.
            // every bomb has its own explosion list
            for (int i = 0; i < positionList.Count; i++)
            {
                explosion = new Explosion(game, spriteBatch, game.Content.Load<Texture2D>("Images/explosion"), positionList[i], 2);
                if(i > 0 && i < 5)
                {
                    explosion.Direction = LEFT;
                }
                if (i >= 5 && i < 9)
                {
                    explosion.Direction = RIGHT;
                }
                if (i >= 9 && i < 13)
                {
                    explosion.Direction = UP;
                }
                if (i >= 13 && i < 17)
                {
                    explosion.Direction = DOWN;
                }
                explosionList.Add(explosion);
            }

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            destRect = new Rectangle((int)position.X, (int)position.Y, tex.Width, tex.Height);
            spriteBatch.Draw(tex, destRect, bombColor);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            delayCounter++;
            bool isBlocked = false;
            bool isIntersect = false;
            // warning sign
            if (delayCounter > delay * 5)
            {
                bombColor = Color.Red;
            }
            // exploded
            if (delayCounter > delay * 10)
            {
                bombColor = Color.Black;
            }
            // draw bomb's explosions
            if (bombColor == Color.Black)
            {
                // check every explosion in the particular bomb's explosion list
                for (int i = 0; i < explosionList.Count; i++)
                {
                    if (i == 0)
                    {
                        explosionList[i].start();
                        continue;
                    }
                    // explosion animation can't cross game screen border
                    if (explosionList[i].Position.X < EDGE_LEFT || explosionList[i].Position.X > EDGE_RIGHT)
                    {
                        isBlocked = true;
                    }
                    else if (explosionList[i].Position.Y < EDGE_TOP || explosionList[i].Position.Y > EDGE_DOWN)
                    {
                        isBlocked = true;
                    }

                    // explosion is within game screen and direction is left
                    if(isBlocked == false && explosionList[i].Direction == LEFT)
                    {
                        for (int j = 0; j < wallPositionList.Count; j++)
                        {
                            // explosion has same position with one of the wall in the wall position list
                            if(explosionList[i].Position == wallPositionList[j])
                            {
                                isIntersect = true;
                                // go to next direction and break out from the for loop
                                i = 4;
                                break;
                            }
                        }
                        if (isIntersect == false)
                        {
                            explosionList[i].start();
                        }
                    }
                    isIntersect = false;

                    if (isBlocked == false && explosionList[i].Direction == RIGHT)
                    {
                        for (int j = 0; j < wallPositionList.Count; j++)
                        {
                            if (explosionList[i].Position == wallPositionList[j])
                            {
                                isIntersect = true;
                                i = 8;
                                break;
                            }
                        }
                        if(isIntersect == false)
                        {
                            explosionList[i].start();
                        }
                    }
                    isIntersect = false;

                    if (isBlocked == false && explosionList[i].Direction == UP)
                    {
                        for (int j = 0; j < wallPositionList.Count; j++)
                        {
                            if (explosionList[i].Position == wallPositionList[j])
                            {
                                isIntersect = true;
                                i = 12;
                                break;
                            }
                        }
                        if (isIntersect == false)
                        {
                            explosionList[i].start();
                        }
                    }
                    isIntersect = false;

                    if (isBlocked == false && explosionList[i].Direction == DOWN)
                    {
                        for (int j = 0; j < wallPositionList.Count; j++)
                        {
                            if (explosionList[i].Position == wallPositionList[j])
                            {
                                isIntersect = true;
                                i = 16;
                                break;
                            }
                        }
                        if (isIntersect == false)
                        {
                            explosionList[i].start();
                        }
                    }
                    isIntersect = false;
                    isBlocked = false;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// specify size of the bomb rectangle and return
        /// </summary>
        /// <returns>Rectagle with given size</returns>
        public Rectangle getBound()
        {
            return new Rectangle((int)position.X, (int)position.Y,
                tex.Width - 8, tex.Height - 8);
        }
    }
}

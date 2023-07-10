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
    public class CollisionManager : GameComponent
    {
        private Link link;
        private Bomb bomb;
        private Wall wall;

        KeyboardState ks;

        public CollisionManager(Game game,
            Link link,
            Wall wall,
            Bomb bomb) : base(game)
        {
            this.link = link;
            this.wall = wall;
            this.bomb = bomb;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// when bomb is exploded, use its explosion list to 
        /// check if player has been collide with explosion
        /// </summary>
        /// <param name="link">player</param>
        /// <param name="explosionList">Each bomb has explosion list</param>
        public void ExplosionRadar(Link link, List<Explosion> explosionList)
        {
            Rectangle linkRect = link.getBound();
            foreach (var item in explosionList)
            {
                // item.Enabled means this explosion is drawable. no obstacles on the way
                if (item.Enabled)
                {
                    Rectangle explosionRect = item.getBound();
                    if (linkRect.Intersects(explosionRect))
                    {
                        link.LinkAlive = false;
                    }
                }
            }
        }

        /// <summary>
        /// get the center coordinate of player then 
        /// </summary>
        /// <param name="link">player position information</param>
        /// <param name="wallList">wall list has position information</param>
        /// <param name="bombList">bomb list has position information</param>
        public void CollisionRadar(Link link, List<Wall> wallList, List<Bomb> bombList)
        {
            ks = new KeyboardState();
            int centerX = ((int)link.Position.X + (int)link.Position.X + 46) / 2;
            int centerY = ((int)link.Position.Y + (int)link.Position.Y + 50) / 2;
            Vector2 center = new Vector2(centerX, centerY); // center position of link
            int radarRange = 10; // 레이더 범위 안의 사각형에 대해 충돌 계산
            int rangeX = 18; // manually typed to avoid bug
            int rangeY = 19;
            int collisionCounter = 0;
            int latestBan = 0;
            const int LEFT = 1;
            const int RIGHT = 2;
            const int UP = 3;
            const int DOWN = 4;

            foreach (var item in wallList)
            {
                // decision when player press left button
                if (!ks.IsKeyDown(Keys.Right))
                {
                    // only cares about walls within radar range 
                    // wall is on the left side of the player
                    // also, y-coordinate should not exceed radar range
                    if (center.X > item.Position.X + item.Tex.Width 
                        && center.Y + radarRange > item.Position.Y
                        && center.Y - radarRange < item.Position.Y + item.Tex.Height)
                    {
                        // player's x-coordinate is smaller means item(box) is close enough,
                        // so player cannnot move left
                        if (center.X - rangeX < item.Position.X + item.Tex.Width) //left
                        {
                            link.BanLeft = true;
                            latestBan = LEFT;
                            collisionCounter++;
                        }
                    }
                }
                if (!ks.IsKeyDown(Keys.Left))
                {
                    if (center.X < item.Position.X 
                        && center.Y + radarRange > item.Position.Y
                        && center.Y - radarRange < item.Position.Y + item.Tex.Height)
                    {
                        if (center.X + rangeX > item.Position.X) //right
                        {
                            link.BanRight = true;
                            latestBan = RIGHT;
                            collisionCounter++;
                        }
                    }
                }
                if (!ks.IsKeyDown(Keys.Down))
                {
                    if (center.Y > item.Position.Y + item.Tex.Height 
                        && center.X + radarRange > item.Position.X
                        && center.X - radarRange < item.Position.X + item.Tex.Width)
                    {
                        if (center.Y - rangeY < item.Position.Y + item.Tex.Height) //up
                        {
                            link.BanUp = true;
                            latestBan = UP;
                            collisionCounter++;
                        }
                    }
                }
                if (!ks.IsKeyDown(Keys.Up))
                {
                    if (center.Y < item.Position.Y 
                        && center.X + radarRange > item.Position.X
                        && center.X - radarRange < item.Position.X + item.Tex.Width)
                    {
                        if (center.Y + rangeY > item.Position.Y) //down
                        {
                            link.BanDown = true;
                            latestBan = DOWN;
                            collisionCounter++;
                        }
                    }
                }
            }

            // this iteration does same as above iteration code.
            // however it manages collide between player and bomb
            foreach (var item in bombList)
            {
                if (!ks.IsKeyDown(Keys.Right))
                {
                    if (centerX > item.Position.X + item.Tex.Width 
                        && centerY + radarRange > item.Position.Y
                        && centerY - radarRange < item.Position.Y + item.Tex.Height)
                    {
                        if (center.X - rangeX < item.Position.X + item.Tex.Width) //left
                        {
                            link.BanLeft = true;
                            latestBan = LEFT;
                            collisionCounter++;
                        }
                    }
                }
                if (!ks.IsKeyDown(Keys.Left))
                {
                    if (centerX < item.Position.X 
                        && centerY + radarRange > item.Position.Y
                        && centerY - radarRange < item.Position.Y + item.Tex.Height)
                    {
                        if (center.X + rangeX > item.Position.X) //right
                        {
                            link.BanRight = true;
                            latestBan = RIGHT;
                            collisionCounter++;
                        }
                    }
                }
                if (!ks.IsKeyDown(Keys.Down))
                {
                    if (centerY > item.Position.Y + item.Tex.Height 
                        && centerX + radarRange > item.Position.X
                        && centerX - radarRange < item.Position.X + item.Tex.Width)
                    {
                        if (center.Y - rangeY < item.Position.Y + item.Tex.Height) //up
                        {
                            link.BanUp = true;
                            latestBan = UP;
                            collisionCounter++;
                        }
                    }
                }
                if (!ks.IsKeyDown(Keys.Up))
                {
                    if (centerY < item.Position.Y 
                        && centerX + radarRange > item.Position.X
                        && centerX - radarRange < item.Position.X + item.Tex.Width)
                    {
                        if (center.Y + rangeY > item.Position.Y) //down
                        {
                            link.BanDown = true;
                            latestBan = DOWN;
                            collisionCounter++;
                        }
                    }
                }
                
            }
            if(collisionCounter == 0)
            {
                link.BanLeft = false;
                link.BanRight = false;
                link.BanUp = false;
                link.BanDown = false;
            }
            else if(collisionCounter < 2)
            {
                if(latestBan == LEFT) //left
                {
                    link.BanLeft = true;
                    link.BanRight = false;
                    link.BanUp = false;
                    link.BanDown = false;
                }
                if (latestBan == RIGHT) //right
                {
                    link.BanLeft = false;
                    link.BanRight = true;
                    link.BanUp = false;
                    link.BanDown = false;
                }
                if (latestBan == UP) //up
                {
                    link.BanLeft = false;
                    link.BanRight = false;
                    link.BanUp = true;
                    link.BanDown = false;
                }
                if (latestBan == DOWN) //down
                {
                    link.BanLeft = false;
                    link.BanRight = false;
                    link.BanUp = false;
                    link.BanDown = true;
                }
            }
        }
    }
}

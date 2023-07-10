using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace LinkInBomberland
{
    public class ActionScene : GameScene
    {
        private SpriteBatch spriteBatch;
        private ActionBackground gameBackground;
        private Link link;
        private Wall wall;
        private Box box;
        private ActionString score;
        private ActionString gameOver;
        private Bomb bomb;
        private Random r = new Random(); 
        private List<Vector2> wallPositionList = new List<Vector2>(); // 벽에 폭탄이 그려지는 것을 막기 위해
        private List<Bomb> bombList = new List<Bomb>(); // 리스트 내용물은 계속 업데이트 됨
        private List<Explosion> explosionList = new List<Explosion>(); // 리스트 내용물은 계속 업데이트 됨
        private List<Wall> wallList = new List<Wall>(); // 리스트 내용물은 초기화된 뒤 고정
        private CollisionManager cm;
        private bool isGameOver = false;
        private SoundEffect walkSound;
        private SoundEffect bombSound;
        private int level = 100;
        private int counter = 0;

        private static int gameTimer = 0;
        private static Game gameStatic;

        private const int WALL_WIDTH = 70;
        private const int COL = 12;
        private const int ROW = 8;
        private const int SIZE_X = 770;
        private const int SIZE_Y = 490;

        public bool IsGameOver { get => isGameOver; set => isGameOver = value; }
        public static int GameTimer { get => gameTimer; set => gameTimer = value; }

        public ActionScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            gameStatic = game;
            gameTimer = 0;

            walkSound = game.Content.Load<SoundEffect>("Sounds/step_cute");
            bombSound = game.Content.Load<SoundEffect>("Sounds/gun");

            //background
            gameBackground = new ActionBackground(game, spriteBatch,
                game.Content.Load<Texture2D>("Images/background"));
            this.Components.Add(gameBackground);

            //link
            link = new Link(game, spriteBatch, game.Content.Load<Texture2D>("Images/link_spritesheet"), 
                new Vector2(Shared.stage.X / 2, Shared.stage.Y / 2), 1, walkSound);
            this.Components.Add(link);
            
            //draw border line box
            DrawBorder();

            //draw walls in game ground randomly
            DrawWall();
            
            //bomb
            BombCreater();

            //collision manager
            cm = new CollisionManager(game, link, wall, bomb);

            //score string
            score = new ActionString(game, spriteBatch, game.Content.Load<SpriteFont>("Fonts/Regular"),
                Vector2.Zero, Color.Black, link);
            this.Components.Add(score);
            
            //game over string
            gameOver = new ActionString(game, spriteBatch, game.Content.Load<SpriteFont>("Fonts/Hilight"), 
                new Vector2(SIZE_X/2 - 120, SIZE_Y/2), Color.Transparent, link);
            this.Components.Add(gameOver);

            
        }

        public override void Update(GameTime gameTime)
        {
            if (link.LinkAlive)
            {
                gameTimer++;
                cm.CollisionRadar(link, wallList, bombList);
                int bombs = bombList.Count;
                if (bombs != 0)
                {
                    // if top of the bomb list is black, remove it from Components list
                    if (bombList[0].BombColor == Color.Black)
                    {
                        // before remove bomb, explosion radar will run
                        // to inspect whether link is alive or not
                        cm.ExplosionRadar(link, bombList[0].ExplosionList);
                        this.Components.Remove(bombList[0]);
                        bombList.RemoveAt(0);
                        bombSound.Play();
                    }
                }
                if (gameTimer % 100 == 0)
                {
                    // periodically remove used explosions from Components list 
                    if (explosionList.Count != 0 && explosionList[0].Enabled)
                    {
                        this.Components.Remove(explosionList[0]);
                        explosionList.RemoveAt(0);
                    }
                }
                // bomb making frequency will be shortened as timer gets bigger
                if (gameTimer % level == 0)
                {
                    BombCreater();
                }
                if (gameTimer > 500)
                {
                    level = 80;
                }
                if (gameTimer > 700)
                {
                    level = 60;
                }
                if (gameTimer > 1000)
                {
                    level = 40;
                }
                if (gameTimer > 1500)
                {
                    level = 20;
                }
                base.Update(gameTime);
            }
            if (link.LinkAlive)
            {
                isGameOver = false;
            }
            else
            {
                counter++;
                if(counter < 20)
                {
                    base.Update(gameTime);
                }
                isGameOver = true;
            }
            if (!link.LinkAlive)
            {
                gameOver.Color = Color.Red;
                this.Components.Add(gameOver);
            }


        }

        /// <summary>
        /// draw boxes to specify border
        /// </summary>
        private void DrawBorder()
        {
            for (int i = 0; i < COL; i++)
            {
                for (int j = 0; j < ROW; j++)
                {
                    int x = 0;
                    int y = 0;
                    // draw left-side vertical border
                    if (i == 0)
                    {
                        x = i * WALL_WIDTH;
                        y = j * WALL_WIDTH;
                    }
                    // draw right-side vertical border
                    else if (i == COL - 1)
                    {
                        x = i * WALL_WIDTH;
                        y = j * WALL_WIDTH;

                    }
                    // draw upper-side horizontal border
                    else if (j == 0)
                    {
                        x = i * WALL_WIDTH;
                        y = j * WALL_WIDTH;
                    }
                    // draw lower-side horizontal border
                    else if (j == ROW - 1)
                    {
                        x = i * WALL_WIDTH;
                        y = j * WALL_WIDTH;
                    }
                    Vector2 boxPosition = new Vector2(x, y);
                    box = new Box(gameStatic, spriteBatch, gameStatic.Content.Load<Texture2D>("Images/boxAlt"), boxPosition);
                    this.Components.Add(box);
                }
            }
        }

        /// <summary>
        /// wall will be ramdomly drawn on the game screen
        /// </summary>
        private void DrawWall()
        {
            for (int i = 0; i < 15; i++)
            {
                bool isOverlap = false;
                int randomX = r.Next(1, 11);
                int randomY = r.Next(1, 7);
                Vector2 wallPosition = new Vector2(randomX * WALL_WIDTH, randomY * WALL_WIDTH);
                if (link.Position == wallPosition)
                {
                    isOverlap = true;
                    i--;
                }
                if (!isOverlap)
                {
                    wall = new Wall(gameStatic, spriteBatch,
                        gameStatic.Content.Load<Texture2D>("Images/castle"), wallPosition);
                    wallPositionList.Add(wallPosition);
                    wallList.Add(wall);
                    this.Components.Add(wall);
                }
            }
        }

        /// <summary>
        /// create bomb but not on wall position
        /// </summary>
        private void BombCreater()
        {
            for (int i = 0; i < 1; i++)
            {
                bool isOverlap = false;
                int randomX = r.Next(1, 11);
                int randomY = r.Next(1, 7);
                Vector2 bombPosition = new Vector2(randomX * WALL_WIDTH, randomY * WALL_WIDTH);
                if (link.Position == bombPosition)
                {
                    isOverlap = true;
                    i--;
                }
                for (int j = 0; j < wallPositionList.Count; j++)
                {
                    if (bombPosition == wallPositionList[j])
                    {
                        // overlap is true means it have to get another random position
                        isOverlap = true;
                        i--;
                        break;
                    }
                }
                if (!isOverlap)
                {
                    bomb = new Bomb(gameStatic, spriteBatch,
                        gameStatic.Content.Load<Texture2D>("Images/bomb"), bombPosition, 20, 
                        wallPositionList);
                    this.Components.Add(bomb);
                    // for removing after explosion
                    bombList.Add(bomb);
                    // explosions in the created bomb will be also added in the Components list
                    for (int j = 0; j < bomb.ExplosionList.Count; j++)
                    {
                        this.Components.Add(bomb.ExplosionList[j]);
                        explosionList.Add(bomb.ExplosionList[j]);
                    }
                }
            }
        }
    }
}

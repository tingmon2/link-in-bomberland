using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LinkInBomberland
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //declatre all scene references
        private StartScene startScene;
        private ActionScene actionScene;
        private HelpScene helpScene;
        private CreditScene creditScene;


        private Song song;
        private KeyboardState oldState;

        private const int SIZE_X = 840;
        private const int SIZE_Y = 560;
        private const int START_GAME = 0;
        private const int HELP = 1;
        //private const int HIGH_SCORE = 2;
        private const int CREDIT = 3;
        private const int QUIT = 4;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // change to manual game screen size 
            graphics.PreferredBackBufferWidth = SIZE_X;
            graphics.PreferredBackBufferHeight = SIZE_Y;
            graphics.ApplyChanges();
            Shared.stage = new Vector2(graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);

            base.Initialize();
        }

        /// <summary>
        /// iterate component list to find and hide every game scenes
        /// </summary>
        private void hideAllScenes()
        {
            GameScene gs = null;
            foreach (GameComponent item in this.Components)
            {
                // if item is game scene, hide it
                if (item is GameScene)
                {
                    gs = (GameScene)item;
                    gs.hide();
                }
            }
        }

        /// <summary>
        /// load action scene only when user choose to play game
        /// </summary>
        private void LoadActionScene()
        {
            actionScene = new ActionScene(this, spriteBatch);
            this.Components.Add(actionScene);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            song = this.Content.Load<Song>("Sounds/13. Milkshake");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.IsMuted = true;
            MediaPlayer.Play(song);

            startScene = new StartScene(this, spriteBatch);
            this.Components.Add(startScene);

            //other scenes will be here

            helpScene = new HelpScene(this, spriteBatch);
            this.Components.Add(helpScene);

            creditScene = new CreditScene(this, spriteBatch);
            this.Components.Add(creditScene);

            //show startscene when game is loaded first time
            startScene.show();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here

            int selectedIndex = 0;
            KeyboardState ks = Keyboard.GetState();

            // start scene control
            if (startScene.Enabled)
            {
                selectedIndex = startScene.Menu.SelectedIndex;
                if (selectedIndex == START_GAME && ks.IsKeyDown(Keys.Enter))
                {
                    if (this.Components.Contains(actionScene))
                    {
                        hideAllScenes();
                        startScene.hide();
                        actionScene.show();
                    }
                    else
                    {
                        LoadActionScene();
                    }

                }
                if (selectedIndex == HELP && ks.IsKeyDown(Keys.Enter))
                {
                    hideAllScenes();
                    startScene.hide();
                    helpScene.show();
                }
                if (selectedIndex == CREDIT && ks.IsKeyDown(Keys.Enter))
                {
                    hideAllScenes();
                    startScene.hide();
                    creditScene.show();
                }
                if (selectedIndex == QUIT && ks.IsKeyDown(Keys.Enter))
                {
                    this.Exit();
                }
            }
            if (ks.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                if (MediaPlayer.IsMuted == true)
                {
                    MediaPlayer.IsMuted = false;
                }
                else
                {
                    MediaPlayer.IsMuted = true;
                }
            }
            oldState = ks;

            // other scenes control
            if (helpScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    hideAllScenes();
                    startScene.show();
                }
            }
            if (creditScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    hideAllScenes();
                    startScene.show();
                }
            }
            if (actionScene != null)
            {
                if (actionScene.Enabled && actionScene.IsGameOver)
                {
                    if (ks.IsKeyDown(Keys.Escape))
                    {
                        actionScene.hide();
                        startScene.show();
                        this.Components.Remove(actionScene);
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

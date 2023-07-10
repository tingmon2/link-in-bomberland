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
    /// start scene has background image and menu items
    /// </summary>
    public class StartScene : GameScene
    {
        private MenuComponent menu;
        private SpriteBatch spriteBatch;
        private ActionBackground startBackground;
        private string[] menuItems = {"Start game", "Help", "High Score", "Credit", "Quit" };

        public MenuComponent Menu { get => menu; set => menu = value; }

        public StartScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;

            startBackground = new ActionBackground(game, spriteBatch,
                game.Content.Load<Texture2D>("Images/mainBackground"));
            this.Components.Add(startBackground);

            Menu = new MenuComponent(game, spriteBatch,
                game.Content.Load<SpriteFont>("Fonts/Regular"),
                game.Content.Load<SpriteFont>("Fonts/Hilight"),
                menuItems);

            this.Components.Add(menu);

        }

       
    }
}

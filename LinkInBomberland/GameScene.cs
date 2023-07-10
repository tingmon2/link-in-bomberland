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
    /// this class is parent of every scenes in the game
    /// </summary>
    public abstract class GameScene : DrawableGameComponent
    {
        // list of menu component item
        private List<GameComponent> components;
        // each scene will have this GameComponent type list and it will contain each scene's components.
        public List<GameComponent> Components { get => components; set => components = value; }

        /// <summary>
        /// show scene
        /// </summary>
        public virtual void show()
        {
            this.Enabled = true;
            this.Visible = true;
        }

        /// <summary>
        /// hide scene
        /// </summary>
        public virtual void hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }

        public GameScene(Game game) : base(game)
        {
            components = new List<GameComponent>();
            //default. every scene is hidden 
            hide();
        }

       

        public override void Draw(GameTime gameTime)
        {
            DrawableGameComponent comp = null;
            foreach (GameComponent item in components)
            {
                // if child scene has drawable game component, draw it
                if (item is DrawableGameComponent)
                {

                    comp = (DrawableGameComponent)item;
                    // draw it only when it is visible
                    if (comp.Visible )
                    {
                        comp.Draw(gameTime);
                    }

                }
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent item in components)
            {
                // update component in child scene when it is enabled
                if (item.Enabled)
                {
                    item.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }



    }
}

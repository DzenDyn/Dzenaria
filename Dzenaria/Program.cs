using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace Dzenaria
{

    sealed class InjectedMain : Terraria.Main
    {
        private SpriteFont font;
        private SpriteBatch spriteBatch;
        private KeyboardState oldKeyboardState;

        internal InjectedMain() : base() { }

        protected override void LoadContent()
        {
            base.LoadContent();

            font = Terraria.Main.fontMouseText;
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            KeyboardState state = Keyboard.GetState();
            Player local = Main.player[Main.myPlayer]; // get our player

            bool instaHeal=false;
            bool manaInfusion=false;

            #region GhostMode
            if (local.ghost)
            {
                local.Ghost();
            }

            if (state.IsKeyDown(Keys.LeftShift) && oldKeyboardState.IsKeyUp(Keys.LeftShift))
            {
                if (local.ghost)
                {
                    local.ghost = false;
                    Terraria.Main.NewText("Ghost mode activated", 200, 200, 255);
                }
                else
                {
                    local.ghost = true;
                    Terraria.Main.NewText("Ghost mode deactivated", 200, 200, 255); 
                }
            }
            #endregion

            #region Heal function
            if (instaHeal)
            {
                local.statLife += 100;
            }

            if (state.IsKeyDown(Keys.NumPad1) && oldKeyboardState.IsKeyUp(Keys.NumPad1))
            {
                if (instaHeal)
                {
                    instaHeal = false;
                    Terraria.Main.NewText("Heal activated", 200, 200, 255);
                }
                else
                {
                    instaHeal = true;
                    Terraria.Main.NewText("Heal deactivated", 200, 200, 255);
                }
            }
            #endregion

            oldKeyboardState = state;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            InjectedMain game = new InjectedMain();
            game.Run();
            Console.ReadKey();
        }
    }
}

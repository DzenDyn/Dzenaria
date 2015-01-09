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
        

        private bool instaHeal = false;
        private bool manaInfusion = false;
        private bool aqualung = false;
        private Vector2 teleportTo;

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

            Player local = Main.player[Main.myPlayer]; // get our player
            KeyboardState state = Keyboard.GetState();
            

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
                    Terraria.Main.NewText("Ghost mode deactivated", 255, 100, 100);
                }
                else
                {
                    local.ghost = true;
                    Terraria.Main.NewText("Ghost mode activated", 100, 255, 100);
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
                    Terraria.Main.NewText("Heal deactivated", 255, 100, 100);
                }
                else
                {
                    instaHeal = true;
                    Terraria.Main.NewText("Heal activated", 100, 255, 100);
                }
            }
            #endregion

            #region Mana infusion function
            if (manaInfusion)
            {
                local.statMana += 100;
            }

            if (state.IsKeyDown(Keys.NumPad2) && oldKeyboardState.IsKeyUp(Keys.NumPad2))
            {
                if (manaInfusion)
                {
                    manaInfusion = false;
                    Terraria.Main.NewText("Mana infusion deactivated", 200, 200, 255);
                }
                else
                {
                    manaInfusion = true;
                    Terraria.Main.NewText("Mana infusion activated", 200, 200, 255);
                }
            }
            #endregion

            #region Aqualung function
            if (aqualung)
            {
                local.breath += 10;
            }

            if (state.IsKeyDown(Keys.NumPad3) && oldKeyboardState.IsKeyUp(Keys.NumPad3))
            {
                if (aqualung)
                {
                    aqualung = false;
                    Terraria.Main.NewText("Aqualung deactivated", 255, 100, 100);
                }
                else
                {
                    aqualung = true;
                    Terraria.Main.NewText("Aqualung activated", 100, 255, 100);
                }
            }
            #endregion

            #region Teleport function

            if (state.IsKeyDown(Keys.F1) && oldKeyboardState.IsKeyUp(Keys.F1))
            {
                teleportTo = local.position;
                Terraria.Main.NewText("Point for teleporting saved", 100, 255, 100);
            }
            if (state.IsKeyDown(Keys.F2) && oldKeyboardState.IsKeyUp(Keys.F2))
            {
                local.Teleport(teleportTo,3);
            }
            #endregion

            oldKeyboardState = state;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //Player local = Main.player[Main.myPlayer]; // get our player


            //spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            //spriteBatch.DrawString(font, "Dzenaria stats:", new Vector2(GraphicsDevice.Viewport.Width-250, GraphicsDevice.Viewport.Height-250), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            //spriteBatch.End();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            InjectedMain game = new InjectedMain();
            game.Run();
        }
    }
}

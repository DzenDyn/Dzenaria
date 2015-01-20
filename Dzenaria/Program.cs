using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
                    Terraria.Main.NewText("Mana infusion deactivated", 255, 100, 100);
                }
                else
                {
                    manaInfusion = true;
                    Terraria.Main.NewText("Mana infusion activated", 100, 255, 100);
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
                local.Teleport(teleportTo,2);
            }
            #endregion

            #region GiveGold function
            if (state.IsKeyDown(Keys.F3) && oldKeyboardState.IsKeyUp(Keys.F3))
            {
                for (var i = 0; i < 100; i++)
                {
                    local.PutItemInInventory(73, -1);
                }
                Terraria.Main.NewText("You get 100 gold coins", 255, 215, 0);                
            }
            #endregion

            #region ChangeName function
            if (state.IsKeyDown(Keys.F12) && oldKeyboardState.IsKeyUp(Keys.F12))
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Name:", "Enter new name", "Default", -1, -1);
                if (input != "")
                {
                    local.name = input;
                    Terraria.Main.NewText("Your name now \"" + input + "\"", 255, 0, 255);
                }
            }
            #endregion

            #region AddItem function
            if (state.IsKeyDown(Keys.F4) && oldKeyboardState.IsKeyUp(Keys.F4))
            {

                string input = Microsoft.VisualBasic.Interaction.InputBox("ID:", "Enter item id", "Default", -1, -1);
                if (input != "")
                {
                    try
                    {
                        local.PutItemInInventory(Convert.ToInt32(input),-1);
                        Terraria.Main.NewText("You get item with id "+input, 255, 215, 0);
                    }
                    catch (Exception)
                    {
                        Terraria.Main.NewText("Cannot add item with id" + input, 255, 100, 100);
                        return;
                    }
                }
            }
            #endregion

            #region ShowItem ID function
            if (state.IsKeyDown(Keys.F5) && oldKeyboardState.IsKeyUp(Keys.F5))
            {
                Terraria.Main.NewText(local.inventory[10].ToString(), 255, 100, 100); 
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
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        static void Main(string[] args)
        {
            IntPtr h = Process.GetCurrentProcess().MainWindowHandle;
            ShowWindow(h, 0);
            InjectedMain game = new InjectedMain();
            game.Run();
            
        }
    }
}

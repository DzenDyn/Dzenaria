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
            Player local = Main.player[Main.myPlayer]; // получаем нашего игрока

            local.ghost = state.IsKeyDown(Keys.LeftShift);
            if (local.ghost)
            {
                local.Ghost();
            }

            // пишем в чат
            if (state.IsKeyDown(Keys.LeftShift) && oldKeyboardState.IsKeyUp(Keys.LeftShift))
            {
                if (local.ghost)
                {
                    local.ghost = false;
                    Terraria.Main.NewText("Режим призрака деактивирован", 200, 200, 255);
                }
                else
                {
                    local.ghost = true;
                    Terraria.Main.NewText("Режим призрака активирован", 200, 200, 255); 
                }
               
                
            }

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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MGSimpelFysik
{
    public class Input
    {
        private KeyboardState previousKeyboardState;

        public Input() { }

        public void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardSate = Keyboard.GetState();


            previousKeyboardState = currentKeyboardSate;
        }

    }
}

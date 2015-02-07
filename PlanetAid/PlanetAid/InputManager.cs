using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace InputManager
{
    public class Input : GameComponent
    {
        private static KeyboardState currentKeyboardState;
        private static KeyboardState previousKeyboardState;
        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        public Input(Game game)
            : base(game) { }
        
        public override void Update(GameTime gameTime)
        {
            // Store the old input states
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;

            // Get the current input states
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        #region Keyboard
        /// <summary>
        /// Checks if a key is currently being held down.
        /// </summary>
        /// <param name="key">The keyboard key to check.</param>
        public static bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if a key was pressed.
        /// </summary>
        /// <param name="key">The keyboard key to check.</param>
        public static bool IsKeyPressed(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key)
                && previousKeyboardState.IsKeyUp(key));
        }

        /// <summary>
        /// Checks if a key was released.
        /// </summary>
        /// <param name="key">The keyboard key to check.</param>
        public static bool IsKeyReleased(Keys key)
        {
            return (currentKeyboardState.IsKeyUp(key)
                && previousKeyboardState.IsKeyDown(key));
        }
        #endregion

        #region Mouse
        /// <summary>
        /// Checks if the left mouse button is currently being held down.
        /// </summary>
        public static bool IsMouseDown()
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Checks if the left mouse button was pressed.
        /// </summary>
        public static bool IsMousePressed()
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed
                && previousMouseState.LeftButton == ButtonState.Released);
        }

        /// <summary>
        /// Checks if the left mouse button was released.
        /// </summary>
        public static bool IsMouseReleased()
        {
            return (currentMouseState.LeftButton == ButtonState.Released
                && previousMouseState.LeftButton == ButtonState.Pressed);
        }
        #endregion
    }
}

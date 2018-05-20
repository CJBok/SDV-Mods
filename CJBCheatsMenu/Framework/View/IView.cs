using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// A view that can be rendered to the screen.
    /// </summary>
    internal interface IView
    {
        /// <summary>
        /// The size and position of the view.
        /// </summary>
        Rectangle Bounds { get; set; }

        /// <summary>
        /// Renders the view to the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to render assets.</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Handle the left click event.
        /// </summary>
        /// <param name="x">x position of mouse when left click occured.</param>
        /// <param name="y">y position of mouse when left click occured.</param>
        void ReceiveLeftClick(int x, int y);

        /// <summary>
        /// Handle the left click release event.
        /// </summary>
        /// <param name="x">x position of mouse when left click was released.</param>
        /// <param name="y">y position of mouse when left click was released.</param>
        void LeftClickReleased(int x, int y);

        /// <summary>
        /// Handle the left click held event.
        /// </summary>
        /// <param name="x">x position of mouse during the left click hold.</param>
        /// <param name="y">y position of mouse during the left click hold.</param>
        void LeftClickHeld(int x, int y);

        /// <summary>
        /// Handles a key press event.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        void ReceiveKeyPress(Keys key);
    }
}

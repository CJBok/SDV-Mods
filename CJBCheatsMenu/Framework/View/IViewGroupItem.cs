using Microsoft.Xna.Framework.Graphics;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// A view that can be rendered in a view group.
    /// </summary>
    interface IViewGroupItem : IView
    {
        /// <summary>
        /// Draws the view at the given position rather than the position defined in the childs bounds.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to render assets.</param>
        /// <param name="x">The x position to draw the view.</param>
        /// <param name="y">The y positoin to draw the view.</param>
        void Draw(SpriteBatch spriteBatch, int x, int y);
    }
}

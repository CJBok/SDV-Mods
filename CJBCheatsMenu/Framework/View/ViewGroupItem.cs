using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// A view that can be rendered in a view group.
    /// </summary>
    internal class ViewGroupItem : StardewValley.Menus.OptionsElement, IViewGroupItem
    {
        /// <summary>
        /// The size and position of the view.
        /// </summary>
        public Rectangle Bounds
        {
            get => this.bounds;
            set => this.bounds = value;
        }

        /// <summary>
        /// Constructor for a ViewGroupItem.
        /// </summary>
        /// <param name="label">The label of the view group item.</param>
        /// <param name="width">The width of the view group item.</param>
        /// <param name="height">The height of the view group item.</param>
        /// <param name="whichOption">Unused for the most part but sometimes effects rendering.</param>
        public ViewGroupItem(string label, int width, int height, int whichOption = -1)
            : base(label, -1, -1, width, height, whichOption)
        {

        }

        /// <summary>
        /// The label that is drawn.
        /// </summary>
        /// <remarks>
        /// The default is the options Label, but can be overridden to customize what is being rendered.
        /// </remarks>
        public virtual string DrawnLabel => this.label;

        /// <summary>
        /// Draws the view group item.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to render assets.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            this.Draw(spriteBatch, this.Bounds.X, this.Bounds.Y);
        }

        /// <summary>
        /// Draws the view group item at the given position.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="x">The x coordinate where to draw the view item.</param>
        /// <param name="y">The y coordinate where to draw the view item.</param>
        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            this.label = this.DrawnLabel;
            this.draw(spriteBatch, x, y);
        }

        /// <summary>
        /// Handle the left click event.
        /// </summary>
        /// <param name="x">x position of mouse when left click occured.</param>
        /// <param name="y">y position of mouse when left click occured.</param>
        public void ReceiveLeftClick(int x, int y)
        {
            this.receiveLeftClick(x, y);
        }

        /// <summary>
        /// Handle the left click release event.
        /// </summary>
        /// <param name="x">x position of mouse when left click was released.</param>
        /// <param name="y">y position of mouse when left click was released.</param>
        public void LeftClickReleased(int x, int y)
        {
            this.leftClickReleased(x, y);
        }

        /// <summary>
        /// Handle the left click held event.
        /// </summary>
        /// <param name="x">x position of mouse during the left click hold.</param>
        /// <param name="y">y position of mouse during the left click hold.</param>
        public void LeftClickHeld(int x, int y)
        {
            this.leftClickHeld(x, y);
        }

        /// <summary>
        /// Handles a key press event.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        public void ReceiveKeyPress(Keys key)
        {
            this.receiveKeyPress(key);
        }
    }
}

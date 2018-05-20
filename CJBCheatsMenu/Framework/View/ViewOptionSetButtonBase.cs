using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// Base class that renders any option that uses a set button.
    /// </summary>
    /// <typeparam name="T">The type of option that this view renders.</typeparam>
    internal class ViewOptionSetButtonBase<T> : ViewOption<T> where T : Menu.IOption
    {
        /// <summary>
        /// The bounds of the set button that is rendered within the option.
        /// </summary>
        protected Rectangle SetButtonBounds { get; set; }

        /// <summary>
        /// The sprite of the set button within the sprite batch.
        /// </summary>
        protected readonly Rectangle SetButtonSprite = new Rectangle(294, 428, 21, 11);

        /// <summary>
        /// Constructor for a option with a set button.
        /// </summary>
        /// <param name="setButtonOption">The underlying option to be rendered.</param>
        /// <param name="containerWidth">The width of the row the option is being rendered in.</param>
        public ViewOptionSetButtonBase(T setButtonOption, int containerWidth) :
            base(setButtonOption, containerWidth + 1, 11 * StardewValley.Game1.pixelZoom)
        {
            SetButtonBounds = new Rectangle(containerWidth - 28 * StardewValley.Game1.pixelZoom, -1 + StardewValley.Game1.pixelZoom * 3, 21 * StardewValley.Game1.pixelZoom, 11 * StardewValley.Game1.pixelZoom);
        }

        /// <summary>
        /// Draws the option into the menu.
        /// </summary>
        /// <param name="spriteBatch">Passed to the base stardew valley renderer to perform rendering.</param>
        /// <param name="slotX">x position of the option to begin rendering.</param>
        /// <param name="slotY">y position of the option to begin rendering.</param>
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            // base draw isn't called, so have to set this here
            this.label = this.Option.Label;
            this.greyedOut = this.Option.Disabled;

            StardewValley.Utility.drawWithShadow(spriteBatch, StardewValley.Game1.mouseCursors, new Vector2(this.SetButtonBounds.X + slotX, this.SetButtonBounds.Y + slotY), this.SetButtonSprite, Color.White, 0.0f, Vector2.Zero, StardewValley.Game1.pixelZoom, false, 0.15f);
        }
    }
}

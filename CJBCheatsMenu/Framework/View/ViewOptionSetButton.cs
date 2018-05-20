using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// Renders an option that has a set button.
    /// </summary>
    internal class ViewOptionSetButton : ViewOptionSetButtonBase<Menu.IOptionSetButton>
    {
        /// <summary>
        /// Constructor for a view with a set button.
        /// </summary>
        /// <param name="setButtonOption">The underlying set button option to render.</param>
        /// <param name="containerWidth">The width of the row the option is being rendered in.</param>
        public ViewOptionSetButton(Menu.IOptionSetButton setButtonOption, int containerWidth) :
            base(setButtonOption, containerWidth)
        {
        }

        /// <summary>
        /// Called when a left click occurs, calls the OnPressed callback for the option.
        /// </summary>
        /// <param name="x">x position of the left click.</param>
        /// <param name="y">y position of the left click.</param>
        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut || !this.SetButtonBounds.Contains(x, y))
            {
                return;
            }
            this.Option.OnPressed();
        }

        /// <summary>
        /// Draws the option into the menu.
        /// </summary>
        /// <param name="spriteBatch">Passed to the base stardew valley renderer to perform rendering.</param>
        /// <param name="slotX">x position of the option to begin rendering.</param>
        /// <param name="slotY">y position of the option to begin rendering.</param>
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            base.draw(spriteBatch, slotX, slotY);
            StardewValley.Utility.drawTextWithShadow(spriteBatch, this.label, StardewValley.Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), this.greyedOut ? StardewValley.Game1.textColor * 0.33f : StardewValley.Game1.textColor, 1f, 0.15f);
        }
    }
}

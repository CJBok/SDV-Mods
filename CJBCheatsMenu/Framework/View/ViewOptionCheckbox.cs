using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// Renders a checkbox option.
    /// </summary>
    internal class ViewOptionCheckbox : ViewOption<Menu.IOptionCheckbox>
    {
        /// <summary>
        /// Constructor for a checkbox view.
        /// </summary>
        /// <param name="checkboxOption">The underlying checkbox option this view renders.</param>
        public ViewOptionCheckbox(Menu.IOptionCheckbox checkboxOption)
            : base(checkboxOption, 9 * StardewValley.Game1.pixelZoom, 9 * StardewValley.Game1.pixelZoom, 0)
        {
        }

        /// <summary>
        /// Whether or not this option is currently checked.
        /// </summary>
        public bool Checked
        {
            get
            {
                return this.Option.Value;
            }
            set
            {
                this.Option.Value = value;
            }
        }

        /// <summary>
        /// Called when a left click occurs, checks/unchecks the checkbox.
        /// </summary>
        /// <param name="x">x position of the left click.</param>
        /// <param name="y">y position of the left click.</param>
        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;

            StardewValley.Game1.soundBank.PlayCue("drumkit6");
            base.receiveLeftClick(x, y);
            this.Checked = !this.Checked;
        }

        /// <summary>
        /// Draws the option into the menu.
        /// </summary>
        /// <param name="spriteBatch">Passed to the base stardew valley renderer to perform rendering.</param>
        /// <param name="slotX">x position of the option to begin rendering.</param>
        /// <param name="slotY">y position of the option to begin rendering.</param>
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            Vector2 position = new Vector2(slotX + this.bounds.X, slotY + this.bounds.Y);
            Rectangle sourceRect = this.Checked ? StardewValley.Menus.OptionsCheckbox.sourceRectChecked : StardewValley.Menus.OptionsCheckbox.sourceRectUnchecked;
            Color color = Color.White * (this.greyedOut ? 0.33f : 1f);
            spriteBatch.Draw(StardewValley.Game1.mouseCursors, position,  sourceRect, color, 0.0f, Vector2.Zero, StardewValley.Game1.pixelZoom, SpriteEffects.None, 0.4f);
            base.draw(spriteBatch, slotX, slotY);
        }
    }
}

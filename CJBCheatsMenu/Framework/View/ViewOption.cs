using Microsoft.Xna.Framework.Graphics;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// Renders an option by displaying a label.
    /// </summary>
    /// <typeparam name="T">The type of option that this view renders.</typeparam>
    internal class ViewOption<T> : ViewGroupItem where T : Menu.IOption
    {
        /// <summary>
        /// The underlying option being rendered.
        /// </summary>
        public T Option { get; private set; }

        /// <summary>
        /// Constructor for a option view.
        /// </summary>
        /// <param name="option">The underlying option being rendered.</param>
        /// <param name="width">The width of this option.</param>
        /// <param name="height">The height of this option.</param>
        /// <param name="whichOption">Value that sometimes affects rendering but should otherwise be ignored.</param>
        public ViewOption(T option, int width, int height, int whichOption = -1)
            : base(option.Label, width, height, whichOption)
        {
            this.Option = option;
            this.greyedOut = this.Option.Disabled;
        }

        /// <summary>
        /// Draws the option into the menu.
        /// </summary>
        /// <param name="spriteBatch">Passed to the base stardew valley renderer to perform rendering.</param>
        /// <param name="slotX">x position of the option to begin rendering.</param>
        /// <param name="slotY">y position of the option to begin rendering.</param>
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            this.greyedOut = this.Option.Disabled;
            base.draw(spriteBatch, slotX, slotY);
        }
    }
}

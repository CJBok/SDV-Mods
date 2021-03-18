using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework.Components
{
    /// <summary>An options element which contains descriptive text.</summary>
    internal class DescriptionElement : BaseOptionsElement
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The checkbox label.</param>
        public DescriptionElement(string label)
          : base(label, -1, -1, 0, 0, 0) { }

        /// <summary>Draw the component to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        /// <param name="slotX">The X position at which to draw, relative to the bounds.</param>
        /// <param name="slotY">The Y position at which to draw, relative to the bounds.</param>
        /// <param name="context">The menu drawing the component.</param>
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY, IClickableMenu context = null)
        {
            int yOffset = 0;
            spriteBatch.DrawString(Game1.smallFont, this.label, new Vector2(slotX + this.bounds.X, slotY + this.bounds.Y + yOffset), Color.Black);
        }
    }
}

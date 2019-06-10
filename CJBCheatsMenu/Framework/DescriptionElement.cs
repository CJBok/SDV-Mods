using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework
{
    /// <summary>An options element which contains descriptive text.</summary>
    internal class DescriptionElement : OptionsElement
    {
        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The checkbox label.</param>
        public DescriptionElement(string label)
          : base(label, -1, -1, 0, 0, 0) { }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            int yOffset = 0;
            //Utility.drawBoldText(spriteBatch, line, Game1.smallFont, new Vector2(slotX + this.bounds.X, slotY + this.bounds.Y + yOffset), Color.Black);
            spriteBatch.DrawString(Game1.smallFont, this.label, new Vector2(slotX + this.bounds.X, slotY + this.bounds.Y + yOffset), Color.Black);
        }
    }
}

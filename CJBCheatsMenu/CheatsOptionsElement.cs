using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;

namespace CJBCheatsMenu
{
    internal class CheatsOptionsElement : OptionsElement
    {
        /*********
        ** Public methods
        *********/
        public CheatsOptionsElement(string label, int whichOption)
          : base(label)
        {
            this.whichOption = whichOption;
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            if (this.whichOption == -1)
            {
                SpriteText.drawString(spriteBatch, this.label, slotX + this.bounds.X, slotY + this.bounds.Y + Game1.pixelZoom * 3, 999, -1, 999, 1f, 0.1f);
                return;
            }

            string info = whichOption == 1
                ? CJB.GetWeatherNexDay()
                : "";
            Utility.drawTextWithShadow(spriteBatch, $"{this.label}: {info}", Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), Game1.textColor, 1f, 0.15f);
        }
    }
}

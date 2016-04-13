using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CJBCheatsMenu {
    class CheatsOptionsElement : OptionsElement {

        public string activeLabel = "";

        public CheatsOptionsElement(string label, int whichOption)
          : base(label) {
            base.whichOption = whichOption;
            activeLabel = label;
        }

        public override void draw(SpriteBatch b, int slotX, int slotY) {
            if (this.whichOption == -1) {
                SpriteText.drawString(b, this.label, slotX + this.bounds.X, slotY + this.bounds.Y + Game1.pixelZoom * 3, 999, -1, 999, 1f, 0.1f, false, -1, "", -1);
                return;
            }

            string info = "";

            switch (whichOption) {
                case 1:
                    info = CJB.getWeatherNexDay();
                    break;
            }

            Utility.drawTextWithShadow(b, this.label + ": " + info, Game1.dialogueFont, new Vector2((float)(this.bounds.X + slotX), (float)(this.bounds.Y + slotY)), Game1.textColor, 1f, 0.15f, -1, -1, 1f, 3);
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework
{
    /// <summary>A button which invokes a callback when clicked.</summary>
    internal class CheatsOptionsButton : OptionsElement
    {
        /*********
        ** Fields
        *********/
        /// <summary>The action to perform when the button is toggled (or <c>null</c> to handle it manually).</summary>
        private readonly Action OnToggled;

        /// <summary>The source rectangle for the 'set' button sprite.</summary>
        private readonly Rectangle SetButtonSprite = new Rectangle(294, 428, 21, 11);

        /// <summary>The button area in screen pixels.</summary>
        private Rectangle SetButtonBounds;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="slotWidth">The field width.</param>
        /// <param name="onToggled">The action to perform when the button is toggled.</param>
        /// <param name="disabled">Whether the button should be disabled.</param>
        public CheatsOptionsButton(string label, int slotWidth, Action onToggled, bool disabled = false)
          : base(label, -1, -1, slotWidth + 1, 11 * Game1.pixelZoom)
        {
            this.SetButtonBounds = new Rectangle(slotWidth - 28 * Game1.pixelZoom, -1 + Game1.pixelZoom * 3, 21 * Game1.pixelZoom, 11 * Game1.pixelZoom);
            this.OnToggled = onToggled;
            this.greyedOut = disabled;
        }

        /// <summary>The method invoked when the player clicks the left mouse button.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut || !this.SetButtonBounds.Contains(x, y))
                return;

            // callback handler
            this.OnToggled();
        }

        /// <summary>Draw the menu to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            Utility.drawTextWithShadow(spriteBatch, this.label, Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), this.greyedOut ? Game1.textColor * 0.33f : Game1.textColor, 1f, 0.15f);
            Utility.drawWithShadow(spriteBatch, Game1.mouseCursors, new Vector2(this.SetButtonBounds.X + slotX, this.SetButtonBounds.Y + slotY), this.SetButtonSprite, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, false, 0.15f);
        }
    }
}

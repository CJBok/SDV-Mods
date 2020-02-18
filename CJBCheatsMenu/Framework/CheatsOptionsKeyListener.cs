using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu.Framework
{
    /// <summary>A button which lets the user choose a key binding.</summary>
    internal class CheatsOptionsKeyListener : OptionsElement
    {
        /*********
        ** Fields
        *********/
        /// <summary>The current key binding.</summary>
        private SButton CurrentValue;

        /// <summary>The action to perform when the key binding is chosen.</summary>
        private readonly Action<SButton> OnSet;

        /// <summary>The translated 'press new key' label.</summary>
        private readonly string PressNewKeyLabel;

        /// <summary>The source rectangle for the 'set' button sprite.</summary>
        private readonly Rectangle SetButtonSprite = new Rectangle(294, 428, 21, 11);

        /// <summary>The button area in screen pixels.</summary>
        private Rectangle SetButtonBounds;

        /// <summary>The button to set when the player clears it.</summary>
        private readonly SButton ClearToButton;

        private string ListenerMessage;
        private bool Listening;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="label">The field label.</param>
        /// <param name="slotWidth">The field width.</param>
        /// <param name="currentValue">The current key binding.</param>
        /// <param name="onSet">The action to perform when the button is toggled.</param>
        /// <param name="i18n">Provides translations for the mod.</param>
        /// <param name="clearToButton">The button to set when the player clears it.</param>
        public CheatsOptionsKeyListener(string label, int slotWidth, SButton currentValue, Action<SButton> onSet, ITranslationHelper i18n, SButton clearToButton = SButton.None)
          : base(label, -1, -1, slotWidth + 1, 11 * Game1.pixelZoom)
        {
            this.CurrentValue = currentValue;
            this.PressNewKeyLabel = i18n.Get("messages.press-new-key");
            this.OnSet = onSet;
            this.SetButtonBounds = new Rectangle(slotWidth - 28 * Game1.pixelZoom, -1 + Game1.pixelZoom * 3, 21 * Game1.pixelZoom, 11 * Game1.pixelZoom);
            this.ClearToButton = clearToButton;
        }

        /// <summary>The method invoked when the player clicks the left mouse button.</summary>
        /// <param name="x">The X-position of the cursor.</param>
        /// <param name="y">The Y-position of the cursor.</param>
        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut || this.Listening || !this.SetButtonBounds.Contains(x, y))
                return;

            this.Listening = true;
            Game1.soundBank.PlayCue("breathin");
            GameMenu.forcePreventClose = true;
            this.ListenerMessage = this.PressNewKeyLabel;
        }

        /// <summary>The method invoked when the player presses a key.</summary>
        /// <param name="key">The key that was pressed.</param>
        public override void receiveKeyPress(Keys key)
        {
            if (this.greyedOut || !this.Listening)
                return;

            if (key == Keys.Escape)
            {
                this.CurrentValue = this.ClearToButton;
                Game1.soundBank.PlayCue("bigDeSelect");
            }
            else
            {
                this.CurrentValue = key.ToSButton();
                Game1.soundBank.PlayCue("coin");
            }

            this.OnSet(this.CurrentValue);
            this.Listening = false;
            GameMenu.forcePreventClose = false;
        }

        /// <summary>Draw the menu to the screen.</summary>
        /// <param name="spriteBatch">The sprite batch being drawn.</param>
        /// <param name="slotX">The X position at which to draw relative to the bounds.</param>
        /// <param name="slotY">The Y position at which to draw relative to the bounds.</param>
        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            Utility.drawTextWithShadow(spriteBatch, $"{this.label}: {this.CurrentValue}", Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), this.greyedOut ? Game1.textColor * 0.33f : Game1.textColor, 1f, 0.15f);
            Utility.drawWithShadow(spriteBatch, Game1.mouseCursors, new Vector2(this.SetButtonBounds.X + slotX, this.SetButtonBounds.Y + slotY), this.SetButtonSprite, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, false, 0.15f);

            if (this.Listening)
            {
                spriteBatch.Draw(Game1.staminaRect, new Rectangle(0, 0, Game1.graphics.GraphicsDevice.Viewport.Width, Game1.graphics.GraphicsDevice.Viewport.Height), new Rectangle(0, 0, 1, 1), Color.Black * 0.75f, 0.0f, Vector2.Zero, SpriteEffects.None, 0.999f);
                spriteBatch.DrawString(Game1.dialogueFont, this.ListenerMessage, Utility.getTopLeftPositionForCenteringOnScreen(Game1.tileSize * 3, Game1.tileSize), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9999f);
            }
        }
    }
}

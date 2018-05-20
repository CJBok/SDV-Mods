using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// Renders a key picker option.
    /// </summary>
    internal class ViewOptionKeyPicker : ViewOptionSetButtonBase<Menu.IOptionKeyPicker>
    {
        /// <summary>
        /// Helper object used to tranlate strings to desired language.
        /// </summary>
        private StardewModdingAPI.ITranslationHelper I18n { get; set; }

        /// <summary>
        /// true if this option is currently listening for a new key press to set the selected key, false otherwise.
        /// </summary>
        private bool Listening { get; set; } = false;

        /// <summary>
        /// The message displayed when listening for a key.
        /// </summary>
        private string ListenerMessage => this.I18n.Get("messages.press-new-key");

        /// <summary>
        /// Controller used to display the "Press any key..." label when listening for a key.
        /// </summary>
        private IOverlayController OverlayController { get; set; }

        /// <summary>
        /// Constructor for a option which allows the user to choose a key.
        /// </summary>
        /// <param name="keyPicker">The underlying key picker option being rendered.</param>
        /// <param name="containerWidth">The width of the row the option is being rendered in.</param>
        /// <param name="overlayController">The controller that can display or hide an overlay message.</param>
        /// <param name="i18n"></param>
        public ViewOptionKeyPicker(Menu.IOptionKeyPicker keyPicker, int containerWidth, IOverlayController overlayController, StardewModdingAPI.ITranslationHelper i18n)
            : base(keyPicker, containerWidth)
        {
            this.OverlayController = overlayController;
            this.I18n = i18n;
        }

        /// <summary>
        /// Called when a left click occurs, starts listening for a key press.
        /// </summary>
        /// <param name="x">x position of the left click.</param>
        /// <param name="y">y position of the left click.</param>
        public override void receiveLeftClick(int x, int y)
        {
            if (!this.greyedOut && !this.Listening && this.SetButtonBounds.Contains(x, y))
            {
                this.Listening = true;
                StardewValley.Game1.soundBank.PlayCue("breathin");
                StardewValley.Menus.GameMenu.forcePreventClose = true;
                this.OverlayController.ShowOverlay(this.ListenerMessage);
            }
        }

        /// <summary>
        /// Called when a keyboard key is pressed, sets the new key if we are listening for key input.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        public override void receiveKeyPress(Keys key)
        {
            if (this.greyedOut || !this.Listening)
                return;
            if (key == Keys.Escape)
            {
                StardewValley.Game1.soundBank.PlayCue("bigDeSelect");
            }
            else
            {
                this.Option.Value = key;
                StardewValley.Game1.soundBank.PlayCue("coin");
            }
            this.Listening = false;
            this.OverlayController.HideOverlay();
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
            StardewValley.Utility.drawTextWithShadow(spriteBatch, this.label + ": " + this.Option.Value.ToString(), StardewValley.Game1.dialogueFont, new Vector2(this.bounds.X + slotX, this.bounds.Y + slotY), this.greyedOut ? StardewValley.Game1.textColor * 0.33f : StardewValley.Game1.textColor, 1f, 0.15f);
        }
    }
}

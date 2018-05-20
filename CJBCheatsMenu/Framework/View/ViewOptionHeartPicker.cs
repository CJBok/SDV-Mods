using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CJBCheatsMenu.Framework.View
{
    /// <summary>
    /// Renders a heart picker option.
    /// </summary>
    internal class ViewOptionHeartPicker : ViewOption<Menu.IOptionHeartPicker>
    {
        /// <summary>
        /// The maximum number of hearts that can be picked.
        /// </summary>
        /// <remarks>
        /// This needs to be increase for your spouse, perhaps there is something within NPC that can help.
        /// At which point it can't be static anymore.
        /// </remarks>
        private static int MAX_HEARTS = 10;

        /// <summary>
        /// The npc used to when caching the current mugshot used for rendering next to this option.
        /// </summary>
        private StardewValley.NPC npcForCurrentMugshot;

        /// <summary>
        /// The currently cached mugshot rendered next to this option.
        /// </summary>
        private StardewValley.Menus.ClickableTextureComponent cachedMugshot;

        /// <summary>
        /// Gets the mugshot to render based on the current NPC.
        /// </summary>
        private StardewValley.Menus.ClickableTextureComponent Mugshot
        {
            get
            {
                if (this.Option.NPC != npcForCurrentMugshot)
                {
                    this.npcForCurrentMugshot = this.Option.NPC;
                    cachedMugshot = new StardewValley.Menus.ClickableTextureComponent("Mugshot", this.bounds, "", "", this.Option.NPC.Sprite.Texture, this.Option.NPC.getMugShotSourceRect(), 0.7f * StardewValley.Game1.pixelZoom);
                }
                return cachedMugshot;
            }
        }

        /// <summary>
        /// Contructor for the heart picker option.
        /// </summary>
        /// <param name="heartPickerOption">The underlying heart picker option to render.</param>
        public ViewOptionHeartPicker(Menu.IOptionHeartPicker heartPickerOption)
            : base(heartPickerOption, 80 * StardewValley.Game1.pixelZoom, 6 * StardewValley.Game1.pixelZoom, 0)
        {
            this.bounds.X = 96;
        }

        /// <summary>
        /// Changes the heart value selected.
        /// </summary>
        /// <param name="x">The x position of the mouse drag.</param>
        /// <param name="y">The y position of the mouse drag.</param>
        public override void leftClickHeld(int x, int y)
        {
            if (this.greyedOut)
                return;

            base.leftClickHeld(x, y);
            this.Option.Value = x >= this.bounds.X ? (x <= this.bounds.Right - 10 * StardewValley.Game1.pixelZoom ? (int)((x - this.bounds.X) / (this.bounds.Width - 10d * StardewValley.Game1.pixelZoom) * MAX_HEARTS) : MAX_HEARTS) : 0;
        }

        /// <summary>
        /// Called when a left click occurs, starts the left click held logic.
        /// </summary>
        /// <param name="x">x position of the left click.</param>
        /// <param name="y">y position of the left click.</param>
        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;
            base.receiveLeftClick(x, y);
            this.leftClickHeld(x, y);
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

            if (this.Mugshot != null)
            {
                this.Mugshot.bounds = new Rectangle(slotX + 32, slotY, StardewValley.Game1.tileSize, StardewValley.Game1.tileSize);
                this.Mugshot.draw(spriteBatch);
            }

            for (int i = 0; i < 10; i++)
            {
                Rectangle sourceRectangle = i < this.Option.Value
                    ? new Rectangle(211, 428, 7, 6)
                    : new Rectangle(218, 428, 7, 6);
                spriteBatch.Draw(StardewValley.Game1.mouseCursors, new Vector2(slotX + this.bounds.X + i * (8 * StardewValley.Game1.pixelZoom), slotY + this.bounds.Y), sourceRectangle, Color.White, 0f, Vector2.Zero, StardewValley.Game1.pixelZoom, SpriteEffects.None, 0.88f);
            }
        }
    }
}

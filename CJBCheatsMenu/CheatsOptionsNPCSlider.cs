using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu
{
    internal class CheatsOptionsNPCSlider : OptionsElement
    {
        /*********
        ** Properties
        *********/
        private readonly string SliderLabel;
        private readonly int SliderMaxValue;
        private readonly NPC Npc;
        private readonly ClickableTextureComponent Mugshot;
        private int Value;


        /*********
        ** Public methods
        *********/
        public CheatsOptionsNPCSlider(NPC npc, int whichOption, int maxValue = 10, int x = 96, int y = -1, int width = 80)
            : base(npc.getName(), x, y, width * Game1.pixelZoom, 6 * Game1.pixelZoom, whichOption)
        {
            this.Npc = npc;
            this.SliderLabel = npc.getName();
            this.SliderMaxValue = maxValue;

            this.Mugshot = new ClickableTextureComponent("Mugshot", this.bounds, "", "", npc.sprite.Texture, npc.getMugShotSourceRect(), 0.7f * Game1.pixelZoom);

            if (Game1.player.friendships.ContainsKey(npc.name))
                this.Value = Math.Max(Game1.player.friendships[npc.name][0] / 250, 10);
        }

        public override void leftClickHeld(int x, int y)
        {
            if (this.greyedOut)
                return;

            base.leftClickHeld(x, y);
            this.Value = x >= this.bounds.X ? (x <= this.bounds.Right - 10 * Game1.pixelZoom ? (int)((x - this.bounds.X) / (this.bounds.Width - 10d * Game1.pixelZoom) * this.SliderMaxValue) : this.SliderMaxValue) : 0;

            if (Game1.player.friendships.ContainsKey(this.Npc.name))
                Game1.player.friendships[this.Npc.name][0] = this.Value * 250;
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (this.greyedOut)
                return;
            base.receiveLeftClick(x, y);
            this.leftClickHeld(x, y);
        }

        public override void draw(SpriteBatch spriteBatch, int slotX, int slotY)
        {
            this.label = this.SliderLabel;

            this.greyedOut = false;
            base.draw(spriteBatch, slotX, slotY);

            if (this.Mugshot != null)
            {
                this.Mugshot.bounds = new Rectangle(slotX + 32, slotY, Game1.tileSize, Game1.tileSize);
                this.Mugshot.draw(spriteBatch);
            }

            for (int i = 0; i < 10; i++)
            {
                Rectangle sourceRectangle = i < this.Value
                    ? new Rectangle(211, 428, 7, 6)
                    : new Rectangle(218, 428, 7, 6);
                spriteBatch.Draw(Game1.mouseCursors, new Vector2(slotX + this.bounds.X + i * (8 * Game1.pixelZoom), slotY + this.bounds.Y), sourceRectangle, Color.White, 0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.88f);
            }
        }
    }
}

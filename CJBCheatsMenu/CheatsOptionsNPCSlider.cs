using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu {
    class CheatsOptionsNPCSlider : OptionsElement 
        {
        public static Rectangle sliderBGSource = new Rectangle(403, 383, 6, 6);
        public static Rectangle sliderButtonRect = new Rectangle(420, 441, 10, 6);
        public const int pixelsWide = 48;
        public const int pixelsHigh = 6;
        public const int sliderButtonWidth = 10;
        public int sliderMaxValue = 100;
        public int value;
        public NPC npc = null;
        public ClickableTextureComponent mugshot;

        public string sliderLabel = "";

        public CheatsOptionsNPCSlider(NPC npc, int whichOption, int maxValue = 10, int x = 96, int y = -1, int width = 80)
            : base(npc.getName(), x, y, width * Game1.pixelZoom, 6 * Game1.pixelZoom, whichOption)
        {
            this.npc = npc;
            sliderLabel = npc.getName();
            sliderMaxValue = maxValue;

            mugshot = new ClickableTextureComponent("mugshot", this.bounds, "", "", npc.sprite.Texture, npc.getMugShotSourceRect(), 0.7f * Game1.pixelZoom);


            if (Game1.player.friendships.ContainsKey(npc.name)) {
                value = Game1.player.friendships[npc.name][0] / 250;
                if (value > 10)
                    value = 10;
            }
        }

        public override void leftClickHeld(int x, int y) {
            if (this.greyedOut)
                return;

            base.leftClickHeld(x, y);
            this.value = x >= this.bounds.X ? (x <= this.bounds.Right - 10 * Game1.pixelZoom ? (int)((double)((float)(x - this.bounds.X) / (float)(this.bounds.Width - 10 * Game1.pixelZoom)) * this.sliderMaxValue) : this.sliderMaxValue) : 0;

            if (Game1.player.friendships.ContainsKey(npc.name)) {
                Game1.player.friendships[npc.name][0] = value * 250;
            }
        }

        public override void receiveLeftClick(int x, int y) {
            if (this.greyedOut)
                return;
            base.receiveLeftClick(x, y);
            this.leftClickHeld(x, y);
        }

        public override void draw(SpriteBatch b, int slotX, int slotY) {
            base.label = this.sliderLabel;

            this.greyedOut = false;
            base.draw(b, slotX, slotY);

            if (mugshot != null) {
                mugshot.bounds = new Rectangle(slotX + 32, slotY, Game1.tileSize, Game1.tileSize);
                mugshot.draw(b);
            }

            //IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsSlider.sliderBGSource, slotX + this.bounds.X, slotY + this.bounds.Y, this.bounds.Width, this.bounds.Height, Color.White, (float)Game1.pixelZoom, false);
            //b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X) + (float)(this.bounds.Width - 10 * Game1.pixelZoom) * ((float)this.value / (float)this.sliderMaxValue), (float)(slotY + this.bounds.Y)), new Rectangle?(OptionsSlider.sliderButtonRect), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.9f);

            for (int j = 0; j < 10; j++) {
                if (j < value)
                    b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X + j * (8 * Game1.pixelZoom)), (float)(slotY + this.bounds.Y)), new Rectangle?(new Rectangle(211, 428, 7, 6)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
                else
                    b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X + j * (8 * Game1.pixelZoom)), (float)(slotY + this.bounds.Y)), new Rectangle?(new Rectangle(218, 428, 7, 6)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
            }
        }
    }
}

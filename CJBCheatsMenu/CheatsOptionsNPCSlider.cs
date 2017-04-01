using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu
{
    class CheatsOptionsNPCSlider : OptionsElement
    {
        /*********
        ** Accessors
        *********/
        public static Rectangle SliderBackgroundSprite = new Rectangle(403, 383, 6, 6);
        public static Rectangle SliderButtonSprite = new Rectangle(420, 441, 10, 6);
        public const int Width = 48;
        public const int Height = 6;
        public const int SliderButtonWidth = 10;
        public int SliderMaxValue = 100;
        public int Value;
        public NPC Npc = null;
        public ClickableTextureComponent Mugshot;

        public string SliderLabel = "";


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
            {
                this.Value = Game1.player.friendships[npc.name][0] / 250;
                if (this.Value > 10)
                    this.Value = 10;
            }
        }

        public override void leftClickHeld(int x, int y)
        {
            if (this.greyedOut)
                return;

            base.leftClickHeld(x, y);
            this.Value = x >= this.bounds.X ? (x <= this.bounds.Right - 10 * Game1.pixelZoom ? (int)((double)((float)(x - this.bounds.X) / (float)(this.bounds.Width - 10 * Game1.pixelZoom)) * this.SliderMaxValue) : this.SliderMaxValue) : 0;

            if (Game1.player.friendships.ContainsKey(this.Npc.name))
            {
                Game1.player.friendships[this.Npc.name][0] = this.Value * 250;
            }
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
            base.label = this.SliderLabel;

            this.greyedOut = false;
            base.draw(spriteBatch, slotX, slotY);

            if (this.Mugshot != null)
            {
                this.Mugshot.bounds = new Rectangle(slotX + 32, slotY, Game1.tileSize, Game1.tileSize);
                this.Mugshot.draw(spriteBatch);
            }

            //IClickableMenu.drawTextureBox(spriteBatch, Game1.mouseCursors, OptionsSlider.SliderBackgroundSprite, slotX + this.bounds.X, slotY + this.bounds.Y, this.bounds.Width, this.bounds.Height, Color.White, (float)Game1.pixelZoom, false);
            //spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X) + (float)(this.bounds.Width - 10 * Game1.pixelZoom) * ((float)this.Value / (float)this.SliderMaxValue), (float)(slotY + this.bounds.Y)), new Rectangle?(OptionsSlider.SliderButtonSprite), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.9f);

            for (int j = 0; j < 10; j++)
            {
                if (j < this.Value)
                    spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X + j * (8 * Game1.pixelZoom)), (float)(slotY + this.bounds.Y)), new Rectangle?(new Rectangle(211, 428, 7, 6)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
                else
                    spriteBatch.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X + j * (8 * Game1.pixelZoom)), (float)(slotY + this.bounds.Y)), new Rectangle?(new Rectangle(218, 428, 7, 6)), Color.White, 0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.88f);
            }
        }
    }
}

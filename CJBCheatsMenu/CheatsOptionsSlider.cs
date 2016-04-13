using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu {
    class CheatsOptionsSlider : OptionsElement 
        {
        public static Rectangle sliderBGSource = new Rectangle(403, 383, 6, 6);
        public static Rectangle sliderButtonRect = new Rectangle(420, 441, 10, 6);
        public const int pixelsWide = 48;
        public const int pixelsHigh = 6;
        public const int sliderButtonWidth = 10;
        public int sliderMaxValue = 100;
        public int value;

        public string sliderLabel = "";

        public CheatsOptionsSlider(string label, int whichOption, int maxValue = 100, int x = -1, int y = -1, int width = 48)
            : base(label, x, y, width * Game1.pixelZoom, 6 * Game1.pixelZoom, whichOption)
        {
            sliderLabel = label;
            sliderMaxValue = maxValue;
            switch (whichOption) {
                case 0:
                    value = CJBCheatsMenu.config.moveSpeed;
                    break;
                case 10:
                    value = (Game1.timeOfDay - 600) / 100;
                    break;
            }
        }

        public override void leftClickHeld(int x, int y) {
            if (this.greyedOut)
                return;

            base.leftClickHeld(x, y);
            this.value = x >= this.bounds.X ? (x <= this.bounds.Right - 10 * Game1.pixelZoom ? (int)((double)((float)(x - this.bounds.X) / (float)(this.bounds.Width - 10 * Game1.pixelZoom)) * this.sliderMaxValue) : this.sliderMaxValue) : 0;

            switch (whichOption) {
                case 0:
                    CJBCheatsMenu.config.moveSpeed = value;
                    CJBCheatsMenu.config.WriteConfig();
                    break;
                case 10:
                    Game1.timeOfDay = this.value * 100 + 600;
                    break;
            }
        }

        public override void receiveLeftClick(int x, int y) {
            if (this.greyedOut)
                return;
            base.receiveLeftClick(x, y);
            this.leftClickHeld(x, y);
        }

        public override void draw(SpriteBatch b, int slotX, int slotY) {
            base.label = this.sliderLabel + ": " + this.value;

            this.greyedOut = false;

            switch (whichOption) {
                case 0:
                    this.greyedOut = !CJBCheatsMenu.config.increasedMovement;
                    break;
                case 10:
                    string ampm = (Game1.timeOfDay < 1200 || Game1.timeOfDay >= 2400) ? "am" : "pm";
                    string hours = (Game1.timeOfDay / 100 % 12 == 0) ? "12" : string.Concat(Game1.timeOfDay / 100 % 12);
                    if (hours.Length == 1)
                        hours = "0" + hours;
                    string minutes = Game1.timeOfDay % 100 + "";
                    if (minutes.Length == 1)
                        minutes = "0" + minutes;

                    base.label = this.sliderLabel + ": " + hours + ":" + minutes + " " + ampm;
                    break;
            }

            base.draw(b, slotX, slotY);
            IClickableMenu.drawTextureBox(b, Game1.mouseCursors, OptionsSlider.sliderBGSource, slotX + this.bounds.X, slotY + this.bounds.Y, this.bounds.Width, this.bounds.Height, Color.White, (float)Game1.pixelZoom, false);
            b.Draw(Game1.mouseCursors, new Vector2((float)(slotX + this.bounds.X) + (float)(this.bounds.Width - 10 * Game1.pixelZoom) * ((float)this.value / (float)this.sliderMaxValue), (float)(slotY + this.bounds.Y)), new Rectangle?(OptionsSlider.sliderButtonRect), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom, SpriteEffects.None, 0.9f);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace CJBCheatsMenu
{
    internal class CheatsOptionsSlider : OptionsElement
    {
        /*********
        ** Properties
        *********/
        private readonly string SliderLabel;
        private readonly int SliderMaxValue;
        private int Value;


        /*********
        ** Public methods
        *********/
        public CheatsOptionsSlider(string label, int whichOption, int maxValue = 100, int x = -1, int y = -1, int width = 48)
            : base(label, x, y, width * Game1.pixelZoom, 6 * Game1.pixelZoom, whichOption)
        {
            this.SliderLabel = label;
            this.SliderMaxValue = maxValue;
            switch (whichOption)
            {
                case 0:
                    this.Value = CJBCheatsMenu.Config.MoveSpeed;
                    break;
                case 10:
                    this.Value = (Game1.timeOfDay - 600) / 100;
                    break;
            }
        }

        public override void leftClickHeld(int x, int y)
        {
            if (this.greyedOut)
                return;

            base.leftClickHeld(x, y);
            this.Value = x >= this.bounds.X ? (x <= this.bounds.Right - 10 * Game1.pixelZoom ? (int)((x - this.bounds.X) / (this.bounds.Width - 10d * Game1.pixelZoom) * this.SliderMaxValue) : this.SliderMaxValue) : 0;

            switch (whichOption)
            {
                case 0:
                    CJBCheatsMenu.Config.MoveSpeed = this.Value;
                    CJBCheatsMenu.SaveConfig();
                    break;
                case 10:
                    Game1.timeOfDay = this.Value * 100 + 600;
                    break;
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
            this.label = $"{this.SliderLabel}: {this.Value}";

            this.greyedOut = false;

            switch (whichOption)
            {
                case 0:
                    this.greyedOut = !CJBCheatsMenu.Config.IncreasedMovement;
                    break;
                case 10:
                    string ampm = (Game1.timeOfDay < 1200 || Game1.timeOfDay >= 2400) ? "am" : "pm";
                    string hours = (Game1.timeOfDay / 100 % 12 == 0) ? "12" : string.Concat(Game1.timeOfDay / 100 % 12);
                    if (hours.Length == 1)
                        hours = "0" + hours;
                    string minutes = Game1.timeOfDay % 100 + "";
                    if (minutes.Length == 1)
                        minutes = "0" + minutes;

                    this.label = $"{this.SliderLabel}:{hours}:{minutes} {ampm}";
                    break;
            }

            base.draw(spriteBatch, slotX, slotY);
            IClickableMenu.drawTextureBox(spriteBatch, Game1.mouseCursors, OptionsSlider.sliderBGSource, slotX + this.bounds.X, slotY + this.bounds.Y, this.bounds.Width, this.bounds.Height, Color.White, Game1.pixelZoom, false);
            spriteBatch.Draw(Game1.mouseCursors, new Vector2(slotX + this.bounds.X + (this.bounds.Width - 10 * Game1.pixelZoom) * (this.Value / (float)this.SliderMaxValue), slotY + this.bounds.Y), OptionsSlider.sliderButtonRect, Color.White, 0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.9f);
        }
    }
}
